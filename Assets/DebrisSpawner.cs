using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisSpawner : MonoBehaviour {

    public static DebrisSpawner instance = null; // Reference to instance which allows it to be accessed by any other script

    public float spawnInterval = 10f;
    float spawnIntervalTimer;
    public float debrisSpeed = -3f; // Negative for left
    public float torque = 0.01f; // Rotating speed
    public bool isEnabled = true;
    public GameObject[] debrises;
    private void Awake()
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
        spawnIntervalTimer = spawnInterval;
    }

    // Update is called once per frame
    void Update () {
        if (isEnabled)
        {
            spawnIntervalTimer -= Time.deltaTime;
            if (spawnIntervalTimer <= 0f)
            {
                SpawnDebris();
                spawnIntervalTimer = spawnInterval;
            }
        }
    }

    void SpawnDebris() // https://forum.unity.com/threads/random-item-spawn-using-array-with-item-rarity-variable.176234/
    {
        int debrisType;
        float bonusSpeed = 0f;
        while (true)
        {
            float bonusChance = 0;
            int selectOne = Random.Range(0, debrises.Length); // Select random debris, all equal here
            GameObject selected = debrises[selectOne].gameObject;
            if (selected.GetComponent<Debris>().isMine)
            {
                bonusChance = GameManager.instance.dayCount * 10; // Bonus chance for mines to spawn increase every day
                bonusSpeed = debrisSpeed*2f;
            }
            float baseChance = selected.GetComponent<Debris>().chanceToSpawn; // Check debris specific spawn chance
            float chance = baseChance + bonusChance;
            int probapility = Random.Range(0, 100); // Random probability
            if (chance > probapility) // If debris specific spawn chance is greater than random prob, spawn it
            {
                debrisType = selectOne;
                Debug.Log("Selected to spawn: " + selected.name + "with chance of: " + chance);
                break; // Break out of loop
            }
        }

        float spawnYPosition = Random.Range(-4.0f, 4.0f);// Y-range = 4 to -4
        var debris = Instantiate(debrises[debrisType], new Vector3(transform.position.x, spawnYPosition,0f),Quaternion.identity);
        debris.GetComponent<Rigidbody2D>().AddForce(new Vector2(debrisSpeed+ bonusSpeed, 0f), ForceMode2D.Impulse);
        float ntorque = Random.Range(-torque, torque);
        debris.GetComponent<Rigidbody2D>().AddTorque(ntorque * debris.GetComponent<Rigidbody2D>().mass); // rotating speed according to mass
        //Debug.Log("Spawned: " + debris +" in Y coordinate: "+ spawnYPosition);
    }
}
