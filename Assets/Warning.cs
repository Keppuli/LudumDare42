using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Warning : MonoBehaviour {
    AudioSource audioS;
    public Text text;
    private void Start()
    {
        audioS = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Truck"))
        {
            audioS.Play();
            text.color = new Color(255,0,0,255);
        }
       
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Truck"))
        {
            audioS.Stop();
            text.color = new Color(255, 255, 255, 215);

        }
    }
}
