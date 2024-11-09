using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class BasicEnemy : Enemy
{
    public float stopDistance;
    private float attackTime;
    public int damage;
    public float attackSpeed;
    public Transform groundDetection;
    private RaycastHit2D groundInfoCheck;
    
    public int pickupChance;
   // public GameObject[] pickups;
    


    
    IEnumerator AttackCoroutine()
    {
       
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
        yield return new WaitForSeconds(0.3f);
    }
    public void Attack()
    {
        isVisible = true;
        GameLog.LogMessage("Attack");
        StartCoroutine(AttackCoroutine());
    }
    // Update is called once per frame
    void Update()
    {
        //GameLog.LogMessage("Basic enemy update");
        if (healthBehaviour.GetCurrentHealth() <= 0)
        {
            int randomNumber = Random.Range(1, 101);
            if (randomNumber < pickupChance) {

                var random = new System.Random();
                int index = random.Next(itemList.Count);
                GameLog.LogMessage("index"+index);
                // GameLog.LogMessage("item NAME:" + itemList.ElementAt(index).Name);
                if(index  < itemList.Count) { 
                    PickupItem instantiatedPickup = PickupItem.SpawnItemWorld(transform.position, Quaternion.identity, itemList.ElementAt(index), transform.parent);//Instantiate(randomPickup, transform.position, transform.rotation);
                    instantiatedPickup.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * 20f);
                    itemList.RemoveAt(index);
                }
            }
            //Destroy(gameObject);
            EnemyDeath();
             //ReleseToPool();
        }
        //transform.Translate(Vector2.left * speed * Time.deltaTime);
        if (player != null)
        {
         //   GameLog.LogMessage("Player is not null"+player.name+"mol name="+transform.name);
            float distance = Vector2.Distance(transform.position, player.position);
            if (distance > stopDistance)
            {
                CheckDirection(player, transform);
                // GameLog.LogMessage("Distance"+ distance +"> stopDistance"+ stopDistance);
                transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
               
                anim.SetBool("isFollowing", true);
               // LayerMask groundLayer = LayerMask.NameToLayer("Ground");
              //  groundInfoCheck = Physics2D.Raycast(groundDetection.position, Vector2.down, Mathf.Infinity, 1 << LayerMask.NameToLayer("Ground"));
               // GameLog.LogMessage("Ground check inf" + groundInfoCheck);
              
            }
            else
            {
               // GameLog.LogMessage("Attacking attackTime="+ attackTime+" Time="+ Time.time);
                if (Time.time >= attackTime)
                {
                    Attack();
                    attackTime = Time.time + timeBtwAttack;
                }
            }
        }
        else
        {
            GameLog.LogMessage("Find player");
            if (GameObject.FindGameObjectWithTag("Player") != null)
                player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }
}
