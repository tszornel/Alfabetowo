using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory ", menuName = "Inventory System/Wallet")]
public class WalletObject : ScriptableObject
{
    //  public ItemName coin = ItemName.Moneta;
    [SerializeField]
    private int amount;
   // public event EventHandler OnWalletChanged;
    public string savePath;



    public void IncreaseAmount(int _amount) {

        amount += _amount;


    }

    public void DecreaseAmount(int _amount)
    {

        amount -= _amount;


    }

    public void ClearWallet()
    {

        amount = 0;


    }

    public int GetAmount()
    {

        return amount;


    }
    /* [ContextMenu("Save")]
     public void Save()
     {
         string saveData = JsonUtility.ToJson(this, true);
         BinaryFormatter bf = new BinaryFormatter();
         FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
         GameLog.LogMessage("Saved to " + Application.persistentDataPath + savePath);
         bf.Serialize(file, saveData);
     }
     [ContextMenu("Load")]
     public void Load()
     {
         //GameLog.LogMessage("Execute Load Inventory");
         if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
         {
             BinaryFormatter bf = new BinaryFormatter();
             FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
             JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
         }
         OnWalletChanged?.Invoke(this, EventArgs.Empty);
     }
     [ContextMenu("Clear")]
     public void Clear()
     {
         // GameLog.LogMessage("Execute Clear Inventory");
        this.amount = 0; 
         OnWalletChanged?.Invoke(this, EventArgs.Empty);
     }*/
}




