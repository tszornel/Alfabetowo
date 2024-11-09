using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
  //  private float dustIstantiateTime = 1f;
    public float moveSpeed;
    public float jumpHeight = 10.0f;
    private Rigidbody2D player;
    public Transform groundCheck;
    public Transform attackPosition;
    public float groundCheckRadius;
    public LayerMask whatIsGround;
    private bool grounded;
   // private bool doubleJump;
    private Animator anim;
    public GameObject dust;
    public GameObject trailObject;
    private bool spawnDust;
    private float moveInput;
    private Animator cameraAnim;
    private AudioSource source;
    public AudioClip landingSound;
    private TrailRenderer trail;
    public Sprite fullheart;
    public Sprite emptyHeart;
    public UnityEngine.UI.Image[] hearts;
    int currentHealth;
    public Animator hurtAnim;
    private SceneTransitions sceneTransitions;
   // private bool attacking;
    private bool damage;
   // private GameObject instantiatedObj;
   // private bool jumping;
    public GameObject pickupEffect;
    private TMP_Text spawnedTextObjectComponent;
    private float timeBtwTrails;
    public float startTimeBtwTrails;
    public GameObject trailEffect;
    public float startTimeBtwAttacks;
    private float timeBtwAttack;
    public float attackRange;
    public LayerMask whatIsEnemies;
    private bool attack;
    public int damageAmount;
    private bool start;
    // Use this for initialization
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        // anim.ResetTrigger("hit");
        spawnDust = true;
        source = GetComponent<AudioSource>();
        cameraAnim = Camera.main.GetComponent<Animator>();
        if (trailObject != null)
        {
            trail = trailObject.GetComponent<TrailRenderer>();
            trail.enabled = false;
        }
        sceneTransitions = FindObjectOfType<SceneTransitions>();
        currentHealth = hearts.Length;
        //player.AddForce(new Vector2(10, 20), ForceMode2D.Impulse);
        player.velocity = new Vector2(player.velocity.x, jumpHeight);
        player.velocity = new Vector3(player.velocity.x * moveSpeed * 4, player.velocity.y, 0);
        float step = moveSpeed * Time.deltaTime; // calculate distance to move
        start = true;
    }
    IEnumerator TakeDamageRoutine()
    {
        damage = true;
        var oppositex = -player.velocity.x;
        var oppositey = -player.velocity.y;
        float brakePower = 4f;
        Vector2 brakeForce = new Vector2(oppositex * brakePower, oppositey * brakePower);
        player.velocity = brakeForce;
        GameLog.LogMessage("HIT SET !!!!!!!!!!");
        anim.SetTrigger("hit");
        hurtAnim.SetTrigger("hurt");
        // source.PlayOneShot();
        cameraAnim.SetTrigger("shake");
        if (currentHealth < 0)
        {
            sceneTransitions.LoadScene("GameOver",4);
            Destroy(this.gameObject);
        }
        yield return new WaitForSeconds(1);
         damage = false;
     }
    public void TakeDamage(int amount)
    {
        GameLog.LogMessage("Take damage amount=" + amount);
       //          GameLog.LogMessage("Take damage amount=" + amount);
            currentHealth -= amount;
            updateHeartUI(currentHealth);
            StartCoroutine(TakeDamageRoutine());
    }
    void updateHeartUI(int currenthealth)
    {
        GameLog.LogMessage("Update hearts" + currenthealth);
        for (int i = 0; i < hearts.Length; i++)
        {
            bool test = i < currenthealth;
            GameLog.LogMessage("test i=" + i + " currentHealth=" + currenthealth + " test=" + test);
            if (i < currenthealth)
            {
                hearts[i].sprite = fullheart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameLog.LogMessage("'Collides with" + collision.collider.tag.ToString());
        if (!damage)
        {
            if (collision.collider.CompareTag("Enemy"))
            {
                GameLog.LogMessage("Strata Życia!!!");
                //animation to take Damage.
                TakeDamage(1);
            }
            else if (collision.collider.CompareTag("Water_collider"))
            {
                GameLog.LogMessage("Toniecie.Strata Życia!!!");
                anim.SetTrigger("toniecie");
                sceneTransitions.LoadScene("GameOver",4);
            }
            //moved to inventory 
            /* else if (collision.collider.CompareTag("Letter")){
                 GameLog.LogMessage("Collides with letter");
                 GameObject other = collision.gameObject;
                 spawnedTextObjectComponent = other.GetComponent<TMP_Text>();
                 string letter = spawnedTextObjectComponent.text;
                 GameLog.LogMessage("Letter found" + letter);
                 Instantiate(pickupEffect, transform.position, Quaternion.identity);
                 Destroy(other);
             }*/
        }
    }
    //nowy Update
    void FixedUpdate()
    {
        if (damage)
        {
            GameLog.LogMessage("return form fixed update");
            return;
        }
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        moveInput = Input.GetAxisRaw("Horizontal");
        player.velocity = new Vector2(moveInput * moveSpeed, player.velocity.y);
        if (moveInput < 0)
        {
            transform.eulerAngles = new Vector3(0, 180f, 0);
        }
        else if (moveInput > 0)
        {
            transform.eulerAngles = new Vector3(0, 0f, 0);
        }
        if (damage || attack) return;
        if (moveInput != 0 && grounded == true)
        {
            anim.SetBool("isRunning", true);
            Instantiate(dust, groundCheck.position, Quaternion.identity);
            if (timeBtwTrails <= 0)
            {
                timeBtwTrails = startTimeBtwTrails;
            }
            else
            {
                Instantiate(trailEffect, groundCheck.position, Quaternion.identity);
                timeBtwTrails -= Time.deltaTime;
            }
        }
        else if (grounded == true && moveInput == 0)
        {
            anim.SetBool("isRunning", false);
        }
    }
    IEnumerator Attack()
    {
        attack = true;
        GameLog.LogMessage("moveHand trigger entered");
        trail.enabled = true;
        anim.SetTrigger("moveHand");
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, whatIsEnemies);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(damageAmount);
        }
        yield return new WaitForSeconds(1);
        trail.enabled = false;
        attack = false;
    }
    IEnumerator JumpingRoutine()
    {
        //anim.ResetTrigger("hit");
        // GameLog.LogMessage("Next animator state info"+anim.GetNextAnimatorStateInfo(0).ToString());
       // jumping = true;
        anim.SetBool("isJumping", true);
        //  GameLog.LogMessage("Jumping trigger entered !!!!!!!!!!!!!!!:"+damage);
        anim.SetTrigger("jump");
        player.velocity = new Vector2(player.velocity.x, jumpHeight);
        yield return new WaitForSeconds(0);
       // jumping = false;
        anim.SetBool("isJumping", false);
    }
    IEnumerator MovePieceTowards(Rigidbody2D piece, Vector3 end, float speed)
    {
        while (piece.transform.position != end)
        {
            piece.transform.position = Vector3.Slerp(piece.transform.position, end, 1f);
            yield return null;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            GameLog.LogMessage("move player" + transform.position);
            Vector3 target = transform.TransformPoint(new Vector3(10f, 6f, 0));
            //Vector3 player_velocity = player.velocity;
            //player.position = Vector3.Lerp(player.position, target, Time.deltaTime * 0.5f);
            //player.velocity = new Vector2(player.velocity.x * 4* Time.deltaTime, player.velocity.y * jumpHeight * Time.deltaTime);
            transform.position = Vector2.MoveTowards(player.position, target, Mathf.Infinity);
            //rotate towards a direction, but not immediately (rotate a little every frame)
            // StartCoroutine(MovePieceTowards(player, target, 10));
            //transform.position = Vector3.Slerp(player.position, target, 1f);
            //transform.rotation = Quaternion.Slerp(transform.rotation, , 1);
            // transform.position = Vector3.SmoothDamp(player.position, target,ref player_velocity, 0.5f);
            GameLog.LogMessage("target  player position:" + player.position);
            start = false;
        }
        if (damage)
        {
            GameLog.LogMessage("return from update");
            return;
        }
        if (grounded)
        {
           // doubleJump = false;
            if (spawnDust == true)
            {
                Instantiate(dust, groundCheck.position, Quaternion.identity);
                spawnDust = false;
                cameraAnim.SetTrigger("shake");
                source.clip = landingSound;
                source.Play();
            }
        }
        else
        {
            spawnDust = true;
        }
        anim.SetBool("Grounded", grounded);
        anim.SetFloat("vVelocity", player.velocity.y);
        if (grounded && Input.GetButtonUp("Jump") && !damage)
        {
            StartCoroutine(JumpingRoutine());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StartCoroutine(Attack());
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition.position, attackRange);
    }
}
