using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Webaby.Utils;
public enum ItemObjectType
{
    Coin,
    Letter,
    Equipment,
    Repaire,
    Default,
    Health,
    Potion,
    Key,
    Feather,
    Map,
    None
}
[Serializable]
public enum AttributesName
{
    Protection, //Ochrona zycia zmniejsza sie po trafieniu przez mola 
    Health, //Zycie 
    Intelect, //Intelect points do zbierania. kazde zadanie podnosi jeden intelekt point W danym levelu trzeba uzbierac okreslona ilosc
    Power, //Attacking more
    Agility, //Jumping upper
    Speed //Running faster
}
public abstract class ItemObject : ScriptableObject
{
    public int ID;
    public ItemObjectType type;
    public string displayName;
    public string objectName;
    public Sprite itemSprite;
    public Sprite itemIcon;
    public Sprite itemSpriteDamaged;
    public int amount;
    [TextArea(16,20)]
    public string description;
    public int price;
    public GameObject itemPrefab;
    public ItemBuff[] buffs;
    public bool stackable;
    public bool damaged;
    public NameObject nameObject;

    /* public ItemObject(int _id, ItemObjectType _type, Sprite _itemSprite,int  _amount, int _price, ItemBuff[] _buffs) {
         this.ID = _id;
         this.type = _type;
         itemSprite = _itemSprite;
         amount = _amount;
         price = _price;
         if (_buffs != null) 
         {
             buffs = _buffs;
         }
     }*/

    public Item createItem() {
        Item item = new Item(this);
        return item;
    }
}
[System.Serializable]
public class Item {
    public int Id;
#if UNITY_EDITOR
    [StringInList(typeof(PropertyDrawerHelper), "AllItemsAndLetters")]
#endif
    public string Name;
    public string DisplayedName;
    public bool stackable;
    //public bool active;
    public ItemObjectType Type;
    public ItemBuff[] buffs;
    public ItemLocation Location;
    [SerializeField]
    public IItemHolderNew itemHolder;
    private ItemObject itemObject;
    public bool damaged;
    private int slotId;

    public int GetSlotId() {
        return slotId;
    
    
    }

    public void  SetSlotId(int _slotId)
    {
        this.slotId = _slotId;


    }
    public Item(int _id, string _name, ItemObjectType _type) {
        Id = _id;
        Name = _name;
        Type = _type;
        
    }
    public Item(Item _item)
    {
        Id = _item.Id;
        Name = _item.Name;
        DisplayedName = _item.DisplayedName;
        Type = _item.Type;
        Location = _item.Location;
        stackable = _item.stackable;  
        damaged = _item.damaged;   
        itemHolder = _item.itemHolder;      
        if (_item.buffs != null) { 
            buffs = new ItemBuff[_item.buffs.Length];
            for (int i = 0; i < buffs.Length; i++)
            {
                buffs[i] = new ItemBuff(_item.buffs[i].min, _item.buffs[i].max)
                {
                    attribute = _item.buffs[i].attribute
                };
            }
        }
    }
   public Item(ItemObject itemObject) {
        Id = itemObject.ID;
        Name = itemObject.name;
        Type = itemObject.type;
        DisplayedName = itemObject.displayName;
        stackable = itemObject.stackable;
        damaged= itemObject.damaged;        
       // this.itemObject = itemObject;
        if (itemObject.buffs != null) { 
            buffs = new ItemBuff[itemObject.buffs.Length];
        for (int i = 0; i < buffs.Length; i++)
        {
            buffs[i] = new ItemBuff(itemObject.buffs[i].min, itemObject.buffs[i].max) {
                attribute = itemObject.buffs[i].attribute
            };
        }
        }
    }

    public Item(string itemName)
    {

        Item _item = PickupItemAssets.Instance.GetAnyItem(itemName);

        Id = _item.Id;
        Name = _item.Name;
        Type = _item.Type;
        Location = _item.Location;
        stackable= _item.stackable;
        damaged = _item.damaged;
        if (_item.buffs != null)
        {
            buffs = new ItemBuff[_item.buffs.Length];
            for (int i = 0; i < buffs.Length; i++)
            {
                buffs[i] = new ItemBuff(_item.buffs[i].min, _item.buffs[i].max)
                {
                    attribute = _item.buffs[i].attribute
                };
            }
        }
    }



    internal void SetLocation(ItemLocation _location)
    {
        Location = _location;
    }
    public ItemObjectType GetItemType() {
        return Type;
    }

