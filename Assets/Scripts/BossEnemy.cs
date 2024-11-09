using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Webaby.Utils;

public class BossEnemy : Enemy
{
    public float stopDistance;
    private float attackTime;
    public int damage;
    public float attackSpeed;
    public Transform groundDetection;
    private RaycastHit2D groundInfoCheck;
    
    public int pickupChance;
    public GameObject[] pickups;
    public CutsceneTimelineBehaviour bossTimeline;
 

    IEnumerator AttackCoroutine()
    {
        anim?.SetBool("isAttacking",true);
        int damageNew = GetPowerValueBuff();
        if (audioBehaviour)
            audioBehaviour.PlayAttack(null);
        playerController.TakeDamage(damageNew);
        Vector2 originalPosition = transform.position;
        Vector2 targetPosition = player.position;
        float percent = 0;
        while (percent <= 1)
        {
            percent += Time.deltaTime * attackSpeed;
            float formula = (-Mathf.Pow(percent, 2) + percent) * 4;
            transform.position = Vector2.Lerp(originalPosition, targetPosition, formula);
        }
        yield return new WaitForSeconds(0.5f);
        anim?.SetBool("isAttacking", false);
    }
    public void Attack()
    {
        GameLog.LogMessage("Attack");
        StartCoroutine(AttackCoroutine());
    }



        
    void Update()
    {
      //  GameLog.LogMessage("is Boss Visible"+transform.name+" isVisible="+isVisible);
        if (healthBehaviour.GetCurrentHealth() <= 0)
        {
            DeathEffect();
            int randomNumber = Random.Range(1, 101);
            if (randomNumber < pickupChance)
            {

             //   var random = new System.Random();
              //  int index = random.Next(itemList.Count);
                int count = itemList.Count;
              //  GameLog.LogMessage("Count="+ itemList.Count+"index=" + index);
                if (count > 0)
                {
                    if (droppedItemsAmount > 0)
                        count = droppedItemsAmount;
                    for (int i = 0; i < count; i++)
                    {
                        var random = new System.Random();
                        int index = random.Next(itemList.Count);
                        PickupItem instantiatedPickup = PickupItem.SpawnItemWorld(transform.position, Quaternion.identity, itemList.ElementAt(index), transform.parent);//Instantiate(randomPickup, transform.position, transform.rotation);
                        instantiatedPickup.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * 20f);
                        itemList.RemoveAt(index);

                    }
                   // itemList.Clear();
                }

            }
            Destroy(gameObject);
        }
        //transform.Translate(Vector2.left * speed * Time.deltaTime);
        if (player != null)
        {
            // GameLog.LogMessage("Player is noy null");
            if (Vector2.Distance(transform.position, player.position) > stopDistance)
            {
                CheckDirection(player, transform);

                if (isVisible) 
                { 
                    transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
                    Vector2 prevDir = transform.TransformDirection(transform.position);
                    Vector2 moveDir = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
                    anim.SetBool("isFollowing", true);
                }

              /*  LayerMask groundLayer = LayerMask.NameToLayer("Ground");
                if(groundDetection)
                    groundInfoCheck = Physics2D.Raycast(groundDetection.position, Vector2.down, Mathf.Infinity, 1 << LayerMask.NameToLayer("Ground"));
*/
           
            }
            else
            {
                if (Time.time >= attackTime)
                {
                    anim.SetBool("isFollowing", false);
                    Attack();
                    attackTime = Time.time + timeBtwAttack;
                }
            }
        }
        else
        {
           // GameLog.LogMessage("Find player");
            if (GameObject.FindGameObjectWithTag("Player") != null)
                player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

  

}
