using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PhysicsObject : MonoBehaviour
{
    public PlayerData playerData;
    protected Vector2 targetVelocity;
    protected bool grounded;
    protected Vector2 groundNormal;
    protected Rigidbody2D rb2d;
    protected Vector2 velocity;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);
    protected const float minMoveDistance = 0.001f;
    protected const float shellRadius = 0.05f;
    protected GroundType groundType;
    public bool superjump = false;
    public bool startedOneWayCollision = false;
    public bool isOnOneWayPlatform=false;
    public Action OnOneWayColliderLeftAction = null;
    public Action LeavePlatform = null;
       
    void SetGroundType(string tagName) {

        switch (tagName)
        {
            case "Ground":
                groundType = GroundType.NormalGround;
                break;
            case "Stones":
                groundType = GroundType.Stones;
                break;
            case "Leafs":
                groundType = GroundType.Leafs;
                break;
            case "Water":
                groundType= GroundType.Water;
                break;
            case "Home":
                groundType= GroundType.Home;
                break;
            default:
                groundType = GroundType.None;
                break;
        }

    }

   
void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        //GameLog.LogMessage("LayerMask"+Physics2D.GetLayerCollisionMask(gameObject.layer).ToString());
        contactFilter.useLayerMask = true;
    }
    void Update()
    {
        targetVelocity = Vector2.zero;
        ComputeVelocity();
    }
    protected virtual void ComputeVelocity()
    {
    }
    void FixedUpdate()
    {
        velocity += playerData.gravityModifier * Physics2D.gravity * Time.fixedDeltaTime;
        velocity.x = targetVelocity.x;
        grounded = false;
        Vector2 deltaPosition = velocity * Time.fixedDeltaTime;
        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);
        Vector2 move = moveAlongGround * deltaPosition.x;
        Movement(move, false);

        if (superjump)
            move += Vector2.up * deltaPosition.y;
        else
            move = Vector2.up * deltaPosition.y;
        Movement(move, true);
      //  GameLog.LogMessage("Movement" + move.ToString());
    }
    void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;
        if (distance > minMoveDistance)
        {
            int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
            hitBufferList.Clear();
            for (int i = 0; i < count; i++)
            {
                hitBufferList.Add(hitBuffer[i]);
            }
            for (int i = 0; i < hitBufferList.Count; i++)
            {
                Vector2 currentNormal = hitBufferList[i].normal;
                if (currentNormal.y > playerData.minGroundNormalY)
                {
                    //  GameLog.LogMessage("Ground Type tag" + hitBufferList[i].transform.tag + " ground type name" + hitBufferList[i].transform.name, hitBufferList[i].transform);
                    if (isOnOneWayPlatform && hitBufferList[i].transform.tag != "OneWayCollider") {
                        OnOneWayColliderLeftAction?.Invoke();
                        
                    }
                        
                    grounded = true;
                    SetGroundType(hitBufferList[i].transform.tag);
                    if (yMovement)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }
                float projection = Vector2.Dot(velocity, currentNormal);
                if (projection < 0)
                {
                    // GameLog.LogMessage("Projection < 0"+ projection);
                    velocity = velocity - projection * currentNormal;
                }
                //GameLog.LogMessage("Projection > 0" + projection);
                float modifiedDistance = hitBufferList[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }
      //  GameLog.LogMessage("Grounded" + grounded);
        rb2d.position = rb2d.position + move.normalized * distance;
    }

}