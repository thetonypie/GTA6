using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Pistol : MonoBehaviour
{
    public AudioClip pistolSound;
    public AudioSource aS;

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

    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public TextMeshProUGUI ammoText;

    public GameObject bulletHole;

    void Awake()
    {
        ammoLeft = ammoAmount;
        ammoClipLeft = ammoClipSize;
        UpdateAmmoText();
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
        UpdateAmmoText();
    }

    void FixedUpdate()
    {
        timer -= Time.deltaTime;
        Vector2 bulletOffset = Random.insideUnitCircle * 5;
        Vector3 randomTarget = new Vector3(Screen.width / 2 + bulletOffset.x, Screen.height / 2 + bulletOffset.y, 0);
        Ray ray = Camera.main.ScreenPointToRay(randomTarget);
        RaycastHit hit;

        if (isShot && ammoClipLeft > 0 && !isReloading)
        {
            isShot = false;
            ammoClipLeft--;
            aS.PlayOneShot(pistolSound);

            if (Physics.Raycast(ray, out hit, range))
            {
                if (hit.transform.CompareTag("Enemy"))
                {
                    hit.collider.gameObject.GetComponent<Enemy>().AddDamage(damage);
                }

                Debug.Log("Collided with " + hit.collider.gameObject.name);
                Instantiate(bulletHole, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                
            }

            // Spawn bullet prefab
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
            Destroy(bullet, 2f);
        }
        else if (isShot && ammoClipLeft <= 0 && !isReloading)
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
            StartCoroutine(ReloadWeapon());
            ammoLeft -= bulletsToReload;
            ammoClipLeft = ammoClipSize;
        }
        else if (ammoLeft < bulletsToReload && ammoLeft > 0)
        {
            StartCoroutine(ReloadWeapon());
            ammoClipLeft += ammoLeft;
            ammoLeft = 0;
        }
        else if (ammoLeft <= 0)
        {
            Debug.Log("No ammo");
        }

        UpdateAmmoText();
    }

    public void AddAmmo(int value)
    {
        ammoLeft += value;
        UpdateAmmoText();
    }

    IEnumerator ReloadWeapon()
    {
        isReloading = true;
        yield return new WaitForSeconds(1f);
        isReloading = false;
    }

    void UpdateAmmoText()
    {
        ammoText.text = ammoClipLeft + " / " + ammoLeft;
    }
}
