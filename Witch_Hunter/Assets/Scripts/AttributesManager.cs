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

    public void Update()
    {
        if (health <= 0 && isEnemyDead == false || health <= 0 && isPlayerDead == false)
        {

            if (isPlayer && isPlayerDead == false)
            {
                isPlayerDead = true;
                anim.SetTrigger("isDead");
                //Stop Moving
                Invoke("ResetPlayer", 0.5f);

            }
            if (isEnemy && isEnemyDead == false)
            {
                isEnemyDead = true;
                anim.SetTrigger("isDead");
                GetComponent<NavMeshAgent>().speed = 0;
                GetComponent<NPCBehaviour>().rotateSpeed = 0;
                Destroy(gameObject, 3);
            }

        }
    }

    public void TakeDamage(int amount)
    {
        if (isPlayer && pController.isBlocking == false)
        {
            health -= amount;
            Vector3 randomness = new Vector3(Random.Range(0f, 0.25f), Random.Range(0f, 0.2f), Random.Range(0f, 0.2f));
            DamagePopUpGenerator.current.CreatePopUp(transform.position, amount.ToString(), Color.yellow);
        }
        if (isEnemy)
        {
            health -= amount;
            Vector3 randomness = new Vector3(Random.Range(0f, 0.25f), Random.Range(0f, 0.2f), Random.Range(0f, 0.2f));
            DamagePopUpGenerator.current.CreatePopUp(transform.position, amount.ToString(), Color.yellow);
        }

       
    }
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

    public void ResetPlayer()
    {
        health = 100;
        //transform.position = resetPosition.position;
        isPlayerDead = false;
    }
}
