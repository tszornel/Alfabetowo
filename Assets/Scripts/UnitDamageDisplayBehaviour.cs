using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDamageDisplayBehaviour : MonoBehaviour
{

    [Header("Damage Color Tint")]
    public Color damageColorTint;

    [Header("Damage Location")]
    public Transform damageDisplayTransform;

    public delegate void DamageDisplayEventHandler(int newDamageAmount, Transform displayLocation, Color damageColor);
    //public event DamageDisplayEventHandler DamageDisplayEvent;

    public static event DamageDisplayEventHandler StaticDamageDisplayEvent;

    public void DisplayDamage(int damageTaken)
    {


       // GameLog.LogMessage("Display Damage" + transform.name, transform);
        if (StaticDamageDisplayEvent != null)
        {
            //GameLog.LogMessage("Display Event !null" + transform.name, transform);
            //DamageDisplayEvent(damageTaken, damageDisplayTransform, damageColorTint);
            if(!damageDisplayTransform)
                damageDisplayTransform = transform; 
            StaticDamageDisplayEvent(damageTaken, damageDisplayTransform, damageColorTint);
        }
    }

}
