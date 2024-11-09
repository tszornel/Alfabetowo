using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
      
        if (collision.CompareTag("Player")) {
            GameLog.LogMessage("collide with spider");
            StartCoroutine(spiderWalking());
        }

    }

    IEnumerator spiderWalking() {
        anim.SetBool("up", true);
        yield return new WaitForSeconds(1);
        anim.SetBool("up", false);
        anim.SetBool("down", true);
        yield return new WaitForSeconds(1);
        anim.SetBool("up", false);
        anim.SetBool("down", false);
       

    }
}
