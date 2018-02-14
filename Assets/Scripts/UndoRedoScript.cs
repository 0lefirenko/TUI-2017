using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoRedoScript : MonoBehaviour
{

    public Sprite[] arraySprites;
    public static int maxRedo = 0;

    private void OnMouseDown()
    {
        if (BlockScript.undoI >= 0)
        {
            maxRedo++;
            RaycastHit2D hit = Physics2D.Raycast(new Vector3(UndoRedo.xPos[BlockScript.undoI], UndoRedo.yPos[BlockScript.undoI], -10f), Vector2.zero);
            if (UndoRedo.spriteNumber[BlockScript.undoI] != -1)
                hit.transform.GetComponent<SpriteRenderer>().sprite = arraySprites[UndoRedo.spriteNumber[BlockScript.undoI]];
            else
                hit.transform.GetComponent<SpriteRenderer>().sprite = null;
            hit.transform.rotation = new Quaternion(UndoRedo.quaternionX[BlockScript.undoI], UndoRedo.quaternionY[BlockScript.undoI], UndoRedo.quaternionZ[BlockScript.undoI], UndoRedo.quaternionW[BlockScript.undoI]);
            BlockScript.undoI--;
            
        }
    }
}
