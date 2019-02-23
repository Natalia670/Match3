using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBall : Ball1
{
    public  void ClearMatch(Vector2[] paths)
    {
        List<GameObject> matchingBalls = new List<GameObject>();
        for (int i = 0; i < paths.Length; i++)
        {
            matchingBalls.AddRange(FindMatch(paths[i]));
        }
        if (matchingBalls.Count >= 2)
        {
            for (int i = 0; i < matchingBalls.Count; i++)
            {
                
                matchingBalls[i].GetComponent<SpriteRenderer>().sprite = null;

                
            }
            matchFound = true;
        }
    }

    void OnMouseDown()
    {
        Select();
        //SwapSprite(previousSelected.render);
    }
}
