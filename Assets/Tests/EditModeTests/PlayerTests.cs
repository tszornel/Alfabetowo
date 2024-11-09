
using NUnit.Framework;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.U2D.Animation;




[TestFixture]
 public class PlayerTests
{
    private GameObject gameObject;
    private Player player;


    private GameObject playerGameObject;
    private Player playerScript;

//    [SetUp]
//    public void SetUp()
//    {
       
//        playerGameObject = new GameObject("Player");
//        playerScript = playerGameObject.AddComponent<Player>();

        
//        playerScript.playerData = ScriptableObject.CreateInstance<PlayerData>();
        
//        playerScript.playerData.categoryCorps = "CategoryCorpse";
//        playerScript.playerData.labelNormal = "LabelNormal";
        
//    }




//    //[Test]
//    //public void EquipArmorNone_ResetsToDefaultState()
//    //{
       
//    //    playerScript.EquipArmorNone();

        
//    //    Assert.AreEqual("LabelNormal", playerScript.corpseResolver.GetLabel(), "Corpse resolver did not reset to expected label.");

      
//    //}

//    [TearDown]
//    public void TearDown()
//    {
       
//        if (playerGameObject != null)
//        {
//            Object.DestroyImmediate(playerGameObject);
//        }
//    }
}
