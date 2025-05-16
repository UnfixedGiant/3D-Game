using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    private float health;
    public float maxHealth = 100f;
    // variable for the healthbar.(Change size to simulate player losing/gaining health)
    public RectTransform healthBar;

    public GameObject Player { get => player; }
    private GameObject player;

    // Sets the players health to their max health.
    void Start()
    {
        health = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player");
    }
    // Clamps the players health between 0 and max health.
    // Updates the healthbar.
    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdatehealthUI();

    }
    // Updates players health bar to represent the players actual health.
    public void UpdatehealthUI ()
    {
        // Percentage of the the players remaining health.
        float healthPercentage = health / maxHealth;
        // Changes the scale of the health bar to match the health %
        healthBar.localScale = new Vector3(healthPercentage, 1, 1);

    }

    // Handles the player taking damage.
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(player);
        }
    }

    // Handles the player gaining health.
    public void HealPlayer(float heal)
    {
        health += heal;
    }


}
