using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FlyingScript : MonoBehaviour
{
    public float speed;
    public float distance;
    private bool movingLeft = true;
    Camera cameraObject;
    public Transform cameraBorderChecker;
    private Vector3 maxDistance;
    TMP_Text spawnedTextObjectComponent;
    public float maxTime = 5;
    public float minTime = 2;
    //current time
    private float time;
    private float spawnTime;
    public GameObject spawnedTextObject;
    public float maxXPosition;
    public float minXPosition;
    public Transform groundDetection;
    private RaycastHit2D groundInfoCheck;
    void Start()
    {
        cameraObject = Camera.main;
        spawnedTextObjectComponent = spawnedTextObject.GetComponent<TMP_Text>();
        SetRandomTime();
        time = minTime;
    }
    void SpawnLetter() {
        time = minTime;
        string letter = GetRandomLetter().ToString();
        print("letter:" + letter);
        spawnedTextObjectComponent.text = ""+ GetRandomLetter().ToString();
        print("spawned text:" + spawnedTextObjectComponent.text);
        spawnedTextObject.tag = "Letter";
        GameLog.LogMessage("spawnedTextObject tag:" + spawnedTextObject.tag);
        GameObject go  = Instantiate(spawnedTextObject, transform.position, Quaternion.identity) as GameObject;
        var dir = go.GetComponent<Rigidbody2D>();
        go.transform.rotation = Quaternion.LookRotation(dir.velocity);
        go.tag = "Letter";
        GameLog.LogMessage("instantiated tag:" + go.tag);
    }
    void SetRandomTime() {
        spawnTime = Random.Range(minTime, maxTime);
        GameLog.LogMessage("Next object will be spawned in" + spawnTime);
    }
    private char GetRandomLetter() {
        string samogloski = "aeiouy";
        return samogloski[Random.Range(0, samogloski.Length)];
    }
    void Update()
    {
    }
    void FixedUpdate()
    {
        //instantiane letter
        time += Time.deltaTime;
        if (time >= spawnTime) {
            SpawnLetter();
            SetRandomTime();
            time = 0;
        }
        transform.Translate(Vector2.left * speed * Time.deltaTime);
        LayerMask groundLayer = LayerMask.NameToLayer("Ground");
        groundInfoCheck = Physics2D.Raycast(groundDetection.position, Vector2.down, Mathf.Infinity, 1 << LayerMask.NameToLayer("Ground"));
       // GameLog.LogMessage("Ground check inf" + groundInfoCheck);
        if (groundInfoCheck.collider == false)
        {
            GameLog.LogMessage("colider false:");
            if (movingLeft == false)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
                movingLeft = true;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingLeft = false;
            }
        }
        /*
        Ray botrightCorner = camera.ViewportPointToRay(new Vector3(1, 0, 0));
        int layerMask2 = 1 << 12;
        int layer_mask2 = LayerMask.GetMask("Books");
        Vector3 direction2 = camera.ViewportToScreenPoint(new Vector3(0, 1, 0));
        RaycastHit2D hit2 = Physics2D.Raycast(botrightCorner.origin, direction2,Mathf.Infinity, layerMask2);
        if (hit2.collider != null)
        {
            //print("hitted  " + hit2.transform.name);
            Debug.DrawRay(botrightCorner.origin, direction2, Color.red);
            if (hit2.transform.Equals(cameraBorderChecker) || (transform.position.x > maxXPosition) || (transform.position.x < minXPosition))
            {
                if (movingLeft == false)
                {
                    transform.eulerAngles = new Vector3(0, 180, 0);
                    movingLeft = true;
                }
                else
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    movingLeft = false;
                }
            }
        }
        else
        {
            //print("looking at nothing border 2");
        }
        Ray botleftCorner = camera.ViewportPointToRay(new Vector3(0, 0, 0));
        int layer_mask = LayerMask.GetMask("Books");
        Vector3 direction = camera.ViewportToScreenPoint(new Vector3(0, 1, 0));
        RaycastHit2D hit = Physics2D.Raycast(botleftCorner.origin, direction, Mathf.Infinity, 1<< LayerMask.NameToLayer("Books"));
        if (hit.collider != null)
        {
            //print("hitted  " + hit.transform.name);
            Debug.DrawRay(botleftCorner.origin, direction, Color.yellow);
            if (hit.transform.Equals(cameraBorderChecker))
            {
                if (movingLeft == true)
                {
                    transform.eulerAngles = new Vector3(0, -180, 0);
                    movingLeft = false;
                }
                else
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    movingLeft = true;
                }
            }
        }
        else {
            // print ("looking at nothing");
        }
        */
    }
}
