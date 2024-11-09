﻿using UnityEngine;
using Transform = UnityEngine.Transform;

public class FollowBehaviour : StateMachineBehaviour
{
    Transform playerPos;
    public float speed;
    private bool moveRight;
    public float stopDistance=0;

    private void CheckDirection(Transform movedObject)
    {

        if (movedObject.position.normalized.x < Vector2.MoveTowards(movedObject.position, playerPos.position, speed * Time.deltaTime).normalized.x)
        {
            //  GameLog.LogMessage("MOVERIGHT");
            moveRight = true;
        }
        else
        {

            //  GameLog.LogMessage("MOVELEFT");
            moveRight = false;
        }

        if (moveRight)
            movedObject.eulerAngles = new Vector3(0, 0, movedObject.eulerAngles.z);
        else
            movedObject.eulerAngles = new Vector3(0, 180, movedObject.eulerAngles.z);
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!playerPos)
            playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CheckDirection(animator.transform);

        bool followDistance = Vector2.Distance(animator.transform.position, playerPos.position) > stopDistance;
        // GameLog.LogMessage("player.playerData.FireflyFollows=" + player.playerData.FireflyFollows + "follow Distance> stop Distance" + followDistance);
        if (followDistance)
        {
            animator.transform.position = Vector2.MoveTowards(animator.transform.position, playerPos.position, speed * Time.deltaTime);
        }
    }
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}
    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}