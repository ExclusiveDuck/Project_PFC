using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum AIState { Patrol, Chase, Search }
public class NPCBehaviour : MonoBehaviour
{
    public AIState myState = AIState.Patrol;
    public NavMeshAgent agent;
    public List<PatrolPoint> myPatrolPoints = new List<PatrolPoint>();
    public bool canSeePlayer = false;
    public float fieldOfView = 100f;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        ProcessPerception();

        // run the current state method
        if (myState == AIState.Patrol)
            Patrol();

        if (myState == AIState.Chase)
        {
            Chase();
        }
        if (myState == AIState.Search)
        {
            Search();
        }

    }

    void Patrol()
    {
        Debug.Log("I'm Patrolling");
    }

    void Chase()
    {
        Debug.Log("I'm Chasing");
    }

    void Search()
    {
        Debug.Log("I'm Searching");
    }

    void ProcessPerception()
    {
        //check if we can see the player

    }

 

    public void SetDestination(Vector3 destination, float stoppingDistance)
    // My own custom way of giving an NPC a new destination (as well as being able to pass in a stoppingDistance (-1 for no change)
    {


        NavMeshPath path = new NavMeshPath();

        NavMeshHit hit;

        //Use NavMesh.SamplePisition to find a valid position on the NavMeshFF
        if (NavMesh.SamplePosition(destination, out hit, 2.0f, NavMesh.AllAreas))
        {
            // if we find a point on the NacMesh, Calculate the path
            if (agent.CalculatePath(hit.position, path))
            {
                // set destination 
                agent.destination = hit.position;

                // set stopping distance
                if (stoppingDistance != -1)
                    agent.stoppingDistance = stoppingDistance; // pass in -1 for "dont change the value, leave it at whatever it was already".

                agent.isStopped = false;
            }
            else
            {
                // Log a warning if the agent cannot reach the destination
                Debug.LogWarning("SET DEST: Agent cannot reach the destination!");
            }
            
        }
        else
        {
            // Log a warning if a valid position could not be found on the NavMesh
            Debug.LogWarning("SET DEST: Unable to find the destination!");
        }

    }
}   

