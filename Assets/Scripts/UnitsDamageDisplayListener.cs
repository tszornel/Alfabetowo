using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsDamageDisplayListener : MonoBehaviour
{

    [Header("Damage Data Sources")]
    public UnitDamageDisplayBehaviour[] unitDamageDisplayBehaviours;

    [Header("Display Managers")]
    public NumberDisplayManager numberDisplayManager;
    public HitVFXDisplayManager hitVFXDisplayManager;


   

    public void RegisterToDisplay() {

        UnitDamageDisplayBehaviour.StaticDamageDisplayEvent += ShowDamageDisplays;
    }

    public void DeregisterToDisplay()
    {

        UnitDamageDisplayBehaviour.StaticDamageDisplayEvent -= ShowDamageDisplays;
    }

    void OnEnable()
    {
        RegisterToDisplay();
       /* for (int i = 0; i < unitDamageDisplayBehaviours.Length; i++)
        {
            unitDamageDisplayBehaviours[i].DamageDisplayEvent += ShowDamageDisplays;
        }*/
    }

    void OnDisable()
    {
        DeregisterToDisplay();
       /* for (int i = 0; i < unitDamageDisplayBehaviours.Length; i++)
        {
            unitDamageDisplayBehaviours[i].DamageDisplayEvent -= ShowDamageDisplays;
        }*/
    }

    void ShowDamageDisplays(int damageAmount, Transform damageLocation, Color damageColor)
    {
      //  GameLog.LogMessage("Show damage Display");
        numberDisplayManager.ShowNumber(damageAmount, damageLocation, damageColor);
        hitVFXDisplayManager.ShowHitVFX(damageLocation);
    }

}
