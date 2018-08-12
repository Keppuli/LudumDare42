using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public static Player instance = null; // Reference to instance which allows it to be accessed by any other script
    public float moveSpeed = 10f;
    public GameObject audioManager;
    Rigidbody2D rb2d;
    public bool playerOnTheHatch = false;
    AudioSource audioS1;
    AudioSource audioS2;
    public GameObject exhaust;
    public float exhaustSpawnTime;
    public float exhaustSpawnTimer;

    private void Awake()
    {
        // Create static instance of this manager and control singleton pattern
        if (instance == null)
            instance = this;
        // This enforces our singleton pattern, meaning there can only ever be one instance of a this manager
        else if (instance != this)
            Destroy(gameObject);
        //Sets this to not be destroyed when reloading scene
    }
    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        AudioSource[] aSources = GetComponents<AudioSource>();
        audioS1 = aSources[0];
        audioS2 = aSources[1];
        audioS2.Pause();
        exhaustSpawnTimer = exhaustSpawnTime;
    }

    // Update is called once per frame
    void Update () {
        if (playerOnTheHatch && Input.GetKeyDown(KeyCode.Space) && GameManager.instance.playerWaitTimer <= 0f)
        {
            Debug.Log("Player entered truck");
            AudioManager.instance.PlaySound("hatchSound");
            Truck.instance.active = true;
            GameManager.instance.ResetPlayerWaitTimer();
            Destroy(gameObject);
        }
        exhaustSpawnTimer -= Time.deltaTime;
    }
    void FixedUpdate()
    {
        rb2d.AddForce(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * moveSpeed);
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            audioS2.UnPause();
            if (exhaustSpawnTimer <= 0)
            {
                exhaustSpawnTimer = exhaustSpawnTime;
                Instantiate(exhaust, transform.position, Quaternion.identity);
            }
        }
        else
            audioS2.Pause();
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "TruckHatch")
        {
            playerOnTheHatch = true;
        }
        if (col.gameObject.CompareTag("PlayerBurner"))
        {
            Destroy(gameObject);
            GameManager.instance.gameOver = true;
            GameManager.instance.gameOverReason.text = "YOU BURNED IN THE ATMOSPHERE!";
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.name == "TruckHatch")
        {
            playerOnTheHatch = false;
        }
    }


    private void OnDestroy()
    {
        Debug.Log("Player destroyed.");
    }
}
