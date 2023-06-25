using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public void AddDamage(int damage)
    {
        Debug.Log("Ouch, " + damage);
    }
}
