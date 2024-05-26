using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class NPCSystem : MonoBehaviour
{
    //public GameObject d_template;

//bool player_detection = false;

    public PlayerInput playerInput;
    InputAction interactAction;

    public int indexNumber;
    public TextMeshProUGUI textUI;
    public GameObject dialogueCanvas;
    public float detectionRange;
    bool canPress;

    private void Awake()
    {
        interactAction = playerInput.actions["Interact"];
        interactAction.ReadValue<float>();
        canPress = true;
    }

    // Update is called once per frame
    void Update()
    {
       // if(player_detection && interactAction.IsPressed() && !PlayerController.dialogue)
       // {
       //     canva.SetActive(true);
       //     PlayerController.dialogue = true;
       //
       //     canva.transform.GetChild(1).gameObject.SetActive(true);
       // }

        if (indexNumber == 0)
        {
            NewDialogue("Hello traveller!");
        }
        if (indexNumber == 1)
        {
            NewDialogue("thank the light you are here!");
        }
        if (indexNumber == 2)
        {
            NewDialogue("The dead seem to have risen again!");
        }
        if (indexNumber == 3)
        {
            NewDialogue("I barely escaped the cemetary with my life!");
        }
        if (indexNumber == 4)
        {
            NewDialogue("You look well versed in the art of combat");
        }
        if (indexNumber == 5)
        {
            NewDialogue("If you can make sure the dead um... stay dead, I can reward you!");
        }

        Collider[] detectionCollider = Physics.OverlapSphere(transform.position, detectionRange);
        foreach (Collider c in detectionCollider)
        {
            if (c.gameObject.tag == ("Player"))
            {
                dialogueCanvas.SetActive(true);

                if (interactAction.IsPressed() && canPress)
                {

                    canPress = false;
                    Invoke("ResetPress", 0.25f);
                    indexNumber++;
                }
            }

        }

        if (indexNumber >= 6)
        {
            dialogueCanvas.SetActive(false);
        }
    }

    private void ResetPress()
    {
        canPress = true;
    }

    void NewDialogue(string text)
    {
        //GameObject template_clone = Instantiate(d_template, d_template.transform);
        //template_clone.transform.parent = canva.transform;
        //template_clone.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = text;
        textUI.text = text;

    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.name == "PlayerArmature")
    //    {
    //        //player_detection = true;
    //
    //    }
    //}
    //
    //private void OnTriggerStay(Collider other)
    //{
    //
    //}
    //
    //private void OnTriggerExit(Collider other)
    //{
    //    dialogueCanvas.SetActive(false);
    //    //player_detection = false;
    //}

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(transform.position, detectionRange);
    //}
}
