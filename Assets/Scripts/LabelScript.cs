using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabelScript : MonoBehaviour {

    public static float x, y;
    public static bool doResistWindow, doEdsWindow;
    private Rect windowRect0 = new Rect(Screen.width / 3, Screen.height / 3, Screen.width / 3, Screen.height / 3);
    private string resist, eds;
    public GameObject block;
    private GUIStyle style;

    void OnGUI()
    {
        if (doEdsWindow)
        {
            GUI.color = Color.cyan;
            windowRect0 = GUI.Window(1, windowRect0, DoMyWindow, "Введите ЭДС источника:");
        }

        if (doResistWindow)
        {
            GUI.color = Color.cyan;
            windowRect0 = GUI.Window(0, windowRect0, DoMyWindow, "Введите сопротивление резистора:");
        }
    }

    void DoMyWindow(int windowID)
    {
        GUI.color = Color.cyan;
        resist = GUI.TextField(new Rect(10, Screen.height / 10, (Screen.width / 3 - 20), Screen.height / 30), resist);
        if (GUI.Button(new Rect(Screen.width / 21, Screen.height / 4, Screen.width / 9, Screen.width / 50), "Подтвердить"))
        {
            GUI.color = Color.cyan;
            RaycastHit2D hit = Physics2D.Raycast(new Vector3(x, y, -10f), Vector2.zero);
            if (windowID == 0)
            {
                hit.collider.gameObject.GetComponent<BlockScript>().resist = int.Parse(resist);
                doResistWindow = false;
            }
            if (windowID == 1)
            {
                hit.collider.gameObject.GetComponent<BlockScript>().eds = int.Parse(resist);
                doEdsWindow = false;
            }
        }
        if (GUI.Button(new Rect(Screen.width / 6, Screen.height / 4, Screen.width / 9, Screen.width / 50), "Отмена"))
        {
            doEdsWindow = false;
            doResistWindow = false;
        }
        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
    }

       

}

