using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class Pickup : MonoBehaviour {
    public int ID;
    public string type;
    public string description;
    public Sprite icon;
    public bool pickedUp;
    [HideInInspector]
    public bool equipped;
    public bool playersWeapon;
    [HideInInspector]
    public GameObject letter;
    //  private Inventory inventory;
    // Use this for initialization
    void Start () {
        GameLog.LogMessage("Start pickup method creation ");
        if (this.type == "Letter")
        {
            GameLog.LogMessage("Letter creation get sprite ");
            string spriteName = "Literka_" + GetComponent<TMP_Text>().text.ToUpper();
            GameLog.LogMessage("Sprite name" + spriteName);
            //this.icon = new Sprite(spriteName);
            icon = Resources.Load(spriteName, typeof(Sprite)) as Sprite;
            /*if (icon == null) {
                this.icon = Resources.Load(spriteName) as Sprite;
                GameLog.LogMessage("Sprite name1");
            }
            if (icon == null)
            {
                this.icon = Resources.Load<Sprite>(spriteName);
                GameLog.LogMessage("Sprite name2");
            }
            if (icon == null)
            {
                GameLog.LogMessage("Sprite name3");
                Sprite[] icons = Resources.LoadAll<Sprite>(spriteName);
                for (int i=0; i < icons.Length;i++)
                {
                    GameLog.LogMessage("Jka IKONA" + icons[i].ToString());      
                      }
            }*/
            if (icon != null)
            {
                GameLog.LogMessage("Sprite:" + icon.ToString());
            }
            else {
                GameLog.LogMessage("Sprite icon is null !!!!!!!!!!" );
            }
        }
    }
    public void ItemUsage() {
        //Letter
        if (type == "Letter")
        {
        }
        //Health
        if (type == "Health")
        {
        }
        //weapon
        if (type == "Weapon") {
            equipped = true;
        }
    }
    void Update()
    {
        if (equipped) {
            //perform weapon acts here
        }
        /**GameLog.LogMessage("Pickup Update method entered");
        if (this.icon == null && this.type == "Letter") {
            GameLog.LogMessage("icon null update letter icon");
            string spriteName = "Literka_" + GetComponent<TMP_Text>().text.ToUpper();
            // icon = new Sprite();
            GameLog.LogMessage("IKONA nazwa :" + spriteName);
            this.icon = Resources.Load(spriteName) as Sprite;
            Sprite icon2 = Resources.Load<Sprite>(spriteName);
            GameLog.LogMessage("IKONA: "+icon+" IKONA2"+ icon2);
        }**/
    }
    /*private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            for (int i = 0; i < inventory.Slots.Length; i++) {
            }
            GameLog.LogMessage("Collides with letter");
            GameObject otherGO = other.gameObject;
            TMP_Text spawnedTextObjectComponent = other.GetComponent<TMP_Text>();
            string letter = spawnedTextObjectComponent.text;
            GameLog.LogMessage("Letter found" + letter);
            Instantiate(pickupEffect, transform.position, Quaternion.identity);
            Destroy(other);
        }
    }*/
}
