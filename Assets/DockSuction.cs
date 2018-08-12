using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockSuction : MonoBehaviour {

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Debris")
        {
            Debug.Log("Sucking debris.");
            col.gameObject.GetComponent<Rigidbody2D>().AddForce((transform.position - col.gameObject.transform.position) * GameManager.instance.dockSuctionstrength);
        }

    }
}
