using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttributes : MonoBehaviour
{
    public AttributesManager playerAM;

    public AttributesManager enemyAM;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Sword hit object named: " + other.gameObject.name);

        if (other.gameObject.tag == "Enemy")
        {    
            Debug.Log("Weapon hit enemy.");
            enemyAM = other.gameObject.GetComponent<AttributesManager>();

            //Debug.Log("HitReg");
            enemyAM.TakeDamage(playerAM.attack, enemyAM);
        }
    }
}
