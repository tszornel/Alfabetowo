using System.Collections.Generic;
using UnityEngine;


public delegate void ModifiedEvent();
[System.Serializable]
public class ModifiableInt
{
    [UnityEngine.SerializeField]
    private int baseValue;
    public int BaseValue { get { return baseValue; } private set { GameLog.LogMessage("Base value"+ baseValue); baseValue=value;UpdateModifiedValue(); GameLog.LogMessage("Base value next" + baseValue); } }
    [UnityEngine.SerializeField]
    private int modifiedValue;
    public int ModifiedValue { get { return modifiedValue; } private set { modifiedValue = value; } }
    protected List<IModifiers> modifiers = new List<IModifiers>();
    public event ModifiedEvent ValueModified;
    public ModifiableInt(ModifiedEvent method = null)
    {
        GameLog.LogMessage("Create new modifiable INT Base value=" + BaseValue); 
        
        modifiedValue = BaseValue;
        
        if (method != null)
            ValueModified += method;
    }
    public void RegisterModEvent(ModifiedEvent method)
    {
        ValueModified += method;
    }
    public void UnRegisterModEvent(ModifiedEvent method)
    {
        ValueModified -= method;
    }
    public void UpdateModifiedValue() {
        var valueToAdd = 0;
        GameLog.LogMessage("UpdateModifiedValue" + valueToAdd);
       
            GameLog.LogMessage("UpdateModifiedValue modifiers cound" + modifiers?.Count);
        if (modifiers != null)
        for (int i = 0; i < modifiers.Count; i++)
        {
            GameLog.LogMessage("modifiers list i=" + i+" modifiers[i]"+ ((ItemBuff)modifiers[i]).attribute);
            modifiers[i].AddValue(ref valueToAdd);
        }
        GameLog.LogMessage("base value" + baseValue +" BaseValue"+BaseValue+ "value to Add " + valueToAdd);
        ModifiedValue = baseValue + valueToAdd;
        GameLog.LogMessage("Modified value" + ModifiedValue);
        if (ValueModified != null) {
            GameLog.LogMessage("ValueModified Invoked");
            ValueModified.Invoke();
        }
        else
            GameLog.LogMessage("ValueModified null");
    }
    public void AddModifier(IModifiers _modifier)
    {
        if (modifiers == null)
            modifiers = new List<IModifiers>();
        modifiers.Add(_modifier);
        UpdateModifiedValue();
    }
    public void RemoveModifier(IModifiers _modifier)
    {
        GameLog.LogMessage("removed Modifier entered"+ modifiers.Count);
        
        modifiers.Remove(_modifier);
        GameLog.LogMessage("removed Modifier" + modifiers.Count);
        for (int i = 0; i < modifiers.Count; i++)
        {
            GameLog.LogMessage("existing modifier atrribute:"+((ItemBuff)modifiers[i]).attribute+" value:"+ ((ItemBuff)modifiers[i]).value);
        }
        UpdateModifiedValue();
    }

    public void RemoveAllModifiers() {

        if(modifiers != null)
             modifiers.Clear();   
        UpdateModifiedValue();
    }

    public void Reset() {

        RemoveAllModifiers();
        ModifiedValue = baseValue;
    }
}