using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float Health, MaxHealth, Width, Height;
    
    [SerializeField] private RectTransform HealthBar;

    public void SetMaxHealth (float maxHealth) {
        MaxHealth = maxHealth;
    }

    public void SetHealth (float health) {
        Health = health;
        float NewWidth = (Health / MaxHealth) * Width;
        HealthBar.sizeDelta = new Vector2(NewWidth, Height);
    }
}
