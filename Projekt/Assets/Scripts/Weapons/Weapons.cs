using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    public List<Transform> weapons;
    public int initialWeapon;
    public bool autoFill;
    int selectedWeapon;

    private void Awake()
    {
        if (autoFill)
        {
            weapons.Clear();
            foreach (Transform weapon in transform)
                weapons.Add(weapon);
        }
    }

    void Start()
    {
        selectedWeapon = initialWeapon;
        UpdateWeapon();
    }

    void Update()
    {
        // Scroll, aby zmieniæ broñ
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedWeapon > 0)
            selectedWeapon = selectedWeapon - 1;
        else if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedWeapon <= 0)
            selectedWeapon = weapons.Count - 1;
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
            selectedWeapon = Mathf.Abs(selectedWeapon + 1) % weapons.Count;

        // Klawisze, aby zmieniæ broñ
        if (Input.GetKeyDown(KeyCode.Alpha1))
            selectedWeapon = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2) && weapons.Count > 1)
            selectedWeapon = 1;
        if (Input.GetKeyDown(KeyCode.Alpha2) && weapons.Count > 2)
            selectedWeapon = 2;

        UpdateWeapon();
    }

    void UpdateWeapon()
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            if (i == selectedWeapon)
                weapons[i].gameObject.SetActive(true);
            else
                weapons[i].gameObject.SetActive(false);
        }
    }
}