using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SniperRifle : MonoBehaviour
{
    public AudioClip rifleSound;
    public AudioSource aS;

    public float snipeRange;
    public int snipeDamage;
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

    public bool isSniping; // Dodana flaga snajpowania
    public float snipeZoomFOV; // Powiêkszenie pola widzenia podczas snajpowania
    private float originalFOV; // Oryginalne pole widzenia

    public float shakeDuration = 0.1f; // Czas trzêsienia
    public float shakeMagnitude = 0.1f; // Intensywnoœæ trzêsienia

    private Vector3 originalPosition; // Oryginalna pozycja obiektu
    private float shakeTimer = 0f; // Licznik czasu trzêsienia

    void Awake()
    {
        ammoLeft = ammoAmount;
        ammoClipLeft = ammoClipSize;
        UpdateAmmoText();

        originalFOV = Camera.main.fieldOfView;

        originalPosition = transform.localPosition; // Zapisanie oryginalnej pozycji obiektu
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

        // Obs³uga snajpowania
        if (Input.GetMouseButtonDown(1)) // Naciœniêcie prawego przycisku myszy
        {
            isSniping = true;
            Camera.main.fieldOfView = snipeZoomFOV;
        }
        else if (Input.GetMouseButtonUp(1)) // Zwolnienie prawego przycisku myszy
        {
            isSniping = false;
            Camera.main.fieldOfView = originalFOV;
        }
    }

    void FixedUpdate()
    {
        timer -= Time.deltaTime;
        Vector2 bulletOffset = Random.insideUnitCircle * 5;
        Vector3 randomTarget = new Vector3(Screen.width / 2 + bulletOffset.x, Screen.height / 2 + bulletOffset.y, 0);
        Ray ray = Camera.main.ScreenPointToRay(randomTarget);
        RaycastHit hit;

        if (isShot && ammoClipLeft > 0 && !isReloading && !isSniping)
        {
            isShot = false;
            ammoClipLeft--;
            aS.PlayOneShot(rifleSound);

            if (Physics.Raycast(ray, out hit, snipeRange))
            {
                if (hit.transform.CompareTag("Enemy"))
                {
                    hit.collider.gameObject.GetComponent<Enemy>().AddDamage(snipeDamage);
                }

                Debug.Log("Collided with " + hit.collider.gameObject.name);
                Instantiate(bulletHole, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));

            }

            // Trzêsienie obiektu
            StartCoroutine(Shake());

            // Spawn bullet prefab
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
            Destroy(bullet, 2f);
        }
        else if (isShot && ammoClipLeft <= 0 && !isReloading && !isSniping)
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

    IEnumerator Shake()
    {
        shakeTimer = shakeDuration; // Ustawienie czasu trzêsienia

        while (shakeTimer > 0)
        {
            transform.localPosition = originalPosition + Random.insideUnitSphere * shakeMagnitude;

            shakeTimer -= Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPosition; // Przywrócenie oryginalnej pozycji obiektu po zakoñczeniu trzêsienia
    }
}
