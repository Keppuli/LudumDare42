using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Truck : MonoBehaviour {

    AudioSource audioS;
    Rigidbody2D rb2d;
    SpriteRenderer sr;
    public float moveSpeed = 20f;
    public static Truck instance = null; // Reference to instance which allows it to be accessed by any other script
    public bool active = true;
    public bool doorOpen = false;
    public GameObject hatch;
    public GameObject door;
    public GameObject playerSpawn;
    public Sprite[] hatchSprites; // 0 closed 1 open
    public int health = 5;
    public Sprite[] healthSprites;
    public GameObject healthObj;
    public float immuneTimer = 1f;
    public AudioClip hatchSound;
    public AudioClip doorSound;
    public GameObject exhaust;
    public Transform exhaustSpot;
    public float exhaustSpawnTime;
    public float exhaustSpawnTimer;
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
    void Start () {
        audioS = GetComponent<AudioSource>();
        rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        exhaustSpawnTimer -= Time.deltaTime;
        if (exhaustSpawnTimer <= 0)
        {
            exhaustSpawnTimer = exhaustSpawnTime;
            Instantiate(exhaust, exhaustSpot.position, Quaternion.identity);
        }
        Health();

        if (immuneTimer > 0)
        {
            immuneTimer -= Time.deltaTime;
            float tmp = Random.Range(0f,1f);
            sr.color = new Color(1, tmp, tmp);
            door.GetComponent<SpriteRenderer>().color = new Color(1, tmp, tmp);
            hatch.GetComponent<SpriteRenderer>().color = new Color(1, tmp, tmp);
        }
        else
        {
            sr.color = new Color(1, 1, 1);
            door.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
            hatch.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);

        }

        if (active)
        {
            audioS.UnPause();
            hatch.GetComponent<SpriteRenderer>().sprite = hatchSprites[0];
            if (Input.GetKeyDown(KeyCode.E))
                ToggleDoor();
        }
        else
        {
            audioS.Pause();
            hatch.GetComponent<SpriteRenderer>().sprite = hatchSprites[1];
        }
    }

    void FixedUpdate()
    {
        if (active)
            rb2d.AddForce(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * moveSpeed);
    }
    void Health()
    {
        if (health == 5)
            healthObj.GetComponent<SpriteRenderer>().sprite = healthSprites[0];
        else if (health == 4)
            healthObj.GetComponent<SpriteRenderer>().sprite = healthSprites[1];
        else if (health == 3)
            healthObj.GetComponent<SpriteRenderer>().sprite = healthSprites[2];
        else if (health == 2)
            healthObj.GetComponent<SpriteRenderer>().sprite = healthSprites[3];
        else if (health == 1)
            healthObj.GetComponent<SpriteRenderer>().sprite = healthSprites[4];
        else if (health == 0)
        {
            GameManager.instance.gameOverReason.text = "YOU LOST YOUR TRUCK!";
            GameManager.instance.gameOver = true;
        }
    }
    public void TakeDamage()
    {
        if (immuneTimer <= 0)
        {
            health -= 1;
            immuneTimer = 1f; // Reset
        }
        
    }
    void ToggleDoor()
    {
        door.SetActive(!door.activeInHierarchy);
        AudioManager.instance.PlaySound("doorSound");
    }
    void OnBecameInvisible()
    {
        GameManager.instance.gameOver = true;
        GameManager.instance.gameOverReason.text = "YOUR TRUCK BURNED IN THE ATMOSPHERE!";
        Destroy(gameObject);
    }

    void LateUpdate () {
		if (active)
        {
            if (Input.GetKeyDown(KeyCode.Space) && GameManager.instance.playerWaitTimer <= 0f)
            {
                AudioManager.instance.PlaySound("hatchSound");
                Instantiate(GameManager.instance.playerInstance, playerSpawn.transform.position, Quaternion.identity);
                GameManager.instance.ResetPlayerWaitTimer();
                active = false;
                Debug.Log("Player exited truck");
            }
        }


    }
}
