using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100; // Maksymalne zdrowie
    private int currentHealth; // Obecne zdrowie

    private Renderer renderer;
    private Color originalColor;

    void Start()
    {
        currentHealth = maxHealth;
        renderer = GetComponent<Renderer>();
        originalColor = renderer.material.color;
    }

    public void AddDamage(int damage)
    {
        currentHealth -= damage;
        StartCoroutine(FlashColor(Color.red, 0.1f)); // Zmiana koloru na czerwony

        if (currentHealth <= 0)
        {
            Destroy(gameObject); // Zniszczenie obiektu, gdy zdrowie osi¹gnie 0
        }
    }

    IEnumerator FlashColor(Color color, float duration)
    {
        renderer.material.color = color;
        yield return new WaitForSeconds(duration);
        renderer.material.color = originalColor;
    }
}
