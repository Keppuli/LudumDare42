using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour {

    public string description;
    public int value;
    public float startForce;
    public bool isMine;
    public float chanceToSpawn;
    public SpaceMine mineScript;
    public bool inTrunk;
    void Start()
    {
        // Start with random rotation
        transform.Rotate(Vector3.forward, Random.Range(-45, 45));
        if (GetComponent<SpaceMine>())
            mineScript = GetComponent<SpaceMine>();
    }

    void OnBecameVisible()
    {
        if (mineScript != null && GetComponent<SpriteRenderer>().isVisible)
            mineScript.enabled = true;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Trunk")) {
            inTrunk = true;
        }

        if (col.gameObject.CompareTag("DebrisCleaner") && (MouseController.instance.tractedObject != gameObject) && !inTrunk)
        {
            Destroy(gameObject);
        }
        else if (col.gameObject.CompareTag("DebrisCleanerScore") && MouseController.instance.tractedObject != gameObject)
        {
            Destroy(gameObject);
            GameManager.instance.debrisEliminated += 1;
        }

        else if (col.gameObject.name == "DockDebrisCleaner")
        {
            AudioManager.instance.PlaySound("suckedSound");
            Debug.Log(transform.name+" sucked by dock.");
            GameManager.instance.money += value;
            GameManager.instance.debrisEliminated += 1;
            Destroy(gameObject);
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Trunk"))
        {
            inTrunk = false;

        }
    }
}
