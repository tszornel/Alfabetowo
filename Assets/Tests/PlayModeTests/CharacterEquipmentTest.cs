using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CharacterEquipmentTest
{
    // A Test behaves as an ordinary method
    /*[Test]
    public void GetWeaponItemWorkedTest()
    {
        // Use the Assert class to test conditions
        GameObject gameObject = new GameObject("CharacterEquipment");
        var characterEquipmentObj = gameObject.AddComponent<CharacterEquipment>();
        characterEquipmentObj.playerData.weaponItem = new Item(1, "sword", ItemObjectType.Equipment);
        string expectedOutput = "ID=1,Name=sword,Type=Equipment";
        yield return null;
        Assert.AreEqual(expectedOutput, characterEquipmentObj.GetWeaponItem());
    }
    */
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator NewTestScriptWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        /*GameObject gameObject = new GameObject("CharacterEquipment");
        var characterEquipmentObj = gameObject.AddComponent<CharacterEquipment>();
        characterEquipmentObj.playerData.weaponItem = new Item(1, "sword", ItemObjectType.Equipment);
        string expectedOutput = "ID=1,Name=sword,Type=Equipment";
        yield return null;
        Assert.AreEqual(expectedOutput, characterEquipmentObj.GetWeaponItem());*/
        yield return null;
    }
}
