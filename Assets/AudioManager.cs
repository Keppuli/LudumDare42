using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager instance = null; // Reference to instance which allows it to be accessed by any other script

    public AudioClip dockingSound;
    public AudioClip undockingSound;
    public AudioClip hatchSound;
    public AudioClip doorSound;
    public AudioClip fixSound;

    public AudioClip[] suckedSound;

    AudioSource audioS;
    AudioSource audioS2;

    void Awake()
    {
        // Create static instance of this manager and control singleton pattern
        if (instance == null)
            instance = this;
        // This enforces our singleton pattern, meaning there can only ever be one instance of a this manager
        else if (instance != this)
            Destroy(gameObject);
        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        AudioSource[] aSources = GetComponents<AudioSource>();
        audioS = aSources[0];
        audioS2 = aSources[1];
    }
    public void PlaySound(string soundName)
    {
        if (soundName == "dockingSound")
            audioS.PlayOneShot(dockingSound);
        else if (soundName == "undockingSound")
            audioS.PlayOneShot(undockingSound);
        else if (soundName == "hatchSound")
            audioS.PlayOneShot(hatchSound);
        else if (soundName == "doorSound")
            audioS.PlayOneShot(doorSound);
        else if (soundName == "fixSound")
            audioS.PlayOneShot(doorSound);
        else if (soundName == "suckedSound")
        {
            int tmp = Random.Range(0,suckedSound.Length);
            audioS.PlayOneShot(suckedSound[tmp],0.5f);
        }
        Debug.Log("AM Played sound: " + soundName);
    }
}
