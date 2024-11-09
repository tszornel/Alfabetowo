using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Webaby.Utils;
public class UI_InventoryCharacterEquipment : MonoBehaviour
{
    private Transform heroSlotTransform;
    [SerializeField]private UI_CharacterEquipmentSlot heroSlot;
    private CharacterEquipment characterEquipment;
    public Transform player;
    private AudioSource playerAudioSource;
    [SerializeField]private AudioClip cantDoItAC;
    private void Start()
    {
        if(heroSlot)
            heroSlot.OnItemDropped += HeroSlot_OnItemDropped;
        if (player)
        {
            characterEquipment = player.GetComponent<CharacterEquipment>();
            playerAudioSource = player.GetComponent<AudioSource>();
        }
     }
    void OnDestroy()
    {
        if (heroSlot)
            heroSlot.OnItemDropped -= HeroSlot_OnItemDropped;
    }



    public void TryToEqip() { 
    
    
        
    
    }
    
    
    private void HeroSlot_OnItemDropped(object sender, UI_CharacterEquipmentSlot.OnItemDroppedEventArgs e)
    {

        GameLog.LogMessage("HeroSlot_OnItemDroppe TrytoEquip put OnSlot:" + e.item + " equipslot" + e.item.GetEquipSlot()+ "location"+e.item.Location);

        
        // player.GetComponent<PlayerPlatformerController>().TryToBuyItem(e.item);

        //if (e.item.Location == ItemLocation.Inventory || e.item.Location == ItemLocation.Equipment)
        //{

            GameLog.LogMessage("TrytoEquip put OnSlot:"+e.item+" equipslot"+ e.item.GetEquipSlot());

            if (e.item.GetEquipSlot() == CharacterEquipment.EquipSlot.Armour && characterEquipment.GetArmorItem() == null)
            {

                if (characterEquipment.GetArmorItem() != null) { 
                   // player.GetComponent<PlayerPlatformerController>().GetInventory().AddItem(characterEquipment.GetArmorItem());
                    //characterEquipment.removeArmourItem();
                }

                characterEquipment.TryEquipItem(e.item.GetEquipSlot(), e.item);
            }
            else if (e.item.GetEquipSlot() == CharacterEquipment.EquipSlot.Weapon && characterEquipment.GetWeaponItem() == null)
            {
              //  player.GetComponent<PlayerPlatformerController>().GetInventory().AddItem(characterEquipment.GetWeaponItem());
              //  characterEquipment.removeWeaponItem();
                characterEquipment.TryEquipItem(e.item.GetEquipSlot(), e.item);
            }
            else if (e.item.GetEquipSlot() == CharacterEquipment.EquipSlot.Helmet && characterEquipment.GetHelmetItem()==null)
            {
           // characterEquipment.removeHelmetItem();
            characterEquipment.TryEquipItem(e.item.GetEquipSlot(), e.item);
            }
            else
            {
                //Odebraj nie moge tego 
                if (cantDoItAC)
                    playerAudioSource.PlayOneShot(cantDoItAC);
            }
       // }
    }
}
