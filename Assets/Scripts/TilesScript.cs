using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesScript : MonoBehaviour
{

    private Color color;
    private Color color1;

    private void OnMouseDown()
    {
        if (BoardManager.toInstantiate != gameObject) BoardManager.toInstantiate = gameObject; else BoardManager.toInstantiate = null;
        BoardManager.rotation = 0;
    }


    void Update()
    {
        if (BoardManager.toInstantiate == gameObject)
        {
            color = new Vector4(0, 1, 0, 1);
            gameObject.GetComponent<SpriteRenderer>().color = color;
        }
        if (BoardManager.toInstantiate != gameObject || BoardManager.rotation != 0)
        {
            color1 = new Vector4(1, 1, 1, 1);
            gameObject.GetComponent<SpriteRenderer>().color = color1;
        }
    }


}
