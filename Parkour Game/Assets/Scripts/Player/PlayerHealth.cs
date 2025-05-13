using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Playerhealth : MonoBehaviour
{

    private float health;
    public float maxHealth = 100f;

    public RectTransform healthBar;

    void Start()
    {
        health = maxHealth;
    }
    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdatehealthUI();
    }
    public void UpdatehealthUI ()
    {

        float healthPercentage = health / maxHealth;
        healthBar.localScale = new Vector3(healthPercentage, 1, 1);

    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }

    public void HealPlayer(float heal)
    {
        health += heal;
    }


}
