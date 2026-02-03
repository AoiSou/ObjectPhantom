using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPBre : MonoBehaviour
{
    public float maxHealth;
    public float health;
    
    [SerializeField] Image healthBar;

    private void Update()
    {
        //test
        // health -= 0.1f;
        // HealthSet();
    }

    void HealthSet()
    {
        healthBar.fillAmount = health / maxHealth;
    }
}
