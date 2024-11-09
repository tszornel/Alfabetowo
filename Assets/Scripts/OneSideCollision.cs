using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneSideCollision : MonoBehaviour {


    public Collider2D oneSideCollider;
    private GameObject playerTransform;
    [SerializeField]private PlayerPlatformerController player;
    private bool isOnCurrentPlatform=false;
    private bool started=false;
    public bool onTriggerStay=false;

    private void Awake()
    {
        if (!player)
        { 
           playerTransform = GameObject.FindGameObjectWithTag("Player");
            player = playerTransform.GetComponent<PlayerPlatformerController>();
        }
        if (!oneSideCollider)
         oneSideCollider = GetComponentInParent<Collider2D>();
        player.LeavePlatform += PlayerCroachingOnThePlatformSubscription;
      //  player.OnOneWayColliderLeftAction += PlayerLeftThePlatform;
    }
    private void PlayerLeftThePlatform()
    {
        if (isOnCurrentPlatform && (player.isOnOneWayPlatform || player.startedOneWayCollision))
        {
            GameLog.LogMessage("PlayerLeftThePlatform entered" + oneSideCollider.isTrigger);
            player.isOnOneWayPlatform = false;
            player.startedOneWayCollision = false;
            oneSideCollider.isTrigger = true;
            GameLog.LogMessage("PlayerLeftThePlatform Action left" + oneSideCollider.isTrigger);
            isOnCurrentPlatform = false;
        }
    }
    private void PlayerCroachingOnThePlatformSubscription()
    {

        StartCoroutine(PlayerLeftPlatformWithCrouchActionRoutine());



    }

    IEnumerator PlayerLeftPlatformWithCrouchActionRoutine()
    {


        yield return new WaitForSeconds(0.35f);
        if (player.croach)
        {

            GameLog.LogMessage("PlayerLeftThePlatform Action" + oneSideCollider.isTrigger);
            oneSideCollider.isTrigger = true;
            yield return new WaitForSeconds(0.35f);
            oneSideCollider.isTrigger = false;
            GameLog.LogMessage("PlayerLeftThePlatform Action left" + oneSideCollider.isTrigger);
            player.isOnOneWayPlatform = false;

        }


    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        GameLog.LogMessage("Wychodze z colidera");
        oneSideCollider.isTrigger = false;
        Physics2D.IgnoreCollision(collision, oneSideCollider, false);
        started = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!onTriggerStay)
            return;
        if (!collision.gameObject.CompareTag("Player"))
        {

            return;
        }

        //Colide with player

        //player is jumping 
       /* if (!player.IsJumping)
        {
            GameLog.LogMessage("Player nie jest w skoku");
            return;

        }*/
      
        GameLog.LogMessage("skok od gory na pltforme jednostrona"+started);
        //playerNotYet on collider
        // oneSideCollider.isTrigger = false;
        if (!started)
            StartCoroutine(IgnoreCollisionRoutine(collision, oneSideCollider));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (!collision.gameObject.CompareTag("Player")) {

            return;
        }

        //Colide with player
             
        //player is jumping 
        /*if (!player.IsJumping)
        {
            GameLog.LogMessage("Player nie jest w skoku");
            return;

        }*/

       
        GameLog.LogMessage("skok od gory na pltforme jednostrona");
        //playerNotYet on collider
        // oneSideCollider.isTrigger = false;
        StartCoroutine(IgnoreCollisionRoutine(collision, oneSideCollider));
    }

    IEnumerator IgnoreCollisionRoutine(Collider2D playerCollider,Collider2D platformCollider )
    {

        GameLog.LogMessage("IgnoreCollisionRoutine entered");
        started = true;
        GameLog.LogMessage("PlayerLeftThePlatform Action" + oneSideCollider.isTrigger);
        oneSideCollider.isTrigger = true;
        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return new WaitForSeconds(0.09f);
        GameLog.LogMessage("collider is triggered false");
        oneSideCollider.isTrigger = false;
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
        GameLog.LogMessage("PlayerLeftThePlatform Action left" + oneSideCollider.isTrigger);
        player.isOnOneWayPlatform = true;
        started = false;



    }

    private void OnDisable()
    {
        player.LeavePlatform -= PlayerCroachingOnThePlatformSubscription;
    }


}
