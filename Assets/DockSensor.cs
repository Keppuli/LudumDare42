using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockSensor : MonoBehaviour {

    public bool docked = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "Truck")
        {
            AudioManager.instance.PlaySound("dockingSound");
            docked = true;
        }

    }
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.name == "Truck")
        {
            docked = true;
        }

    }
    void OnTriggerExit2D(Collider2D col)
    {

        if (col.gameObject.name == "Truck")
        {
            AudioManager.instance.PlaySound("undockingSound");

            docked = false;
        }
    }
}
