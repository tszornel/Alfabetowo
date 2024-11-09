using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;
using static AttachmentSetBehaviour;

public class Player : MonoBehaviour
{

    public PlayerData playerData;      
    private SpriteResolver rightHandResolver;
    private SpriteResolver leftHandResolver;
    private SpriteResolver rightLegResolver;
    private SpriteResolver leftLegResolver;
    private SpriteResolver corpseResolver;
    private SpriteResolver headResolver;
    private SpriteResolver hatResolver;
    private SpriteRenderer rightLegRenderer;
    private SpriteRenderer leftLegRenderer;
    private SpriteRenderer hatRenderer;
    private SpriteRenderer corpseRenderer;
    public Transform WeaponManager;
   // public Transform leftHandWeapon;
    public Transform rightHandSpriteTransform;
    public Transform leftHandSpriteTransform;
    public Transform rightHand;
    public Transform leftHand;
    public Transform rightLeg;
    public Transform leftLeg;
    public Transform corpse;
    public Transform head;
    public Transform hat;
    private PlayerPlatformerController playerController;
    private CharacterEquipment characterEquipment;
    AttachmentSetBehaviour attachementSet;
    List<Item> equipmentList = new List<Item>();


    //public PlayerAttribute[] playerAttributes;
    public static Action<PlayerAttribute[]> PlayerAttributesChangeAction;
    public event EventHandler OnEquipChanged;
    private ItemObjectDatabase itemObjectDB;
    //  public event EventHandler OnGoldAmountChanged;
    //  public event EventHandler OnHealthPotionAmountChanged;
    public PlayerAttribute[] GetPlayerAttributes() {
        return playerData.playerAttributes;
    }
    
    public static Player Instance { get; private set; }
    private int check = 0;
    private void Awake()
    {
        attachementSet = GetComponent<AttachmentSetBehaviour>();
        characterEquipment = GetComponent<CharacterEquipment>();
        playerController = transform?.GetComponent<PlayerPlatformerController>();
        rightHandResolver = rightHand?.GetComponent<SpriteResolver>();
        leftHandResolver = leftHand?.GetComponent<SpriteResolver>();
        rightLegResolver = rightLeg?.GetComponent<SpriteResolver>();
        leftLegResolver = leftLeg?.GetComponent<SpriteResolver>();
        corpseResolver = corpse?.GetComponent<SpriteResolver>();
        corpseRenderer = corpse?.GetComponent<SpriteRenderer>();
        rightLegRenderer = rightLeg?.GetComponent<SpriteRenderer>();
        leftLegRenderer= leftLeg?.GetComponent<SpriteRenderer>();
        headResolver = head?.GetComponent<SpriteResolver>();
        hatResolver = hat?.GetComponent<SpriteResolver>();
        hatRenderer = hat?.GetComponent<SpriteRenderer>();
        itemObjectDB = Resources.Load<ItemObjectDatabase>("ItemsDatabase");
        DisableSuperJump();
    }



    public void switchHands(bool turnRight) {

       /* if (turnRight)
        {
            WeaponManager.SetParent(rightHandSpriteTransform,false);
            WeaponManager.SetAsFirstSibling();

        }
        else
        {
            WeaponManager.SetParent(leftHandSpriteTransform, false);
            WeaponManager.SetAsFirstSibling();
        }*/



     AttachmentSetBehaviour.switchHands(turnRight);   



    }
    public void PlayerDrawning(int corpseSortingOrder, int leftLegSortingOrder, int rightLegSortingOrder, string sortingLayerName) {

        GameLog.LogMessage("Player Drawning change corpse");
        corpseRenderer.sortingLayerName = sortingLayerName;
        corpseRenderer.sortingOrder = corpseSortingOrder;
        rightLegRenderer.sortingLayerName = sortingLayerName;
        rightLegRenderer.sortingOrder = rightLegSortingOrder;
        leftLegRenderer.sortingLayerName = sortingLayerName;
        leftLegRenderer.sortingOrder = leftLegSortingOrder;
        //corpseRenderer.sortingOrder = 0;


    }
    public void SetFireFlyFollow() 
    {

        playerData.FireflyFollows = true;
    
    }

