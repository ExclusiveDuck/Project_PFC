using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Audio;

public class PlayerController : MonoBehaviour
{
    //Third person Controller reference
    [SerializeField]
    private Animator playerAnim;
    PlayerInput playerInput;
    InputAction meleeAttack;
    InputAction equipAction;
    InputAction blockAction;

    //Equip-Unequip parameters
    [SerializeField]
    private GameObject sword;
    [SerializeField]
    private GameObject swordOnShoulder;
    public bool isEquipping;
    public bool isEquipped;

    //Audio
    public AudioManager audioManager;

    //Blocking Parameters
    public bool isBlocking;

    //Kick Parameters
    public bool isKicking;

    //Attack Parameters
    public bool isAttacking;
    private float timeSinceAttack;
    public int currentAttack = 0;

    //Sword collider variable
    public BoxCollider swordCollider;

    //CUSTOM NPC INTERACTABLE DIALOGUE
    static public bool dialogue = false;


    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        meleeAttack = playerInput.actions["MeleeAttack"];
        meleeAttack.ReadValue<float>();
        equipAction = playerInput.actions["Equip"];
        equipAction.ReadValue<float>();
        blockAction = playerInput.actions["Block"];
        blockAction.ReadValue<float>();

        // get reference to the audio manager
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

    }

    private void Update()
    {
        timeSinceAttack += Time.deltaTime;

        Attack();

        Equip();
        Block();
        Kick();
    }

   
    public void ActivateSwordCollider()
    {
        //Debug.Log("ActivateSwordCollider");
        swordCollider.enabled = true;
    }

    public void DeactivateSwordCollider()
    {
        //Debug.Log("DeactivateSwordCollider");
        swordCollider.enabled = false;
    }


    private void Equip()
    {
        
        if (equipAction.WasPerformedThisFrame() && playerAnim.GetBool("Grounded"))
        {
            isEquipping = true;
            playerAnim.SetTrigger("Equip");
            audioManager.Play("DrawSword");
            Debug.Log("Play SWORD EQUIP sound!");
        }
    }
    
    public void ActiveWeapon()
    {
        if (!isEquipped)
        {
            sword.SetActive(true);
            swordOnShoulder.SetActive(false);
            isEquipped = !isEquipped;
        }
        else
        {
            sword.SetActive(false);
            swordOnShoulder.SetActive(true);
            isEquipped = !isEquipped;
        }
    }



    public void Equipped()
    {
        isEquipping = false;
    }

    private void Block()
    {
        if (blockAction.IsPressed() && playerAnim.GetBool("Grounded"))
        {
            playerAnim.SetBool("Block", true);
            isBlocking = true;
        }

        else
        {
            playerAnim.SetBool("Block", false);
            isBlocking = false;
        }
    }

    public void Kick()
    {
        if (Input.GetKey(KeyCode.LeftControl) && playerAnim.GetBool("Grounded"))
        {
            playerAnim.SetBool("Kick", true);
            isKicking = true;
        }
        else
        {
            playerAnim.SetBool("Kick", false);
            isKicking = false;
        }
    }

    private void Attack()
    {

        if (meleeAttack.IsPressed() && playerAnim.GetBool("Grounded") && timeSinceAttack > 0.8f)
        {
            audioManager.Play("Attack 1");
            Debug.Log("Play Attack 1 sound!");

            if (!isEquipped)
                return;

            currentAttack++;
            isAttacking = true;

            if (currentAttack > 3)
                currentAttack = 1;

            //Reset
            if (timeSinceAttack > 1f)
                currentAttack = 1;

            //Call Attack Triggers
            playerAnim.SetTrigger("Attack" + currentAttack);

            //Reset Timer
            timeSinceAttack = 0;
        }
    }



        
    public void ResetAttack()
    {
        isAttacking = false;
    }

}
