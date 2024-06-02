using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.SceneManagement;

public class AttributesManager : MonoBehaviour
{
    //Materials
    public Material playerHit;          
    public Material enemyHit;
    public Material playerOriginalMaterial;
    public Material enemyOriginalMaterial;
    public SkinnedMeshRenderer meshRend;

    public float enemyKillCounter;
    public float shakeIntensity;
    public float shakeFrequency;
    ///private CinemachineShake cinShake;
    //Player Health
    public HealthBar healthBar;
    public int maxHealth = 100;
    public int currentHealth;
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
    [HideInInspector]public AudioManager audioManager;

    // Stop "double hit" happening
    public bool isInvincible = false;
    public float invincibleTimer = 0f;
    public float invincibleTimerDuration = 1.5f;

    private AttributesManager playerAB;

    
    private void Start()
    {
        playerAB = GameObject.Find("PlayerArmature").GetComponent<AttributesManager>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        // get reference to the audio manager
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        if (isPlayer)
        {
            playerOriginalMaterial = meshRend.material;
        }
        if (isEnemy)
        {
            enemyOriginalMaterial = meshRend.material;
        }
       
    }

    


    public void Update()
    {
        if (currentHealth <= 0 && isEnemyDead == false || currentHealth <= 0 && isPlayerDead == false)
        {

            if (isPlayer && isPlayerDead == false)
            {
                Die();
            }
            if (isEnemy && isEnemyDead == false)
            {
                playerAB.enemyKillCounter++;
                anim.SetTrigger("isDead");
                anim.SetBool("NotAlive", true);
                isEnemyDead = true;
                anim.SetTrigger("isDead");
                GetComponent<NavMeshAgent>().speed = 0;
                GetComponent<NPCBehaviour>().rotateSpeed = 0;
                audioManager.Play("Skeleton Death");
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
            //Debug.Log("The hit enemy is on invincibility timeout... skip damage!");
            return;
        }
        if (isPlayer && pController.isBlocking == true)
        {
            //Debug.Log("hit while blocking");
            audioManager.Play("Shield Hit");
            CinemachineShake.Instance.ShakeCamera(shakeIntensity, shakeFrequency);
        }
        if (isPlayer && pController.isBlocking == false)
        {
            audioManager.Play("Player Hit");
            CinemachineShake.Instance.ShakeCamera(shakeIntensity, shakeFrequency);
            currentHealth -= amount;

            healthBar.SetHealth(currentHealth);
            meshRend.material = playerHit;
            Invoke("DeactivateHitFlash", 0.15f);
            DamagePopUpGenerator.current.CreatePopUp(transform.position, amount.ToString(), Color.red);
            targetAM.isInvincible = true;
        }
        if (isEnemy)
        {
            //Debug.Log("Player HIT ENEMY - TAKE DAMAGE" + "        TargetAM position = " + targetAM.transform.position);
            currentHealth -= amount;



            DamagePopUpGenerator.current.CreatePopUp(new Vector3(targetAM.transform.position.x, 1f, targetAM.transform.position.z), amount.ToString(), Color.yellow);
            targetAM.isInvincible = true;
            meshRend.material = enemyHit;
            Invoke("DeactivateHitFlash", 0.15f);
        }


    }

    private void Die()
    {
        isPlayerDead = true;
        
        Invoke("ResetPlayer", 0.5f);
    }
    public void ResetPlayer()
    {
        Debug.Log("Reset");
        currentHealth = 100;
        string sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
        isPlayerDead = false;
    }

    public void DeactivateHitFlash()
    {
        if (isPlayer)
        {
            meshRend.material = playerOriginalMaterial;
        }

        if (isEnemy)
        {
            meshRend.material = enemyOriginalMaterial;
        }
    }
}