    public void UnSetFireFlyFollow()
    {

        playerData.FireflyFollows = false;

    }

    public bool IsLighterFollowing()
    {

        return playerData.FireflyFollows;

    }

    private void OnApplicationQuit()
    {
        ResetPlayer();
    }

    public void DisableSuperJump()
    {
        leftLegRenderer.material = playerData.standardMaterial;
        rightLegRenderer.material = playerData.standardMaterial;
        playerData.jumpTakeOffSpeed = 10f;
        playerData.gravityModifier = 3f;
    }

    public void EnableSuperJump() {

        leftLegRenderer.material = playerData.glowMaterial;
        rightLegRenderer.material = playerData.glowMaterial;
        playerData.jumpTakeOffSpeed = 16f;
        playerData.gravityModifier = 2f;

    }

    private void Start()
    {
        
        if (playerData.playerAttributes != null)
        {
            for (int i = 0; i < playerData.playerAttributes.Length; i++)
            {
                if (playerData.playerAttributes[i].parent=null)
                    playerData.playerAttributes[i].SetParent(this);
            }
        }
        if (PlayerAttributesChangeAction != null)
        {

            PlayerAttributesChangeAction(playerData.playerAttributes);
        }
    }
    public void SetCharacterEquipment(CharacterEquipment characterEquipment)
    {
      //  GameLog.LogMessage("Set character equipment");
        this.characterEquipment = characterEquipment;
        //  UpdateVisual();
        characterEquipment.OnEquipmentAdded += OnEquipmentAddedAction;
        characterEquipment.OnEquipmentRemoved += OnEquipmentRemovedAction;
    }
    public void UnSetCharacterEquipment()
    {
      if (characterEquipment) { 
        characterEquipment.OnEquipmentAdded -= OnEquipmentAddedAction;
        characterEquipment.OnEquipmentRemoved -= OnEquipmentRemovedAction;
        }
    }
    void OnEquipmentRemovedAction(Item removedItem) {

        if (removedItem == null) { 
        
            return;
        }
        GameLog.LogMessage(string.Concat("Removed item", removedItem?.ToString()));
        
        if (playerData.playerAttributes != null) { 
        if (removedItem.buffs!=null)
            for (int i = 0; i < removedItem.buffs.Length; i++)
            {
                for (int j = 0; j < playerData.playerAttributes.Length; j++)
                {
                    if (playerData.playerAttributes[j].type == removedItem.buffs[i].attribute) {
                        GameLog.LogMessage("Removed item buffs " + removedItem.buffs[i].attribute + "value"+ removedItem.buffs[i].value);
                            playerData.playerAttributes[j].value.RemoveModifier(removedItem.buffs[i]);
                    }
                }
            }
            if (PlayerAttributesChangeAction != null) 
            { 

                PlayerAttributesChangeAction(playerData.playerAttributes);
            }
    }
    }
    void OnEquipmentAddedAction(Item addedItem)
    {
        SteeringPanel.Instance?.ActivateAbilityFVX();
        GameLog.LogMessage("On equipment added action entered");
        if (playerData.playerAttributes != null) { 
            GameLog.LogMessage(string.Concat("Added item", addedItem.ToString()));
        if (addedItem.buffs != null)
            for (int i = 0; i < addedItem.buffs.Length; i++)
            {
                for (int j = 0; j < playerData.playerAttributes.Length; j++)
                {
                    if (addedItem.buffs[i].attribute == playerData.playerAttributes[j].type)
                    {
                        GameLog.LogMessage("Add modifier" + addedItem.buffs[i].attribute + " "+ addedItem.buffs[i].value);
                            playerData.playerAttributes[j].value.AddModifier(addedItem.buffs[i]);
                    }
                }
            }
        if (PlayerAttributesChangeAction != null)
            PlayerAttributesChangeAction(playerData.playerAttributes);
        }

    }
    public void OnEquipmentUpdatePlayerStats(PlayerEquipmentData equipmentData)
    {
        GameLog.LogMessage("On Equipment player Stats");

        
        if (playerData.playerAttributes != null)
        {
            for (int i = 0; i < playerData.playerAttributes.Length; i++)
            {
                if (playerData.playerAttributes[i].parent = null)
                    playerData.playerAttributes[i].SetParent(this);
                GameLog.LogMessage("Attribute " + playerData.playerAttributes[i].type+ "Base value  " + playerData.playerAttributes[i].value.ModifiedValue+" i="+i);
            }
        }
        GameLog.LogMessage("On Equipment Update Player Stats"+check);
        check += 1;
        equipmentList.Clear();
        if (equipmentData.helmetItem!=null)
            equipmentList.Add(equipmentData.helmetItem);
        if (equipmentData.armorItem != null)
            equipmentList.Add(equipmentData.armorItem);
        if (equipmentData.weaponItem != null) {
            
            equipmentList.Add(equipmentData.weaponItem);
        }
            
        GameLog.LogMessage(" check size equpment list" + equipmentList);
        for (int k = 0; k < equipmentList.Count; k++)
        {
     
            if (k > 2)
                return;
         
            if (equipmentList[k].buffs != null)
                for (int i = 0; i < equipmentList[k].buffs.Length; i++)
                {
                    for (int j = 0; j < playerData.playerAttributes.Length; j++)
                    {
                        if (equipmentList[k].buffs[i].attribute == playerData.playerAttributes[j].type)
                        {
                            GameLog.LogMessage("Attribute name"+ equipmentList[k].buffs[i].attribute+ "Add value" + equipmentList[k].buffs[i].value);
                            playerData.playerAttributes[j].value.AddModifier(equipmentList[k].buffs[i]);
                            GameLog.LogMessage("Base value" + playerData.playerAttributes[j].value.BaseValue);
                            GameLog.LogMessage("New value !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!"+ playerData.playerAttributes[j].type + playerData.playerAttributes[j].value.ModifiedValue);
                        }
                    }
                }
        }
    }
    public void SetEquipment(int itemId)
    {
        if (!rightHandResolver || !leftHandResolver || !rightLegResolver || !leftLegResolver ||!corpseResolver||!headResolver || !hatResolver || !hatRenderer)
        {
            rightHandResolver = rightHandSpriteTransform.GetComponent<SpriteResolver>();
            leftHandResolver = leftHandSpriteTransform.GetComponent<SpriteResolver>();
            rightLegResolver = rightLeg.GetComponent<SpriteResolver>();
            leftLegResolver = leftLeg.GetComponent<SpriteResolver>();
            corpseResolver = corpse.GetComponent<SpriteResolver>();
            headResolver = head.GetComponent<SpriteResolver>();
            hatResolver = hat.GetComponent<SpriteResolver>();
            hatRenderer = hat.GetComponent<SpriteRenderer>();
        }
        if (!playerController) {
            playerController = transform.GetComponent<PlayerPlatformerController>();
        }
        // Equip Item
        GameLog.LogMessage("Before switch item id=" + itemId);
        CheckEquipment(itemId);
        OnEquipChanged?.Invoke(this, EventArgs.Empty);
    }

