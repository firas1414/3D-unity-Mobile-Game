using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public delegate void OnHealthChange(float current_Health, float delta, float maxHealth);
    public delegate void OnTakeDamage(float current_Health, float delta, float maxHealth);
    public delegate void OnDied();

    [SerializeField] float current_Health = 100; 
    [SerializeField] float maxHealth = 100; 
    public event OnHealthChange onHealthChange;
    public event OnTakeDamage onTakeDamage;
    public event OnDied onDied;
    
    public void changeHealth(float amt)
    {
        if (amt == 0 || current_Health == 0)
        {
            return;
        }

        current_Health += amt;

        if (amt < 0)
        {
            onTakeDamage?.Invoke(current_Health, amt, maxHealth);
        }
        onHealthChange?.Invoke(current_Health, amt, maxHealth);

        if (current_Health <= 0)
        {
            current_Health = 0;
            onDied?.Invoke();
        }

        //Debug.Log($"{gameObject.name} , taking damage : {amt} , current_Health : {current_Health}");
    }
}
