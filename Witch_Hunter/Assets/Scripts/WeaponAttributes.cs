using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class WeaponAttributes : MonoBehaviour
{
    public float shakeIntensity;
    public float shakeFrequency;
    public AudioManager audioManager;

    public AttributesManager playerAM;

    public AttributesManager enemyAM;

    private void Start()
    {
        // get reference to the audio manager
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Sword hit object named: " + other.gameObject.name);

        if (other.gameObject.tag == "Enemy")
        {
            
            //Debug.Log("Weapon hit enemy.");
            enemyAM = other.gameObject.GetComponent<AttributesManager>();
            audioManager.Play("Enemy Hit");
            CinemachineShake.Instance.ShakeCamera(shakeIntensity, shakeFrequency);
            //Debug.Log("HitReg");
            enemyAM.TakeDamage(playerAM.attack, enemyAM);
            
        }
    }
}
