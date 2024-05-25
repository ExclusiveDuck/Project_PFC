using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCSystem : MonoBehaviour
{
    public GameObject d_template;
    public GameObject canva;
    bool player_detection = false;

    // Update is called once per frame
    void Update()
    {
        if(player_detection && Input.GetKeyDown(KeyCode.E))
        {
            NewDialogue("Hello traveller!");
            NewDialogue("thank the light you are here!");
            NewDialogue("The dead seem to have risen again!");
            NewDialogue("I barely escaped the cemetary with my life!");
            NewDialogue("You look well versed in the art of combat");
            NewDialogue("If you can make sure the dead um... stay dead, I can reward you!");
            canva.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    void NewDialogue(string text)
    {
        GameObject template_clone = Instantiate(d_template, d_template.transform);
        template_clone.transform.parent = canva.transform;
        template_clone.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = text;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "PlayerArmature")
        {
            player_detection = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        player_detection = false;
    }
}
