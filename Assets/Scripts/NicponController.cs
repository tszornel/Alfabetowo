using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NicponController : MonoBehaviour
{
    public  GameObject effect;
    public GameObject positionObject;
    //private  Vector3 position;
    // Start is called before the first frame update
    public void StartPlasmaEffect() { Instantiate(effect, positionObject.transform.position, Quaternion.identity); }
}