    private void CheckEquipment(int itemId)
    {
        GameLog.LogMessage("CheckEquipment entered" + itemId);
        EquipmentObject equipment;
        ItemObject itemObject = itemObjectDB?.GetItemObjectFromID(itemId);
       
        if (itemObject && itemObject.type == ItemObjectType.Equipment)
        {
            equipment = ((EquipmentObject)itemObject);

            if (equipment.WeaponType == EquipmentType.Weapon)
                EquipWeapon(itemObject);
            else if(equipment.WeaponType == EquipmentType.Tool)
                EquipWeapon(itemObject);
            else if (equipment.WeaponType == EquipmentType.Armour)
                EquipArmor(itemId);
            else if (equipment.WeaponType == EquipmentType.Helmet)
                EquipHelmet(itemId);

        }
    }
    private void EquipArmourNone()
    {
       // GameLog.LogMessage("Equip Armour !!!!!!!!!!!!!!!!!!!!!!!!!" + categoryCorps + "  " + labelArmour);
        SetClotherWithID(corpseResolver, playerData.categoryCorps, playerData.labelNormal);
        SetClotherWithID(leftLegResolver, playerData.categoryLeftLeg, playerData.labelLeftLeg_normal);
        SetClotherWithID(rightLegResolver, playerData.categoryRightLeg, playerData.labelRightLeg_normal);
        SetClotherWithID(rightHandResolver, playerData.categoryRightHand, playerData.labelRightHand_normal);
        SetClotherWithID(leftHandResolver, playerData.categoryLeftHand, playerData.labelLeftHand_normal);
        OnEquipChanged?.Invoke(this, EventArgs.Empty);

        // GameLog.LogMessage("Sprite resolver label" + corpseResolver.ToString());
    }
    private void EquipHelmet(int id)
    {
        SetClotherWithID(hatResolver, playerData.categoryHats, playerData.labelHelm);
        hatRenderer.sortingOrder = 8;
    }
 
