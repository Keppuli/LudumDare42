using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {

    private Rigidbody2D rb;
    private Animator anim;
    public Canvas canvasObject;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if (!GameManager.instance.dockingTime)
        {
            rb.AddForce(new Vector2(-2f, 0f), ForceMode2D.Impulse);
            anim.enabled = false;
        }
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.name == "DebrisCleaner")
        {
            Destroy(gameObject);
            //Debug.Log(transform.name+" went out screen and was destroyed.");
        }
        if (col.gameObject.name == "MouseController")
        {
            MouseController.instance.SetCursor(1);
            if (Input.GetMouseButtonUp(0))
            {
                canvasObject.gameObject.SetActive(true);
            }
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.name == "MouseController")
        {
            MouseController.instance.SetCursor(0);
        }
    }
}
