using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NextDialogue : MonoBehaviour
{
    public PlayerInput playerInput;
    InputAction interactAction;
    int index = 2;

    private void Awake()
    {
        interactAction = playerInput.actions["Interact"];
        interactAction.ReadValue<float>();

    }

    private void Update()
    {
        if (interactAction.IsPressed() && transform.childCount > 1)
        {
            if (PlayerController.dialogue)
            {
                transform.GetChild(index).gameObject.SetActive(true);
                index += 1;
                if (transform.childCount == index)
                {
                    index = 2;
                    PlayerController.dialogue = false;
                }
            }
            else
            {
                gameObject.SetActive(false);
            }

        }

    }
}
