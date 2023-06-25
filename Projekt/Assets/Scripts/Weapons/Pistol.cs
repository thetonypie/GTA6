using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    public float range;
    public int damage;
    public float fireRate;
    public int maxAmmo;
    public int ammoAmount;
    public int ammoClipSize;
    public int ammoLeft;
    public int ammoClipLeft;
    bool isShot;
    float timer;
    bool isReloading;

    void Awake()
    {
        ammoLeft = ammoAmount;
        ammoClipLeft = ammoClipSize;
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && timer <= 0)
        {
            isShot = true;
            timer = fireRate;
        }

        if (Input.GetKeyDown(KeyCode.R) && isReloading == false && ammoClipLeft != ammoClipSize)
        {
            Reload();
        }

        ammoLeft = Mathf.Clamp(ammoLeft, 0, maxAmmo);
    }

    void FixedUpdate()
    {
        timer -= Time.deltaTime;
        Vector2 bulletOffset = Random.insideUnitCircle * 5;
        // Tworzymy promieñ, który wychodzi od naszej kamery do pozycji myszki
        Vector3 randomtarget = new Vector3(Screen.width / 2 + bulletOffset.x, Screen.height / 2 + bulletOffset.y, 0);
        Ray ray = Camera.main.ScreenPointToRay(randomtarget);
        RaycastHit hit;
        if (isShot == true && ammoClipLeft > 0 && isReloading == false)
        {
            isShot = false;
            ammoClipLeft--;
            //Jesli po wcisnieciu przycisku 'Fire1' promien wszedl w kolizje z jakims obiektem
            //Wykonuje ponizsze instrukcje
            if (Physics.Raycast(ray, out hit, range))
            {
                if (hit.transform.CompareTag("Enemy"))
                {
                    hit.collider.gameObject.GetComponent<Enemy>().AddDamage(damage);
                }

                Debug.Log("Wszedlem w kolizje z " + hit.collider.gameObject.name);
            }
        }

        else if (isShot == true && ammoClipLeft <= 0 && isReloading == false)
        {
            isShot = false;
            Reload();
        }
    }

    void Reload()
    {
        int bulletsToReload = ammoClipSize - ammoClipLeft;
        if (ammoLeft >= bulletsToReload)
        {
            StartCoroutine("ReloadWeapon");
            ammoLeft -= bulletsToReload;
            ammoClipLeft = ammoClipSize;
        }

        else if (ammoLeft < bulletsToReload && ammoLeft > 0)
        {
            StartCoroutine("ReloadWeapon");
            ammoClipLeft += ammoLeft;
            ammoLeft = 0;
        }

        else if (ammoLeft <= 0)
        {
            Debug.Log("No ammo");
        }
    }

    public void AddAmmo(int value)
    {
        ammoLeft += value;
    }

    IEnumerator ReloadWeapon()
    {
        isReloading = true;
        yield return new WaitForSeconds(1f);
        isReloading = false;
    }
}
