using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;


public enum AIState { Patrol, Chase, Search }

[SelectionBase]
public class NPCBehaviour : MonoBehaviour
{
    [Header("General AI variables")]
    public AttributesManager attributesManager;
    public Animator anim;
    public AIState myState = AIState.Patrol;
    public NavMeshAgent agent;
    public bool isAlive = true;
    public bool canSeePlayer = false;
    public float fieldOfView = 100f;
    public float perceptionDistance = 10f;
    public Transform eyePosition;
    public GameObject target;
    public float debugLineDuration = 1f;
    public float rotateSpeed = 20f;

    public AudioManager audioManager;

    [Header("Move Speeds")]
    public float patrolSpeed = 1.4f;
    public float chaseSpeed = 3.0f;


    [Header("Patrol State")]
    public List<PatrolPoint> myPatrolPoints = new List<PatrolPoint>();
    public int myPatrolPointIndex = -1;

    [Header("Chase State")]
    public float lostPlayerTimer = 0f;
    public float lostPlayerGiveUpDuration = 2f;

    [Header("Search State")]
    public float searchTimer = 0f;
    public float searchDuration = 4f;

    // Private variables
    private RaycastHit hit;

    [Header("Attack State")]
    public BoxCollider attackbox;
    public float attackRange;
    public bool canMove;
    public bool canAttack;
    public float attackResetTime;

