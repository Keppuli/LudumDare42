using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartNoTutorial : MonoBehaviour {
    public static RestartNoTutorial instance = null; // Reference to instance which allows it to be accessed by any other script
    public GameObject tutorialStuff;
    public bool skipTutorial;
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
    }
    private void Update()
    {
        if (tutorialStuff == null)
            tutorialStuff = GameObject.Find("TUTORIALSTUFF"); // Automatically reset reference after restart
    }
    public void SkipTutorial()
    {
        Destroy(tutorialStuff);
        Dock.instance.tutorialDockingDone = true;
        GameManager.instance.dockingTime = false;
        Dock.instance.visible = false;
        DebrisSpawner.instance.isEnabled = true;
        Time.timeScale = 1;
        skipTutorial = false;
        Debug.Log("Tutorial skipped");
    }

}
