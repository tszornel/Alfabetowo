using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using UnityEngine.UIElements;
using static AttachmentSetBehaviour;

public class AttachmentSetBehaviour : MonoBehaviour
{
    [SerializeField]private static AttachmentSet attachementHelmet;
    [SerializeField] private static AttachmentSet attachementArmour;
    [SerializeField] private static AttachmentSet attachementWeapon;
    public AttachmentSet[] attachments = new AttachmentSet[3];
    private static SpriteRenderer HelmetRenderer;
    private static SpriteRenderer ArmourRenderer;
    public static SpriteRenderer WeaponRenderer;


    public Transform GetWeaponTransform() {
         return attachementWeapon.attachmentItem.transform; 
    }


    [System.Serializable]
    public struct AttachmentSet
    {
        public string attachmentSetName;
        public Transform attachmentJoint;
        public Transform attachmentItem;
       

        public override string ToString()
        {
            StringBuilder attachementSetString = new StringBuilder();
            attachementSetString.Append("Set Name="+attachmentSetName);
            if (attachmentJoint)
                attachementSetString.Append(" Joint name=" + attachmentJoint.name);
            if (attachmentItem)
                attachementSetString.Append(" Item name=" + attachmentItem.name);
            return attachementSetString.ToString();
        }
    }
    private void Awake()
    {
       
    }

    private void Start()
    {
        if (!attachementHelmet.attachmentJoint)
            attachementHelmet = Array.Find(attachments, match: attachetObject => attachetObject.attachmentSetName == "Helmet");
        if (!attachementArmour.attachmentJoint)
            attachementArmour = Array.Find(attachments, match: attachetObject => attachetObject.attachmentSetName == "Armour");
        if (!attachementWeapon.attachmentJoint)
            attachementWeapon = Array.Find(attachments, match: attachetObject => attachetObject.attachmentSetName == "Weapon");
        GameLog.LogMessage("Display attachementHelmet" + attachementHelmet.ToString());
        GameLog.LogMessage("Display attachementArmour" + attachementArmour.ToString());
        GameLog.LogMessage("Display attachementWeapon" + attachementWeapon.ToString());



    }

    public void removeWeapon() {
        GameLog.LogMessage("Removing weapon from Weapon");

        if(!attachementWeapon.attachmentItem)
            attachementWeapon = Array.Find(attachments, match: attachetObject => attachetObject.attachmentSetName == "Weapon");
        if (attachementWeapon.attachmentItem)
              Destroy(attachementWeapon.attachmentItem.gameObject);


       // GameLog.LogMessage("attachement weapon" + attachementWeapon);
       // GameLog.LogMessage("attachement from table weapon" + attachments[2]);
      
    }

    public void setWeapon(ItemObject io, bool _faceRight)
    {
        GameLog.LogMessage("Attachement set beahviour setWeapon" + io);
        if (!attachementWeapon.attachmentJoint)
            attachementWeapon = Array.Find(attachments, match: attachetObject => attachetObject.attachmentSetName == "Weapon");
        

        EquipmentObject weapon;
        ItemObject weaponObject = io;
        if (weaponObject.type == ItemObjectType.Equipment)
        {
            weapon = ((EquipmentObject)weaponObject);

            if (weapon.WeaponType == EquipmentType.Weapon || (weapon.WeaponType == EquipmentType.Tool))
            {

                SetAttachement(attachementWeapon, weapon.itemPrefab, weaponObject.createItem(), _faceRight);

            }  

        }
        
    }

    

    void SetAttachement(AttachmentSet _attachmentSet,GameObject weaponPrefab, Item _item,bool _faceRight) {

        GameLog.LogMessage("SetWeapon entered item ");
        if (_item != null)
            GameLog.LogMessage("SetWEapon item" + _item);

        if (weaponPrefab)
            GameLog.LogMessage("SetWeapon weapon = " + weaponPrefab);

        _item.damaged = false;
        //destroy previous if exists
      //  Transform previousWeaponTransform;
        if (_attachmentSet.attachmentItem)
        {
            GameLog.LogMessage("Destroy previous entered");
            Destroy(_attachmentSet.attachmentItem);

           
        }
        //set new weapon 

        if (weaponPrefab) {
            GameLog.LogMessage("Set new weapon" + weaponPrefab.name + "_attachmentSet.attachmenSetName" + _attachmentSet.attachmentSetName+" joint"+ _attachmentSet.attachmentJoint);
            // _attachmentSet.attachmentItem 

        }

        else if (_item!=null)
            GameLog.LogMessage("Set new weapon" + _item.Name + "_attachmentSet.attachmentJoint=" + _attachmentSet.attachmentSetName);




        Transform go;
        // AttachmentSet atachement = Array.Find(attachments, match: attachetObject => attachetObject.attachmentSetName == "RightHand");
        if (weaponPrefab != null)
            _attachmentSet.attachmentItem = Instantiate(weaponPrefab, _attachmentSet.attachmentJoint, false).transform;
        else if (_item != null) {
            go = PickupItem.InstantiateStandardItemEquipment(_attachmentSet.attachmentJoint, _item);
            _attachmentSet.attachmentItem = go;
        }

        
      

        _attachmentSet.attachmentItem.SetParent(_attachmentSet.attachmentJoint,false);
        _attachmentSet.attachmentItem.SetAsFirstSibling();
        Collider2D collider = _attachmentSet.attachmentItem.GetComponent<Collider2D>();
        if (collider)
            collider.enabled = false;
        _attachmentSet.attachmentItem.localPosition = new Vector3(0, 0, 0);
        _attachmentSet.attachmentItem.name = _attachmentSet.attachmentItem.name.Substring(0, _attachmentSet.attachmentItem.name.IndexOf('('));
        GameLog.LogMessage("New name = " + _attachmentSet.attachmentItem.name);
        GameLog.LogMessage("Display set attachement" + _attachmentSet);
        attachementWeapon = _attachmentSet;
        attachments[0] = _attachmentSet;


        WeaponRenderer = _attachmentSet.attachmentItem.GetComponent<PickupItem>().spriteRenderer;

        GameLog.LogMessage("SetAttachement FaceRight" + _faceRight);
        switchHands(_faceRight);




    }

 
    public Transform checkWeapon(string name)
    {
        AttachmentSet attachement = Array.Find(attachments, match: attachetObject => attachetObject.attachmentSetName == name);

        return attachement.attachmentItem;
    }


