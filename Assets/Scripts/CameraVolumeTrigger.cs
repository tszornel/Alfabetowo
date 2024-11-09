using Cinemachine;
using UnityEngine;



[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class CameraVolumeTrigger : MonoBehaviour
{

    [SerializeField] CinemachineVirtualCamera targetCamera;
    [SerializeField] Vector2 boxSize;
    [SerializeField] GameObject CameraGroup;
    [SerializeField] Player player;
    [SerializeField] GameObject lighter;


    BoxCollider2D box;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        box.isTrigger = true;
        // box.size = boxSize;
     
        rb.isKinematic = true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            if (!player)
                player = collision.transform.GetComponent<Player>();
            GameLog.LogMessage("Collide with player so switch cam entered ");
            if (CameraSwitcher.ActiveCamera != targetCamera) { 
            
                if(CameraGroup && player.playerData.FireflyFollows && lighter)
                    CameraSwitcher.SwitchCamera(targetCamera,CameraGroup,lighter);
                else
                    CameraSwitcher.SwitchCamera(targetCamera);
            }
                
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            if (!player)
                player = collision.transform.GetComponent<Player>();
            
                 
            }

     }
   
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;  
        Gizmos.DrawWireCube(transform.position, boxSize);   
    }
}
