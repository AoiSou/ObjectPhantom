using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int maxHealth;
    public int health;

    public GameObject canvas;
    public Slider HPSlider;

    void Start()
    {
        HPSlider = canvas.transform.Find("HPBar").GetComponent<Slider>();
        HPSlider.value = 1f;
    }
    private void Update()
    {
        canvas.transform.rotation = Camera.main.transform.rotation;
        
        UpdateHPValue();
    }
    
    public int GetHp() {
        return health;
    }
    
    public void UpdateHPValue() {
        HPSlider.value = (float)GetHp() / (float)GetMaxHp();
    }
    
    public int GetMaxHp() {
        return maxHealth;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Destroy(gameObject);
            GameManager.successcount++;
        }
    }
}
