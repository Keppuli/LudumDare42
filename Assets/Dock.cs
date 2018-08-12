using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dock : MonoBehaviour {

    AudioSource audioS;
    public static Dock instance = null; // Reference to instance which allows it to be accessed by any other script
    public GameObject dockColor;
    public GameObject dockSensorLeft;
    public GameObject dockSensorRight;
    public GameObject debrisCleaner;
    public GameObject suction;
    public bool visible = false;
    Vector2 startPosition;
    Vector2 endPosition;
    public float positionOffset = 2f;
    public bool tutorialDockingDone = false;


    void Awake()
    {
        // Create static instance of this manager and control singleton pattern
        if (instance == null)
            instance = this;
        // This enforces our singleton pattern, meaning there can only ever be one instance of a this manager
        else if (instance != this)
            Destroy(gameObject);
        //Sets this to not be destroyed when reloading scene
        //DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        audioS = GetComponent<AudioSource>();
        startPosition = transform.position;
        endPosition = new Vector2(startPosition.x, startPosition.y+positionOffset);
        audioS.Pause();
    }
    void Update () {

        if (dockSensorLeft.GetComponent<DockSensor>().docked && dockSensorRight.GetComponent<DockSensor>().docked)
        {
            dockColor.GetComponent<SpriteRenderer>().color = new Color(0,255,0);
            StartSuction();
            audioS.UnPause();

        }
        else
        {
            dockColor.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
            StopSuction();
            audioS.Pause();

        }
        if (visible && transform.position.y != endPosition.y)
            transform.position = Vector2.Lerp(transform.position, endPosition, Time.deltaTime);
        else if (!visible && transform.position.y != startPosition.y)
            transform.position = Vector2.Lerp(transform.position, startPosition, Time.deltaTime);

    }

    void StartSuction()
    {
        debrisCleaner.SetActive(true);
        suction.SetActive(true);
        Debug.Log("Docked. Starting suction.");
        Dock.instance.tutorialDockingDone = true;
    }
    void StopSuction()
    {
        debrisCleaner.SetActive(false);
        suction.SetActive(false);
        Debug.Log("Detached. Stopped suction.");
    }


}
