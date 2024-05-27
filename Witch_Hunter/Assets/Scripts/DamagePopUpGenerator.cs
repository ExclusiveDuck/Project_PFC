using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DamagePopUpGenerator : MonoBehaviour
{
    public static DamagePopUpGenerator current;
    public GameObject prefab;

    // Update is called once per frame
    private void Awake()
    {
        current = this;
    }

   

    public void CreatePopUp(Vector3 pos, string text, Color color)
    {
        //Debug.Log("POPUP!");    // YES

        /*
        // if position given for popup is Vector3.zero, then we know this is an ememy health popup
        if (pos == Vector3.zero)
        {
            Debug.Log("Vector3.Zero, therefore:  Player hit ENEMY - do damage popup.");
            // set position to this script
            pos = ;
        }
        else
        {
            Debug.Log("Enemy hit PLAYER - do damage popup.");   // YES
        }
        */

        var popup = Instantiate(prefab, pos, Quaternion.identity);
        var temp = popup.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        temp.text = text;
        temp.faceColor = color;

        //Destroy Timer
        Destroy(popup, 1f);
    }
}
