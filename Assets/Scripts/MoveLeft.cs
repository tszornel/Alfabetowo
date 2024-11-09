using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MoveLeft : MonoBehaviour
{
    private float startpos, xLenght, dist;
    SpriteRenderer rg;
    public float speed;
    public GameObject cam;
    private float rightObjBound, leftCamBound,objLenght;
    private Vector3 p1, p2;
    // Use this for initialization
    void Start()
    {
        rg = GetComponent<SpriteRenderer>();
        xLenght = Camera.main.aspect * Camera.main.orthographicSize * 2f;
    }
    // Update is called once per frame
    void Update()
    {
        //leftCamBound = cam.transform.position.x;
        //rightObjBound = transform.position.x;
        p1 = gameObject.transform.TransformPoint(Vector3.left);
        p2 = gameObject.transform.TransformPoint(Vector3.right);
        objLenght = p2.x - p1.x; 
        // rg.velocity = Vector2.right * speed * Time.deltaTime;
        transform.Translate(Vector3.left * speed * Time.deltaTime, Camera.main.transform);
        //GameLog.LogMessage("Pozycja rzeki" + transform.position.x + "  camera position" + cam.transform.position.x);
        // if ( (p2.x < cam.transform.position.x - xLenght/2f) && ( objLenght > xLenght/2) ) {
        //GameLog.LogMessage("entered to cam moved to much lenght"+ lenght);
        // transform.position = new Vector3((cam.transform.position.x + xLenght/2f + objLenght ), transform.position.y, transform.position.z);
        //}
        // GameLog.LogMessage("nowa pozycja");
        //  startpos = transform.position.x;
        //GameLog.LogMessage("nowa pozycja"+ startpos);   
        if (p2.x < cam.transform.position.x - xLenght) return;
        //else if (( p2.x < cam.transform.position.x - xLenght/2f - objLenght)  && (objLenght <= xLenght/2f) )
        //{
          //  transform.position = new Vector3((cam.transform.position.x + xLenght/2f + objLenght*4f ), transform.position.y, transform.position.z);
        //}
    }
}
