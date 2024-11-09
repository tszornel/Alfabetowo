using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ChestController : MonoBehaviour
{
    public bool isOpen;
    private Animator anim;
    public Transform objectInside;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        objectInside.gameObject.SetActive(false);
    }
    // Update is called once per frame
    public void OpenChest(GameObject player)
    {

        GameLog.LogMessage("The checs is now open");
        if (!isOpen)
        {
            isOpen = true;
            anim.SetBool("isOpen", isOpen);
            GameLog.LogMessage("The checs is now open");
            // the chest is now open
            StartCoroutine(OpenChestCouroutine());
            
        }
    }
    IEnumerator OpenChestCouroutine()
    {
        yield return new WaitForSeconds(3);
        objectInside.gameObject.SetActive(true);
       
    }
}
