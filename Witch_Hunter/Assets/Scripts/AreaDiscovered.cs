using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class AreaDiscovered : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasgroup;

    public Image image;
    public float transparency;

    private bool fadein = false;
    private bool fadeout = false;
    private float alphaValue;
    public float fadeAwayPerSecond;

    public float TimeToFade;
    public TextMeshProUGUI fadeAway;
    public float fadeTime;
    public GameObject titleTrigger;
    public AudioManager audioManager;
    

    
    
    
    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        titleTrigger.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            audioManager.Play("Area Discovered");
            
            //Debug.Log("AreaDisc");

            //turn on image
            titleTrigger.SetActive(true);

            // prepare the image to fade in
            transparency = 0;
            Color tempColor = image.color;
            image.color = new Color(image.color.r, image.color.g, image.color.b, transparency);
            
            fadein = true;

            StartCoroutine("WaitForSec");
        }
    }

    IEnumerator WaitForSec()
    {
        yield return new WaitForSeconds(10f);
        Destroy(titleTrigger);
        Destroy(gameObject);
    }

    void Update()
    {
        HandleFading();
    }


    public void HandleFading()
    {
        if (fadein)
        {
            transparency += 1f * Time.deltaTime;
            image.color = new Color(image.color.r, image.color.g, image.color.b, transparency);
            //Debug.Log("FADEIN Transparency: " + transparency);

            if (transparency > 2f)
            {
                fadein = false;
                fadeout = true;
            }
        }

        if (fadeout)
        {
            transparency -= 1f * Time.deltaTime;
            image.color = new Color(image.color.r, image.color.g, image.color.b, transparency);
            //Debug.Log("FADE OUT Transparency: " + transparency);

            if (transparency <= 0f)
            {
                fadeout = false;
            }
        }

    }



}
