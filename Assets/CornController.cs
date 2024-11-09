using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornController : MonoBehaviour
{

    public void DisableCorn()
    {


        transform.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
    }
}
