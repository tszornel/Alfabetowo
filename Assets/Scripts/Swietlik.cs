using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Webaby.Utils;

public class Swietlik : MonoBehaviour
{
    public Transform playerTransform;
    public float stopDistance;
    public float speed;
    private Animator _animator;
    private Player player;
    
    // Start is called before the first frame update
    void Start()
    {
       // stopDistance = 5;
       // speed = 2;
}
    private void Awake()
    {
        if (player == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            if (playerTransform != null)
                player = playerTransform.GetComponent<Player>();

            _animator = Transform.FindObjectOfType<Animator>(); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        float distanceBeetwen = Vector2.Distance(transform.position, playerTransform.position);
      //  GameLog.LogMessage("Distance:" + distanceBeetwen);
        if (player != null)
        {

            if(_animator && UtilsClass.AnimatorHasParameter(_animator,"guarding"))
                 _animator.SetTrigger("guarding");
            // GameLog.LogMessage("Player is noy null");

            bool followDistance = Vector2.Distance(transform.position, playerTransform.position) > stopDistance;
            GameLog.LogMessage("player.playerData.FireflyFollows=" + player.playerData.FireflyFollows + "follow Distance> stop Distance" + followDistance);
             if (player.playerData.FireflyFollows && Vector2.Distance(transform.position, playerTransform.position) > stopDistance)
             {
                GameLog.LogMessage("FOLLOW SUCCESS");
                transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
            }
        }
}
}
