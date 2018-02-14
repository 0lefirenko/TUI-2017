using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public struct UndoRedo
{
    public static int[] xPos = new int[1000];
    public static int[] yPos = new int[1000];
    public static int[] xPosForDrag = new int[1000];
    public static int[] yPosForDrag = new int[1000];
    public static int[] spriteNumber = new int[1000];
    public static int[] newSpriteNumber = new int[1000];
    public static float[] quaternionW = new float[1000];
    public static float[] quaternionX = new float[1000];
    public static float[] quaternionY = new float[1000];
    public static float[] quaternionZ = new float[1000];
    public static float[] newQuaternionW = new float[1000];
    public static float[] newQuaternionX = new float[1000];
    public static float[] newQuaternionY = new float[1000];
    public static float[] newQuaternionZ = new float[1000];
    public static int[] eds = new int[1000];
    public static int[] resist = new int[1000];
}

public class BlockScript : MonoBehaviour
{
    IEnumerator Waiting(float waitTime)                //Timer coroutine
    {
        yield return new WaitForSeconds(waitTime);          //Waits for alotted seconds to pass
        hold = false;                                       //sets hold bool to false (no longer allows DoubleClick action)
    }

    private GameObject instantiateObject;
    private Sprite tempSprite;
    private Vector3 dragPosition;
    public Camera _camera;
    public Sprite[] arraySprites;
    public static int undoI = -1;
    public int resist = -1;
    public int eds = -1;
    public bool hold = false;               //Begins hold bool as false
    public float secondsToWait = 1f;      //sets the timer between clicks in seconds (default is 1.0)
    private bool doResist, doEds;





    public int getIntOfImage(Sprite sprite1)
    {

        for (int i = 0; i < arraySprites.Length; i++)
        {
            if (arraySprites[i] == sprite1)
            {
                return i;
            }
        }
        return -1;
    }

    private void OnGUI()
    {
        GUI.color = Color.cyan;
        if (doEds)
        {
            GUI.Label(new Rect((8.5f / 47f) * Screen.width + transform.position.x * (2.42f / 47f) * Screen.width, Screen.height - ((0.1f / 47f) + (transform.position.y +0.9f) * (2.43f / 47f) * Screen.width), 45, 20), eds.ToString() + " В");
        }

        GUI.color = Color.cyan;
        if (doResist)
        {
            GUI.Label(new Rect(((8.5f+0.6f)/47f)*Screen.width + transform.position.x * (2.42f / 47f) * Screen.width, Screen.height - ((0.8f / 47f) + (transform.position.y + 0.65f) * (2.42f / 47f) * Screen.width), 60, 20), resist.ToString() + " Ом");
        }
    }

    private void Update()
    {

        if (getIntOfImage(gameObject.GetComponent<SpriteRenderer>().sprite) == 6)
        {
            doResist = true;
        }
        else doResist = false;

        if (getIntOfImage(gameObject.GetComponent<SpriteRenderer>().sprite) == 3)
        {
            doEds = true;
        }
        else doEds = false;

        if (Input.GetMouseButtonUp(1))
        {
            RaycastHit2D hit1 = Physics2D.Raycast(_camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit1.transform.position == transform.position)
            {
                UndoRedoScript.maxRedo = 0;
                undoI++;
                SaveData(undoI);
                RaycastHit2D hit = Physics2D.Raycast(_camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                hit.transform.GetComponent<SpriteRenderer>().sprite = null;
                BoardManager.toInstantiate = null;
                BoardManager.rotation = 0;
                SaveNewData(undoI);
            }
        }
            

    }

    void OnMouseDown()
    {
        if (hold == true && (getIntOfImage(gameObject.GetComponent<SpriteRenderer>().sprite) == 6 || getIntOfImage(gameObject.GetComponent<SpriteRenderer>().sprite) == 3)) //these actions can occur if bool is true
        {
            LabelScript.x = transform.position.x;
            LabelScript.y = transform.position.y;
            if (getIntOfImage(gameObject.GetComponent<SpriteRenderer>().sprite) == 6)
            {
                LabelScript.doResistWindow = true;
            }
            if (getIntOfImage(gameObject.GetComponent<SpriteRenderer>().sprite) == 3)
            {
                LabelScript.doEdsWindow = true;
            }
            
        }
        hold = true;                                    //Sets hold bool to true
        StartCoroutine(Waiting(secondsToWait));    //Begins timer


        if (BoardManager.rotation != 0)
        {
            UndoRedoScript.maxRedo = 0;
            undoI++;
            SaveData(undoI);
            transform.Rotate(new Vector3(0, 0, BoardManager.rotation));
            SaveNewData(undoI);
        }
        else
        if (BoardManager.toInstantiate != null && BoardManager.toInstantiate != gameObject.GetComponent<SpriteRenderer>().sprite)
            {
            UndoRedoScript.maxRedo = 0;
            undoI++;
            SaveData(undoI);
            gameObject.transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0, 0, 0));
            gameObject.GetComponent<SpriteRenderer>().sprite = BoardManager.toInstantiate.GetComponent<SpriteRenderer>().sprite;
            SaveNewData(undoI);
        }
    }



    private void OnMouseUp()
    {
        RaycastHit2D hit = Physics2D.Raycast(_camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.transform != null)
        {
            
            tempSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
            gameObject.GetComponent<SpriteRenderer>().sprite = null;
            hit.transform.GetComponent<SpriteRenderer>().sprite = tempSprite;
            hit.transform.SetPositionAndRotation(hit.transform.position, gameObject.transform.rotation);

        }
    }









    private void SaveData(int i)
    {
        UndoRedo.xPos[i] = (int)gameObject.transform.position.x;
        UndoRedo.yPos[i] = (int)gameObject.transform.position.y;
        UndoRedo.spriteNumber[i] = getIntOfImage(gameObject.GetComponent<SpriteRenderer>().sprite);
        UndoRedo.quaternionW[i] = gameObject.transform.rotation.w;
        UndoRedo.quaternionX[i] = gameObject.transform.rotation.x;
        UndoRedo.quaternionY[i] = gameObject.transform.rotation.y;
        UndoRedo.quaternionZ[i] = gameObject.transform.rotation.z;
    }


    private void SaveNewData(int i)
    {
        UndoRedo.newSpriteNumber[i] = getIntOfImage(gameObject.GetComponent<SpriteRenderer>().sprite);
        UndoRedo.newQuaternionW[i] = gameObject.transform.rotation.w;
        UndoRedo.newQuaternionX[i] = gameObject.transform.rotation.x;
        UndoRedo.newQuaternionY[i] = gameObject.transform.rotation.y;
        UndoRedo.newQuaternionZ[i] = gameObject.transform.rotation.z;
    }

}