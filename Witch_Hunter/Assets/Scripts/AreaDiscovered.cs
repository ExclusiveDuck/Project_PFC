using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AreaDiscovered : MonoBehaviour
{
    private float alphaValue;
    private float fadeAwayPerSecond;
    private TextMeshProUGUI fadeAway;
    
    public float fadeTime;

    public GameObject TitleTrigger;
    void start()
    {
        TitleTrigger.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.tag == "Player")
        {
            TitleTrigger.SetActive(true);
            StartCoroutine("WaitForSec");
        }
    }

    IEnumerator WaitForSec()
    {
        yield return new WaitForSeconds(5);
        Destroy(TitleTrigger);
        Destroy(gameObject);
    }
    private void Start()
    {
        fadeAway = GetComponent<TextMeshProUGUI>();
        fadeAwayPerSecond = 1 / fadeTime;
        alphaValue = fadeAway.color.a;
    }
    void Update()
    {
        if (fadeTime > 0)
        {
            fadeTime -= Time.deltaTime;
            alphaValue -= fadeAwayPerSecond * Time.deltaTime;
            fadeAway.color = new Color(fadeAway.color.r, fadeAway.color.g, fadeAway.color.b, alphaValue);
        }

    }
}
