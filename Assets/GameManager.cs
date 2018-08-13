using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject bg;
    public GameObject playerInstance;
    public static GameManager instance = null; // Reference to instance which allows it to be accessed by any other script
    public float interactionDistance = 2f; // X Away from obj pivot point! Used by all NPC's to determite loot distance etc.
    public float playerWaitTime = 0.5f; // Wait time for changing in and outside from truck
    public float playerWaitTimer;
    public float tractorBeamStrength = 1f;
    public float dockSuctionstrength = 1f;
    public int dayCount;
    public float dayTime;
    public float dayTimer;
    public bool dockingTime = false;
    public int unpaidMoney;
    public int money;
    public int debrisEliminated;
    public int debrisRemaining;
    public int totalDebris;
    public bool gameOver;
    public int basefixCost;
    public int fixCost;
    public Text roundCount;
    public Text moneyCount;
    public Text debrisGoneCount;
    public Canvas gameOverCanvas;
    public Text gameOverReason;
    public GameObject paused;
    public Button fixButton;
    public Text earnedCash;
    public Text removedDebris;

    void Awake()
    {
        // Create static instance of this manager and control singleton pattern
        if (instance == null)
            instance = this;
        // This enforces our singleton pattern, meaning there can only ever be one instance of a this manager
        else if (instance != this)
            Destroy(gameObject);

    }
    private void Start()
    {
        ResetPlayerWaitTimer();
        ResetDayTimer();
        Time.timeScale = 1;
      
    }

    private void Update()
    {
        earnedCash.text = "$ "+money.ToString();
        removedDebris.text = debrisEliminated.ToString();
        if (GameObject.Find("TUTORIALSTUFF") && RestartNoTutorial.instance.skipTutorial)
            RestartNoTutorial.instance.SkipTutorial();
        fixCost = basefixCost * dayCount;
        debrisRemaining = totalDebris - debrisEliminated;

        RotateBG();

        if (gameOver)
        {
            gameOverCanvas.gameObject.SetActive(true);
            Time.timeScale = 0;
            if (Input.GetKeyDown(KeyCode.Return))
            {
                gameOver = false;
                gameOverCanvas.gameObject.SetActive(false);
                RestartNoTutorial.instance.skipTutorial = true;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        roundCount.text = dayCount.ToString();
        moneyCount.text = money.ToString();
        debrisGoneCount.text = debrisRemaining.ToString();

        if (Input.GetKeyDown(KeyCode.F4))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetKeyDown(KeyCode.P))
            TogglePause();
        // When its docking time these won't be run
        if (!dockingTime)
        {
            if (dayTimer > 0f)
                dayTimer -= Time.deltaTime;
            else if (dayTimer <= 0f)
                ResetDayTimer();
        }

        if (playerWaitTimer > 0f)
            playerWaitTimer -= Time.deltaTime;

    }
    public void ResetPlayerWaitTimer()
    {
        playerWaitTimer = playerWaitTime;
    }
    public void ResetDayTimer()
    {
        dayTimer = dayTime;
        dayCount += 1;
        bg.transform.Rotate(0f,0f,0f); // Reset to original position when full circle has been done
        dockingTime = true;
        Dock.instance.visible = true;
        DebrisSpawner.instance.isEnabled = false;
    }
    public void TogglePause()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            paused.SetActive(true);
        }

        else if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            paused.SetActive(false);
        }
    }
    void RotateBG()
    {
        bg.transform.Rotate(0f,0f, Time.deltaTime); //+ dayTime / 360 * Time.deltaTime
        if (transform.eulerAngles.z >= 360)
            bg.transform.Rotate(0f, 0f, 0f);
    }
    
}
