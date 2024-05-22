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
