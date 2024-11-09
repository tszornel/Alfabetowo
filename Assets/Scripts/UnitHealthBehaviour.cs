using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Webaby.Utils;

public class UnitHealthBehaviour : MonoBehaviour
{

    [Header("Health Info")]
    [SerializeField]
    [ReadOnly] private int currentHealth;
    [ReadOnly] private int maxHealth;

    [Header("Events")]
    public UnityEvent<int> healthDifferenceEvent;
    public UnityEvent healthIsZeroEvent;

    //Delegate for external systems to detect (IE: Unit's UI)
    public delegate void HealthChangedEventHandler(int newHealthAmount);
    public event HealthChangedEventHandler HealthChangedEvent;


    public void SetupCurrentHealth(int totalHealth)
    {
        currentHealth = totalHealth;
       
    }

    public void SetupMaxHealth(int _maxHealth)
    {
        maxHealth = _maxHealth;     
    }

    public void ChangeHealth(int healthDifference)
    {
        GameLog.LogMessage("ChangeHealth" + healthDifference);
        currentHealth = currentHealth - healthDifference;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            HealthIsZeroEvent();
        }

        healthDifferenceEvent.Invoke(healthDifference);
        DelegateEventHealthChanged();
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    void HealthIsZeroEvent()
    {
        healthIsZeroEvent.Invoke();
    }

    void DelegateEventHealthChanged()
    {
        if (HealthChangedEvent != null)
        {
            HealthChangedEvent(currentHealth);
        }
    }

    public int GetMaxValue()
    {
        return maxHealth;   
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override bool Equals(object other)
    {
        return base.Equals(other);
    }

    public override string ToString()
    {
        return "currentHealth="+ currentHealth+" maxHealth="+maxHealth;
    }
}
