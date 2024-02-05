using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class followScript : MonoBehaviour
{
    public Transform[] waypoints;
    public Animator animator;
    public Transform player;
    private NavMeshAgent agent;
    private Vector3 startPos;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        startPos = transform.position;

        
    }

    // Update is called once per frame
    void Update()
    {
        //calculate distance to player
        float distance = Vector3.Distance(player.transform.position, transform.position);
        if (distance < 10f)
        {
            //follow the player
            FollowPlayer();
        }
        else
        {
           Back();
       }
    }
    public void FollowPlayer()
    {
        agent.SetDestination(player.transform.position);
        animator.SetBool("isWalking", true);
    }
    public void Back()
    {
        if(Vector3.Distance(transform.position, startPos) > 0.1f)
        {
            agent.SetDestination(startPos);
            animator.SetBool("isWalking", true);
        }
        else
        {
            //Patroll();
        }
        
    }
    
    public void goTo(Transform target)
    {
        agent.SetDestination(target.position);
        animator.SetBool("isWalking", true);
    }
    /*public void Patroll()
    {
        
        agent.SetDestination(waypoints[Random.Range(0, waypoints.Length)].position);
    }*/
}
