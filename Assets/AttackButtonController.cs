using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class AttackButtonController : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera _camera;
    private object lenght;


    private void Awake()
    {
        _camera = Camera.main;  
    }
   /* void Update()
    {
        //  var CheckPlayerBehind = Physics2D.Raycast(transform.position, transform.forward, Mathf.Infinity, 1 << LayerMask.NameToLayer("Player"));
        // GameLog.LogMessage("Ground check inf" + groundInfoCheck);
        var hit = Physics2D.Raycast(_camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f)), transform.forward, Mathf.Infinity, 1 << LayerMask.NameToLayer("Player"));
        
            if (hit.collider != null && hit.collider.name.Equals("Player"))
            {
                GameLog.LogMessage("Player schowany !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            }
    }*/

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;   
    }
}
