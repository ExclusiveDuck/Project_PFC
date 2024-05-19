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
    public int myPatrolPointIndex = -1;
    public bool isAlive = true;
    public Transform eyePosition;

    public GameObject target;

    private RaycastHit hit;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.Find("Player");
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
        //if no patrol points then dont try to patrol
        if (myPatrolPoints.Count < 1)
            return;

        //Debug.Log("I'm Patrolling");

        //set up first patrol point on start
        if (myPatrolPointIndex == -1)
        {
            myPatrolPointIndex = 0;
            MoveToNewPatrolPoint();
        }
        
        //check if agent has reached patrol point
        if (Vector3.Distance (transform.position, myPatrolPoints[myPatrolPointIndex].transform.position) < 1f)
        {
            // increment patrol point
            myPatrolPointIndex++;
            if (myPatrolPointIndex == myPatrolPoints.Count)
            myPatrolPointIndex = 0;

            MoveToNewPatrolPoint();
        }
    }

    void MoveToNewPatrolPoint()
    {
        //send agent to patrol point
        SetDestination(myPatrolPoints[myPatrolPointIndex].transform.position, 0);
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

        if (agent.isOnNavMesh && agent.isActiveAndEnabled)
        {
            if (isAlive)
            {
                //draw a Linecast from the enemy to the player
                if (Physics.Linecast(eyePosition.transform.position, target.transform.position + transform.up * 1f, out hit))
                {
                    Debug.Log(hit.collider.name);

                    //if the name of the collider on the player (targer) == the name of the collider the raycast hit first...
                    if (hit.collider.name == "Paladin WProp J Nordstrom")
                    {
                        // if player is in my FieldOfView
                        if (Vector3.Angle(target.transform.position - eyePosition.transform.position, transform.forward) <= fieldOfView / 2)
                        {
                            // draw GREEN line
                            Debug.DrawLine(eyePosition.transform.position, target.transform.position + transform.up * 1f, Color.green, 0.1f);
                            canSeePlayer = true;
                        }
                        else
                        {
                            //draw BLUE line (player is outside fieldofview)
                            Debug.DrawLine(eyePosition.transform.position, target.transform.position + transform.up * 1f, Color.blue, 0.1f);
                            canSeePlayer = false;
                        }
                    }
                    else
                    {
                        //draw RED line (player is blocked by something)
                        Debug.DrawLine(eyePosition.transform.position, target.transform.position + transform.up * 1f, Color.red, 0.1f);
                        canSeePlayer = false;
                    }
                }
            }
        }
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

