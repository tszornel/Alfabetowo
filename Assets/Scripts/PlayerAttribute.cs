using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerAttribute{
    [System.NonSerialized]
    public Player parent;
    public AttributesName type;
    public ModifiableInt value;
    public void SetParent(Player _parent) {
        this.parent = _parent;
        value = new ModifiableInt(AtrributeModifier);
    }
    public void AtrributeModifier() {
        parent.AtrributeModifier(this);
    }
}
