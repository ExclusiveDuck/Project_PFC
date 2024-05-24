using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttributes : MonoBehaviour
{
    public AttributesManager playerAM;

    public AttributesManager enemyAM;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag ==("Enemy"))
        {
            Debug.Log("HitReg");
            enemyAM.GetComponent<AttributesManager>().TakeDamage(playerAM.attack);
        }
    }
}
