using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "_PlayerData", menuName = "Inventory System/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Labels")]
    public string categoryLeftLeg = "leftLeg";
    public string categoryRightLeg = "rightLeg";
    public string categoryRightHand = "rightHand";
    public string categoryLeftHand = "leftHand";
    public string categoryCorps = "corpse";
    public string categoryWeapon = "Weapon";
    public string categoryHats = "hats";
    //   public  string labelSword = "sword";
    public string labelEmptySword = "None";
    public string labelArmour = "corpse_armoured";
    public string labelNormal = "corpse_with_belt";
    public string labelHat = "hat";
    public string labelHelm = "helm";
    // public  string labelKij = "kij";
    //  public  string labelBicz = "bicz";
    public string labelRightHand_armoured = "rightHand_armoured";
    public string labelLeftHand_armoured = "left_hand_armoured";
    public string labelRightHand_normal = "rightHand";
    public string labelLeftHand_normal = "left_hand_standard";
    public string labelRightLeg_normal = "right_leg_standard";
    public string labelLeftLeg_normal = "left_leg_standard";
    public string labelRightLeg_armoured = "right_leg_armoured";
    public string labelLeftLeg_armoured = "left_leg_armoured";
    [Header("Player Materials")]
    public Material glowMaterial;
    public Material standardMaterial;
    [Header("Player Atrributes")]
    public PlayerAttribute[] playerAttributes;
    [Header("Effect prefabs")]
    public GameObject pickupEffect;
    [Header("Player Settings")]
    public float minGroundNormalY = .65f;
    public float gravityModifier = 1f;
    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;
    [Header("Player Masks Settings")]
    public LayerMask whatIsEnemies;
    public LayerMask[] whatIsItem;
    [Header("Player Internal objects")]
    public GameObject trailObject;
    public bool FireflyFollows = false;
    public float superJumpTime = 40f;
    public bool playLetterSounds;
    public Vector2 checkPointPosition;
    public float letterDisplayTime = 5f;


    public int GetValueBuff(AttributesName name)
    {
        foreach (var buff in playerAttributes)
        {
            if (buff.type == name)
            {
                return buff.value.ModifiedValue;
            }
            else
            {
                continue;
            }
        }
        return 0;
    }

   

    public void ResetPlayerData() {

        FireflyFollows = false;
        if (playerAttributes != null)
        {
            for (int i = 0; i < playerAttributes.Length; i++)
            {
                playerAttributes[i].value.Reset();
            }
        }
    }
}
