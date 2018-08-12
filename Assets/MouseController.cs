using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseController : MonoBehaviour {

    AudioSource audioS;
    public static MouseController instance = null; // Reference to instance which allows it to be accessed by any other script
    public CursorMode cursorMode = CursorMode.ForceSoftware;
    public Texture2D[] cursorTexture;
    public Vector2 cursorHotspot;
    public GameObject tractedObject;
    public GameObject tractorBeam;
    public Canvas canvasObject;
    public Text itemDescription;
    public Text itemPrice;
    public RawImage bgImage;

    void Awake()
    {
        // Create static instance of this manager and control singleton pattern
        if (instance == null)
            instance = this;
        // This enforces our singleton pattern, meaning there can only ever be one instance of a this manager
        else if (instance != this)
            Destroy(gameObject);
       
        cursorHotspot = new Vector2(cursorTexture[0].width / 2, cursorTexture[0].height / 2);
        SetCursor(0);
        //Cursor.visible = false;
        audioS = GetComponent<AudioSource>();
    }
    public void SetCursor(int style) // 0 blue,1 green
    {
        Cursor.SetCursor(cursorTexture[style], cursorHotspot, cursorMode);
    }

    void Update()
    {
        // Set object position to follow mouse position (NOT FOR CURSOR AS THEY AUTOMATICALLY DO SO!)
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var layerMask = (1 << 12);
        layerMask |= (1 << 10); // truck
        layerMask |= (1 << 13);
        layerMask |= (1 << 14);

        //RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero,100f, 1 << LayerMask.NameToLayer("Debris"));
        RaycastHit2D hit = Physics2D.CircleCast(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.2f, Vector2.zero, 100f, layerMask);

        if (hit.collider != null)
        {
            bgImage.color = new Color(.5f, .5f, .5f, .5f); // DEFAULT


            if (hit.collider.CompareTag("TrunkBlock"))
            {
                itemDescription.text = "Open hatch with:";
                itemPrice.text = "(space)";
                Debug.Log("TrunkBlock");

            }

            if (hit.collider.CompareTag("TruckDoor"))
            {
                Debug.Log("TruckDoor");
            }
            if (hit.collider.CompareTag("TruckHatch"))
            {
                Debug.Log("TruckHatch");
            }
            if (hit.collider.CompareTag("FixButton"))
            {

                if (DisplayMouseOverTextFix() && Input.GetMouseButtonDown(0))
                {
                    if (GameManager.instance.money > GameManager.instance.fixCost)
                    {
                        GameManager.instance.money -= GameManager.instance.fixCost;
                        Truck.instance.health += 1;
                        AudioManager.instance.PlaySound("fixSound");
                    }
                }
            }

            if (hit.collider.CompareTag("Debris"))
            {
                DisplayMouseOverText(hit.transform.gameObject);
                SetCursor(1);
                if (Input.GetMouseButtonDown(0))
                {
                    tractedObject = hit.collider.gameObject;
                }
            }
            if (hit.collider.name == "Supervisor")
            {
                SetCursor(1);
                if (Input.GetMouseButtonUp(0))
                {
                    if (Dock.instance.tutorialDockingDone)
                    {
                        GameManager.instance.dockingTime = false;
                        Dock.instance.visible = false;
                        DebrisSpawner.instance.isEnabled = true;
                    }
                }
            }
        }
        else
        {
            canvasObject.gameObject.SetActive(false);
            SetCursor(0);
        }
        // MOUSE EVENT
        if (Input.GetMouseButtonUp(0))
        {
            tractedObject = null;
        }
        if (tractedObject == null) // If debris flies off screen and gets destroyed
        {
            audioS.Pause();
            tractorBeam.SetActive(false);
        }
        if (tractedObject != null && !Truck.instance.active)
        {
            audioS.UnPause();
            tractorBeam.SetActive(true);
            tractorBeam.GetComponent<LineRenderer>().SetPosition(0, Player.instance.transform.position);
            tractorBeam.GetComponent<LineRenderer>().SetPosition(1, tractedObject.transform.position);
            tractedObject.GetComponent<Rigidbody2D>().AddForce((Player.instance.transform.position - tractedObject.transform.position) * GameManager.instance.tractorBeamStrength);
        }
        else
        {
            tractorBeam.SetActive(false);
            tractedObject = null;
            audioS.Pause();
        }
    }
    void DisplayMouseOverText(GameObject target)
    {
        int itemValue = target.GetComponent<Debris>().value;
        canvasObject.gameObject.SetActive(true);
        itemDescription.text = target.GetComponent<Debris>().description;
        itemPrice.text = itemValue.ToString()+"$";

        if (itemValue >= 100 && itemValue < 500)
            bgImage.color = new Color(0, 1f, 0, .5f);
        else if (itemValue >= 500 && itemValue < 1000)
            bgImage.color = new Color(0, 0, 1f, .5f);
        else if (itemValue >= 1000 && itemValue < 2000)
            bgImage.color = new Color(0.75f, 0, 1f, .5f);
        else if (itemValue >= 2000 )
            bgImage.color = new Color(1f, 0.4f, 0, .5f);
        if (target.GetComponent<Debris>().isMine)
        {
            bgImage.color = new Color(1f, 0, 0, .5f);
            itemPrice.text = "DEATH";
        }
    }
    bool DisplayMouseOverTextFix()
    {
        canvasObject.gameObject.SetActive(true);

        if (Truck.instance.health < 5)
        { 
            if (GameManager.instance.money > GameManager.instance.fixCost)
            {
                bgImage.color = new Color(0, 1f, 0, .5f);
                itemDescription.text = "Fix truck for:";
                itemPrice.text = GameManager.instance.fixCost.ToString()+"$";
                return true;
            }
            else
            {
                itemDescription.text = "Lacking cash!";
                bgImage.color = new Color(1f, 0, 0, .5f);
                itemPrice.text = GameManager.instance.fixCost.ToString()+"$";
                return false;
            }

        }
        else
        {
            itemDescription.text = "No need to fix.";
            itemPrice.text = "";
            return false;
        }
    }
}