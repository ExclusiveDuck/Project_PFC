using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class AreaDiscovered : MonoBehaviour
{
    public AudioManager audioManager;


    // private float alphaValue;
    // private float fadeAwayPerSecond;
    // public TextMeshProUGUI fadeAway;
    // 
    // public float fadeTime;

    public GameObject titleTrigger;
    void Start()
    {
        // get reference to the audio manager
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        titleTrigger.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            audioManager.Play("Area Discovered");
            Debug.Log("Play Area Discovered");
            Debug.Log("AreadDisc");
            titleTrigger.SetActive(true);
            StartCoroutine("WaitForSec");
        }
    }

    IEnumerator WaitForSec()
    {
        yield return new WaitForSeconds(5);
        Destroy(titleTrigger);
        Destroy(gameObject);
    }

    void Fade()
    {

    }























    //private void Start()
    //{
    //    //fadeAway = GetComponent<TextMeshProUGUI>();
    //    //fadeAwayPerSecond = 1 / fadeTime;
    //    //alphaValue = fadeAway.color.a;
    //}
    void Update()
    {
       //if (fadeTime > 0)
       //{
       //    fadeTime -= Time.deltaTime;
       //    alphaValue -= fadeAwayPerSecond * Time.deltaTime;
       //    fadeAway.color = new Color(fadeAway.color.r, fadeAway.color.g, fadeAway.color.b, alphaValue);
       //}

    }
}