    public ItemObject GetItemObject()
    {   
        if (!itemObject)
        { 
            if (Type == ItemObjectType.Letter)
                itemObject = PickupItemAssets.Instance.lettersDatabase.GetItemObjectFromName(Name);
            else
                itemObject = PickupItemAssets.Instance.itemsDatabase.GetItemObjectFromName(Name);
        }
        return itemObject;
    }

    public LetterObject GetLetterObject()
    {
        if (!itemObject)
            itemObject = PickupItemAssets.Instance.lettersDatabase.GetItem[Id];
        return (LetterObject)itemObject;
    }


    public CharacterEquipment.EquipSlot GetEquipSlot()
    {
        if (this.damaged)
        {
            GameLog.LogMessage("Obiekt do reperacji !");
            return CharacterEquipment.EquipSlot.Repair;
        }
          
        switch (Type)
        {
            case ItemObjectType.Equipment:
                EquipmentType equipmentType;
                if (!itemObject)
                {
                    itemObject = this.GetItemObject();
                }
                equipmentType = ((EquipmentObject)itemObject).WeaponType;
                return equipmentType.ToString().ToEnum<CharacterEquipment.EquipSlot>();
           
            case ItemObjectType.Letter:
                return CharacterEquipment.EquipSlot.Letter;
            case ItemObjectType.Default:
                return CharacterEquipment.EquipSlot.Default;        
            default:
                return CharacterEquipment.EquipSlot.None;
        }

       
    }
    public override string ToString()
    {
        /* string buffsString="";
         for (int i = 0; i < buffs.Length; i++)
         {
             buffsString += buffs[i].ToString() +"\n";
         }*/
        return "ID=" + Id + ",Name=" + Name + ",Type=" + Type+" damaged="+damaged; //+ buffsString;
    }


    public void SetItemHolder(IItemHolderNew _itemHolder)
    {
        itemHolder = _itemHolder;
    }
    public IItemHolderNew GetItemHolder()
    {
        if (itemHolder != null)
            GameLog.LogMessage("Get Item Holder " + this.itemHolder);
        else
            GameLog.LogMessage("Item holder null for item" + this); 
        return itemHolder;
    }

    public override bool Equals(object obj)
    {
        return obj is Item item &&
               Id == item.Id &&
               Name == item.Name &&
               stackable == item.stackable &&
               Type == item.Type &&
               EqualityComparer<ItemBuff[]>.Default.Equals(buffs, item.buffs) &&
               damaged == item.damaged;
    }

    public override int GetHashCode()
    {
        int hashCode = -1267396702;
        hashCode = hashCode * -1521134295 + Id.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
        hashCode = hashCode * -1521134295 + damaged.GetHashCode();
        return hashCode;
    }
}
[System.Serializable]
public class ItemBuff  : IModifiers{
    public AttributesName attribute;
    public int value;
    public int min;
    public int max;
    public ItemBuff(int _min,int _max) {
        min = _min;
        max = _max;
        GenerateValue();
    }
    public ItemBuff(AttributesName _attribute,int _min, int _max)
    {
        attribute = _attribute;
        min = _min;
        max = _max;
        GenerateValue();
    }

   


    public void AddValue(ref int baseValue)
    {
        baseValue += value;
    }
    public int GenerateValue() {
        value = UnityEngine.Random.Range(min, max);
        return value;
    }
    /*public override string ToString()
    {
        return "min=" + min + ",max=" + max + ",value=" + value;
    }*/

    // override object.Equals
    public override bool Equals(object obj)
    {
        GameLog.LogMessage("check equals !@!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!:"+ attribute+" VSLUE"+value);
        GameLog.LogMessage("check equals obj attribute" + ((ItemBuff)obj).attribute + " value" + ((ItemBuff)obj).value);
        if (obj == null || attribute != ((ItemBuff)obj).attribute || value != ((ItemBuff)obj).value )       {
            GameLog.LogMessage("check equals !@!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! false");
            return false;
        }

        GameLog.LogMessage("check equals !@!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! true");
        return true;
    }

    public override int GetHashCode()
    {
        int hashCode = -1060830807;
        hashCode = hashCode * -1521134295 + attribute.GetHashCode();
        hashCode = hashCode * -1521134295 + value.GetHashCode();
        hashCode = hashCode * -1521134295 + min.GetHashCode();
        hashCode = hashCode * -1521134295 + max.GetHashCode();
        return hashCode;
    }
}
