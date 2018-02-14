using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedoScript : MonoBehaviour
{

    public Sprite[] arraySprites;

    private void OnMouseDown()
    {
        if (UndoRedoScript.maxRedo > 0)
        {
            BlockScript.undoI++;
            RaycastHit2D hit = Physics2D.Raycast(new Vector3(UndoRedo.xPos[BlockScript.undoI], UndoRedo.yPos[BlockScript.undoI], -10f), Vector2.zero);
            if (UndoRedo.newSpriteNumber[BlockScript.undoI] != -1)
                hit.transform.GetComponent<SpriteRenderer>().sprite = arraySprites[UndoRedo.newSpriteNumber[BlockScript.undoI]];
            else
                hit.transform.GetComponent<SpriteRenderer>().sprite = null;
            hit.transform.rotation = new Quaternion(UndoRedo.newQuaternionX[BlockScript.undoI], UndoRedo.newQuaternionY[BlockScript.undoI], UndoRedo.newQuaternionZ[BlockScript.undoI], UndoRedo.newQuaternionW[BlockScript.undoI]);
            UndoRedoScript.maxRedo--;

        }


    }
}