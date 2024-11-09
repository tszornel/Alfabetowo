using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MoveLeftCam : MonoBehaviour
{
    private float startpos, lenght, dist;
    public float speed;
    public GameObject cam;
    // Use this for initialization
    void Start()
    {
        // rg = GetComponent<Rigidbody2D>();
        startpos = transform.position.x;
        lenght = transform.GetComponent<SpriteRenderer>().bounds.size.x;
    }
    // Update is called once per frame
    void Update()
    {
        // rg.velocity = Vector2.right * speed * Time.deltaTime;
        transform.Translate(Vector3.left * speed * Time.deltaTime, Camera.main.transform);
        //GameLog.LogMessage("Pozycja rzeki" + transform.position.x + "  camera position" + cam.transform.position.x);
        if (transform.position.x < cam.transform.position.x)
        {
            //GameLog.LogMessage("entered to cam moved to much lenght"+ lenght);
            transform.position = new Vector3((cam.transform.position.x + lenght / 2) / 2, transform.position.y, transform.position.z);
            // GameLog.LogMessage("nowa pozycja");
            startpos = transform.position.x;
            //GameLog.LogMessage("nowa pozycja"+ startpos);
        }
    }
}
