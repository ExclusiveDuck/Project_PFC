using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttributesManager : MonoBehaviour
{
    public int health;
    public int attack;
    public float critDamage = 0.5f;
    public float critChance = 0.5f;
    public Animator anim;
    public bool isEnemyDead;
    public bool isPlayerDead;
    public bool isPlayer;
    public bool isEnemy;
    public Transform resetPosition;
    public PlayerController pController;
    public AudioManager audioManager;

    // Stop "double hit" happening
    public bool isInvincible = false;
    public float invincibleTimer = 0f;
    public float invincibleTimerDuration = 1.5f;


    private void Start()
    {
        // get reference to the audio manager
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }
    public void Update()
    {
        if (health <= 0 && isEnemyDead == false || health <= 0 && isPlayerDead == false)
        {

            if (isPlayer && isPlayerDead == false)
            {
                

                isPlayerDead = true;
                audioManager.Play("Player Death");
                //Stop Moving
                Invoke("ResetPlayer", 0.5f);
            }
            if (isEnemy && isEnemyDead == false)
            {
                
                anim.SetTrigger("isDead");
                isEnemyDead = true;
                anim.SetTrigger("isDead");
                GetComponent<NavMeshAgent>().speed = 0;
                GetComponent<NPCBehaviour>().rotateSpeed = 0;
                Destroy(gameObject, 3);
            }

        }
        else // is alive
        {
            UpdateInvincibleTimer();
        }
    }


    void UpdateInvincibleTimer()
    {
        if (isInvincible)
        {
            // update timer
            invincibleTimer += Time.deltaTime;
            // if timer has elapsed, reset invincible (turn off)
            if (invincibleTimer > invincibleTimerDuration)
            {
                isInvincible = false;
                invincibleTimer = 0f;
            }
        }
    }


    public void TakeDamage(int amount, AttributesManager targetAM)  // pass in refernce to the AttributeManager on the thing that is taking damage
    {
        // MUZ HACKS

        // could add in a check here if the AttributeManager passed in isInvincle or not.  If they are, return

        if (targetAM.isInvincible)
        {
            Debug.Log("The hit enemy is on invincibility timeout... skip damage!");
            return;
        }

        if (isPlayer && pController.isBlocking == false)
        {
            //Debug.Log("Enemy HIT PLAYER - TAKE DAMAGE");   // YES
            health -= amount;
            //Vector3 randomness = new Vector3(Random.Range(0f, 0.25f), Random.Range(0f, 0.2f), Random.Range(0f, 0.2f));
            DamagePopUpGenerator.current.CreatePopUp(transform.position, amount.ToString(), Color.yellow);
            targetAM.isInvincible = true;
        }
        if (isEnemy)
        {
            //Debug.Log("Player HIT ENEMY - TAKE DAMAGE" + "        TargetAM position = " + targetAM.transform.position);
            health -= amount;
            //Vector3 randomness = new Vector3(Random.Range(0f, 0.25f), Random.Range(0f, 0.2f), Random.Range(0f, 0.2f));
            DamagePopUpGenerator.current.CreatePopUp(new Vector3(targetAM.transform.position.x, 1f, targetAM.transform.position.z), amount.ToString(), Color.yellow);
            targetAM.isInvincible = true;
        }


    }


    /*
    public void DealDamage(GameObject target)
    {
        float totalDamage = attack;

        // Count RNG Chance
        if(Random.Range(0f, 1f)< critChance)
        {
            totalDamage *= critDamage;
        }
        var atm = target.GetComponent<AttributesManager>();
        if(atm != null)
        {
            atm.TakeDamage(attack);
        }
    }
    */


    public void ResetPlayer()
    {
        health = 100;
        //transform.position = resetPosition.position;
        isPlayerDead = false;
    }
}