    private void Start()
    {
        // get reference to the audio manager
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }




    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.Find("PlayerArmature");
    }



    void Update()
    {
        if (agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            ProcessPerception();

            // run the current state method
            if (myState == AIState.Patrol && canMove)
                Patrol();
            if (myState == AIState.Chase && canMove)
                Chase();
            if (myState == AIState.Search && canMove)
                Search();

            if (Vector3.Distance(transform.position, target.transform.position) < attackRange && canAttack)
            {
                Attack();
            }
        }        

    }




    void Patrol()
    {
        if (canSeePlayer)
        {
            audioManager.Play("Creature Noise");
            //Debug.Log("Play Creature Noise!");
            agent.speed = chaseSpeed;
            myState = AIState.Chase;
            return;
        }


        // do not try to patrol at all, if we have no PatrolPoints
        if (myPatrolPoints.Count < 1)
            return;


        //Debug.Log("I'm Patrolling.");

        // set up forst patrol point on start up
        if (myPatrolPointIndex == -1)
        {
            myPatrolPointIndex = 0;
            MoveToNewPatrolPoint();
            //Debug.Log("Agent: Set to go to first patrol point");
        }


        //Debug.Log("Distance Remaining: " + Vector3.Distance(transform.position, myPatrolPoints[myPatrolPointIndex].transform.position));

        // check if agent has reached patrol point
        if (Vector3.Distance(transform.position, myPatrolPoints[myPatrolPointIndex].transform.position) < 1f)
        {
            // increment patrol point (wrap back to first patrol point)
            myPatrolPointIndex++;
            if (myPatrolPointIndex >= myPatrolPoints.Count)
                myPatrolPointIndex = 0;

            MoveToNewPatrolPoint();
            //Debug.Log("Agent: Set to go to next waypoint.");
        }


    }


    void MoveToNewPatrolPoint()
    {
        // send agent to patrol point
        SetDestination(myPatrolPoints[myPatrolPointIndex].transform.position, 0);
    }



    void Chase()
    {

        // move towards player
        SetDestination(target.transform.position, attackRange);

        // if can't see player for a few seconds, go to search state
        if (!canSeePlayer)
        {
            
            lostPlayerTimer += Time.deltaTime;
            // if timer reaches limit, go to Search
            if (lostPlayerTimer > lostPlayerGiveUpDuration)
            {
                agent.speed = patrolSpeed;
                // go to search state
                myState = AIState.Search;
                //reset timer
                lostPlayerTimer = 0f;
            }

        }

        // reset the timer if we can see the player
        if (canSeePlayer)
            lostPlayerTimer = 0f;

    }

    private void Attack()
    {
        anim.SetTrigger("isAttacking");
        Invoke("ResetAttack", attackResetTime);
        canAttack = false;
        canMove = false; 
    }


    public void ActivateDamageCollider()
    {
        attackbox.enabled = true;
    }
    public void DeactivateDamageCollider()
    {
        attackbox.enabled = false;
        canMove = true;

    }

    private void ResetAttack()
    {
        canAttack = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // MUZ HACKS
            // 
            AttributesManager playerAM = other.GetComponent<AttributesManager>();

            playerAM.TakeDamage(attributesManager.attack, playerAM);
        }
    }

    void Search()
    {
        //Debug.Log("I'm Searching.");

        // spin on the spot, looking for player
        transform.Rotate(new Vector3(0, rotateSpeed * Time.deltaTime, 0));


        // if detect player, chase
        if (canSeePlayer)
        {
            agent.speed = chaseSpeed;
            audioManager.Play("Creature Noise");
            Debug.Log("Play Creature Noise!");
            myState = AIState.Chase;
            return;
        }

        // run search timer, and give up after a while, and return to Patrol
        if (!canSeePlayer)
        {
            searchTimer += Time.deltaTime;
            // if timer reaches limit, go back to Patrol
            if (searchTimer > searchDuration)
            {
                agent.speed = patrolSpeed;
                // go to patrol state
                myState = AIState.Patrol;
                MoveToNewPatrolPoint();
                //reset timer
                searchTimer = 0f;
            }
        }

        // reset the timer if we can see the player
        if (canSeePlayer)
            searchTimer = 0f;


    }





    void ProcessPerception()
    {
        // check if we can see the player

        if (agent.isOnNavMesh && agent.isActiveAndEnabled)
        {
            if (isAlive)
            {
                // draw a Linecast from the enemy to the player
                if (Physics.Linecast(eyePosition.transform.position, target.transform.position + transform.up * 1f, out hit))
                {
                    
                    // if the name of the Collider on the player (target) == the name of the Collider the raycast hit first...
                    if (hit.collider.name == "PlayerArmature")
                    {
                        
                        // if player is in my FieldOfView & within Perception Distance
                        if (Vector3.Angle(target.transform.position - eyePosition.transform.position, transform.forward) <= fieldOfView / 2 && Vector3.Distance(transform.position, target.transform.position) < perceptionDistance)
                        {
                           
                            // draw GREEN line
                            Debug.DrawLine(eyePosition.transform.position, target.transform.position + transform.up * 1f, Color.green, debugLineDuration);

                            canSeePlayer = true;
                            //lastSightedPos = target.transform.position - (Vector3.up * 1.9f);
                        }
                        else
                        {
                            // draw BLUE line  (player is outside fieldOfView)
                            Debug.DrawLine(eyePosition.transform.position, target.transform.position + transform.up * 1f, Color.blue, debugLineDuration);
                            canSeePlayer = false;
                        }
                    }
                    else
                    {
                        // draw RED line  (player is blocked by something)
                        Debug.DrawLine(eyePosition.transform.position, target.transform.position + transform.up * 1f, Color.red, debugLineDuration);
                        canSeePlayer = false;
                        //Debug.Log("Agent: Line of sight to player blocked by: " + hit.collider.name);
                    }
                }
            }
        }
    }





    public void SetDestination(Vector3 destination, float stoppingDistance)
    // My own custom way of giving an NPC a new destination (as well as being able to pass in a stoppingDistance (-1 for no-change))
    {

        NavMeshPath path = new NavMeshPath();

        NavMeshHit hit;

        // Use NavMesh.SamplePosition to find a valid position on the NavMeshFF
        if (NavMesh.SamplePosition(destination, out hit, 2.0f, NavMesh.AllAreas))
        {
            // if we find a point on the NavMesh, calculate the path
            if (agent.CalculatePath(hit.position, path))
            {
                // set destination
                agent.destination = hit.position;

                // set stopping distance                                 
                if (stoppingDistance != -1)  // pass in -1 for "don't change the value, leave it at whatever it was already".
                    agent.stoppingDistance = stoppingDistance;

                agent.isStopped = false;
            }
            else
            {
                // Log a warning if the agent cannot reach the destination
                Debug.LogWarning("SET DEST:  Agent cannot reach the destination!");
            }
        }
        else
        {
            // Log a warning if a valid position could not be found on the NavMesh
            Debug.LogWarning("SET DEST:  Unable to find a valid position on the NavMesh.");
        }
    }
}