    public AttachmentSet findAttachementSet(string name)
    {
        AttachmentSet attachement = Array.Find(attachments, match: attachetObject => attachetObject.attachmentSetName == name);
       
        return attachement;
    }




    public static void switchHands(bool turnRight)
    {
        GameLog.LogMessage("switch hands");

        if(!WeaponRenderer)
            WeaponRenderer = attachementWeapon.attachmentItem?.GetComponent<PickupItem>()?.spriteRenderer;

        
        if (!turnRight && WeaponRenderer) 
        { 
            GameLog.LogMessage("Przewroc w lewo ,Set attachement set sorting order =15", WeaponRenderer);
           // attachments[0] = _attachmentSet;

            WeaponRenderer.sortingOrder = 10;

        }
        else if(WeaponRenderer && turnRight)
        {
            GameLog.LogMessage("Przewroc w prawo, Set attachement set sorting order =9", WeaponRenderer);
          //  attachments[1] = _attachmentSet;
            WeaponRenderer.sortingOrder = 9;
        }
        }

        /*if (attachments[0].attachmentItem)
        {
            
            GameLog.LogMessage("switch right hand weapn=" + attachementRight.attachmentItem + "to " + attachementLeft.attachmentJoint.name);
            attachments[0].attachmentItem.SetParent(attachementLeft.attachmentJoint, false);
            attachments[0].attachmentItem.SetAsLastSibling();
            if (!WeaponRenderer)
            {

                WeaponRenderer = attachments[0].attachmentItem.GetComponent<SpriteRenderer>();
               
                if (!WeaponRenderer)
                {
                    //  WeaponRenderer = attachments[0].attachmentItem.GetComponent<PickupItem>().spriteRenderer;
                   WeaponRenderer = attachments[0].attachmentItem.GetComponentInChildren<SpriteRenderer>();
                    
                }

               
                WeaponRenderer.sortingLayerID = SortingLayer.NameToID("Player");// 4;//LayerMask.NameToLayer("Player");
            }
            if (WeaponRenderer)
                WeaponRenderer.sortingOrder = 8;
            attachementLeft.attachmentItem = attachments[0].attachmentItem;
            attachementRight.attachmentItem = null;
            attachments[1] = attachementLeft;
            attachments[0] = attachementRight;
            GameLog.LogMessage("after switch right hand=" + attachments[0]);
            GameLog.LogMessage("after switch left hand=" + attachments[1]);

        }else if (attachments[1].attachmentItem)
        {
            attachementLeft = attachments[1];
            GameLog.LogMessage("switch left hand="+ attachementLeft);
            attachments[1].attachmentItem.SetParent(attachementRight.attachmentJoint, false);
            attachments[1].attachmentItem.transform.SetAsLastSibling();

            if (!WeaponRenderer) {

                WeaponRenderer = attachments[1].attachmentItem.GetComponent<SpriteRenderer>();
                if (!WeaponRenderer) {
                    //  WeaponRenderer = attachments[1].attachmentItem.GetComponent<PickupItem>().spriteRenderer;
                  WeaponRenderer = attachments[1].attachmentItem.GetComponentInChildren<SpriteRenderer>();
                   
                        }
                WeaponRenderer.sortingLayerID = SortingLayer.NameToID("Player"); ;// LayerMask.NameToLayer("Player");
            }
            if(WeaponRenderer)
                WeaponRenderer.sortingOrder = 9;
           

            attachementRight.attachmentItem = attachments[1].attachmentItem;
            attachementLeft.attachmentItem = null;
            attachments[0] = attachementRight;
            attachments[1] = attachementLeft;
            GameLog.LogMessage("after switch right hand=" + attachments[0]);
            GameLog.LogMessage("after switch left hand=" + attachments[1]);
        }
        */


    
       

        
    
}