using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributesManager : MonoBehaviour
{
    public int health;
    public int attack;
    public float critDamage = 0.5f;
    public float critChance = 0.5f;

    public void TakeDamage(int amount)
    {
        health -= amount;
        Vector3 randomness = new Vector3(Random.Range(0f, 0.25f), Random.Range(0f, 0.2f), Random.Range(0f, 0.2f));
        DamagePopUpGenerator.current.CreatePopUp(transform.position, amount.ToString(), Color.yellow);
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
}