    private void EquipArmor(int id)
    {
        GameLog.LogMessage("Equip Armour !!!!!!!!!!!!!!!!!!!!!!!!!"+ id);
        SetClotherWithID(corpseResolver, playerData.categoryCorps, playerData.labelArmour);
        SetClotherWithID(leftLegResolver, playerData.categoryLeftLeg, playerData.labelLeftLeg_armoured);
        SetClotherWithID(rightLegResolver, playerData.categoryRightLeg, playerData.labelRightLeg_armoured);
        SetClotherWithID(rightHandResolver, playerData.categoryRightHand, playerData.labelRightHand_armoured);
        SetClotherWithID(leftHandResolver, playerData.categoryLeftHand, playerData.labelLeftHand_armoured);

      //  GameLog.LogMessage("Sprite resolver label" + corpseResolver.ToString());
    }
    public void EquipArmorNone()
    {
        SetClotherWithID(corpseResolver, playerData.categoryCorps, playerData.labelNormal);
        SetClotherWithID(rightHandResolver, playerData.categoryRightHand, playerData.labelRightHand_normal);
        SetClotherWithID(leftHandResolver, playerData.categoryLeftHand, playerData.labelLeftHand_normal);
        SetClotherWithID(rightLegResolver, playerData.categoryRightLeg, playerData.labelRightLeg_normal);
        SetClotherWithID(leftLegResolver, playerData.categoryLeftLeg, playerData.labelLeftLeg_normal);
        OnEquipChanged?.Invoke(this, EventArgs.Empty);
    }
    public void EquipWeapon_Punch()
    {
        GameLog.LogMessage("Ustaw tylko piesci");
        attachementSet.removeWeapon();
        OnEquipChanged?.Invoke(this, EventArgs.Empty);


    }
    public void EquipHelmetNone()
    {
        SetClotherWithID(hatResolver, playerData.categoryHats, playerData.labelHat);
        hatRenderer.sortingOrder = 3;
        OnEquipChanged?.Invoke(this, EventArgs.Empty);
    }
    private void EquipWeapon(ItemObject io)
    {
        GameLog.LogMessage("Equip Weapon");
        string labelName;
        
        if (io)
            labelName = io.name;
        else
            labelName = playerData.labelEmptySword;
      

        bool faceRight = playerController.faceRightCheck();
        if(!attachementSet) 
            attachementSet = GetComponent<AttachmentSetBehaviour>();
        GameLog.LogMessage("attachement set" + attachementSet.ToString()+"io"+io);
        attachementSet.setWeapon(io, faceRight);

   
    }
   
    public static void SetClotherWithID(
    SpriteResolver spriteResolver,
    string category,
    string label)
    {
        GameLog.LogMessage("Sprite resolver" + spriteResolver.ToString());
        GameLog.LogMessage("Category=" + category + " label=" + label);
        spriteResolver.SetCategoryAndLabel(category, label);
        spriteResolver.ResolveSpriteToSpriteRenderer();
    }
    public void AtrributeModifier(PlayerAttribute attribute)
    {
        GameLog.LogMessage(string.Concat("Modified attribute type:", attribute.type, " new attribute value=",attribute.value.ModifiedValue));
    }


    public void ResetPlayer()
    {

        playerData.ResetPlayerData();


    }
}
