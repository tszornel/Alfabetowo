using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneSideCollider : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]PlayerPlatformerController player;
    GameObject playerTransform;
    GameObject groundCheck;
    Collider2D currentCollider;
    RaycastHit2D groundDetection;
    public float distance=Mathf.Infinity;
    private bool isOnCurrentPlatform;
    Collider2D collision;



    void Awake()
    {
        currentCollider = GetComponent<BoxCollider2D>();
        if (!player)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player");
            player = playerTransform.GetComponent<PlayerPlatformerController>();

           
        }
        player.LeavePlatform += PlayerCroachingOnThePlatformSubscription;
        player.OnOneWayColliderLeftAction += PlayerLeftThePlatform;
    }

    IEnumerator PlayerLeftPlatformWithCrouchActionRoutine()
    {


        yield return new WaitForSeconds(0.25f);
        if (player.croach)
        {

            GameLog.LogMessage("PlayerLeftThePlatform Action" + currentCollider.isTrigger);
            currentCollider.isTrigger = true;
            GameLog.LogMessage("PlayerLeftThePlatform Action left" + currentCollider.isTrigger);
            player.isOnOneWayPlatform = false;

        }

       
    }


    private void OnDisable()
    {
        player.LeavePlatform -= PlayerCroachingOnThePlatformSubscription;
        player.OnOneWayColliderLeftAction -= PlayerLeftThePlatform;
    }

    private void PlayerLeftThePlatform()
    {
        if (isOnCurrentPlatform && (player.isOnOneWayPlatform || player.startedOneWayCollision))
        {
            GameLog.LogMessage("PlayerLeftThePlatform entered" + currentCollider.isTrigger);
            player.isOnOneWayPlatform = false;
            player.startedOneWayCollision = false;
            currentCollider.isTrigger = true;
            GameLog.LogMessage("PlayerLeftThePlatform Action left" + currentCollider.isTrigger);
            isOnCurrentPlatform = false;
        }
    }

    void PlayerLeftThePlatformSubscription() {
       

            StartCoroutine(CheckColliderRoutine());

      

    }



    void IgnoreColliderRoutine(Collider2D collision) {

       
        //  StartCoroutine(IgnoreColliderRoutine2());
        Debug.Log("OnTriggernExit2D Collide with one way platform");
        GameLog.LogMessage("Kolicja z playerem" + player.IsJumping);
        GetComponent<BoxCollider2D>().isTrigger = false;
        player.isOnOneWayPlatform = true;
        isOnCurrentPlatform = true;

       // yield return null;
    }



     IEnumerator CheckColliderRoutine()
     {

        
       // Debug.Log("Is jumping " + player.IsJumping);
       
        
           // GameLog.LogMessage("Player nie skacze");
            groundDetection = Physics2D.Raycast(player.transform.position, Vector2.down, distance, 1 << LayerMask.NameToLayer("OneWayCollider"));
            Debug.DrawLine(transform.position, groundDetection.point, Color.red);
            if (groundDetection.collider == false)
            {
                GameLog.LogMessage("Player oposcil platforme  na platformie:"+ groundDetection.collider.transform.name +" obecnie na platformie "+isOnCurrentPlatform);
            //player spadl i trzeba odchaczyc 
            if (!player.IsJumping)
                PlayerLeftThePlatform();


            }

        yield return null;


    }
   /* private void OnDrawGizmos()
    {
        Gizmos.DrawLine(collision.transform.position, collision.ClosestPoint(currentCollider.transform.position));
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("OnTriggerEnter2D with one way platform");
        player.startedOneWayCollision = true;
       

        //GameLog.LogMessage("DOTS"+Vector2.Dot(collision.transform.position.normalized, collision.ClosestPoint(currentCollider.transform.position.normalized)));
      //  Collider2D collider2=  Physics2D.OverlapCircle(collision.ClosestPoint(collision.transform.position), 2f);


      
    }
    
    
    private void PlayerCroachingOnThePlatformSubscription() {
        
            StartCoroutine(PlayerLeftPlatformWithCrouchActionRoutine());
           
      

    }

    
    private void Update()
    {
       // Debug.Log("UPDATE " + player.isOnOneWayPlatform + "   started " + player.startedOneWayCollision);
       

     }

    private void OnTriggerExit2D(Collider2D collision)
    {

        Debug.Log("trigger exit !!!!!!!!!!!!!!!!!!!!");
        if (!collision.CompareTag("Player"))
            return;

       // if (!player.IsJumping)
       //     return;

        Debug.Log("OnTriggerEnter2D player is ready with one way platform");
        
        IgnoreColliderRoutine(collision);
    }

}
