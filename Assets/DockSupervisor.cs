using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DockSupervisor : MonoBehaviour {

    public Canvas canvas;
    public Text text;
    public List<string> dialog = new List<string>();

    private void Update()
    {
        if (Dock.instance.tutorialDockingDone)
        {
            text.text = "Click me when you are ready to continue.";

        }
    }
    
}
