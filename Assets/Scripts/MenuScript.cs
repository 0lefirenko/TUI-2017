using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour {

    // Use this for initialization
    private void OnMouseDown()
    {
        Debug.Log("Exit");
        Application.Quit();
        //Application.LoadLevel("Menu");
    }
}
