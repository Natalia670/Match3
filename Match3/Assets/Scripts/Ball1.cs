using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Retrieved from: https://www.raywenderlich.com/673-how-to-make-a-match-3-game-in-unity

public class Ball1 : MonoBehaviour
{
    protected static Color selectedColor = new Color(.5f, .5f, .5f, 1.0f);
    protected static Ball1 previousSelected = null;

    protected bool matchFound = false;

    protected SpriteRenderer render;
    protected bool isSelected = false;
    protected bool isStone = false;

    protected Vector2[] adjacentDirections = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    void Awake()
    {
        render = GetComponent<SpriteRenderer>();
    }

    protected void Select()
    {
        isSelected = true;
        render.color = selectedColor;
        previousSelected = gameObject.GetComponent<Ball1>();
    }

    protected void Deselect()
    {
        isSelected = false;
        render.color = Color.white;
        previousSelected = null;
    }

    void OnMouseDown()
    {

            if (render.sprite == null || BoardManager.instance.IsShifting)
            {
                return;
            }

            if (isSelected)
            {
                Deselect();
            }
            else
            {
                if (previousSelected == null)
                {
                    Select();
                }
                else
                {
                    if (GetAllAdjacentTiles().Contains(previousSelected.gameObject))
                    {

                        var tempRender = previousSelected.render;
                        SwapSprite(previousSelected.render);
                        previousSelected.ClearAllMatches(tempRender);
                        ClearAllMatches(tempRender);
                        previousSelected.Deselect();


                        //Debug.Log("YEY");
                    }
                    else
                    {
                        previousSelected.GetComponent<Ball1>().Deselect();
                        Select();
                        //Debug.Log("Noooo");
                    }


                }

            }
        
    }

    IEnumerator SwapNoMatch(SpriteRenderer tempRender) {
        Debug.Log(tempRender);
        SwapSprite(render);
        yield return new WaitForSeconds(1.0f);
        SwapSprite(tempRender);
       
    }

    protected void SwapSprite(SpriteRenderer render2)
    {
        if (render.sprite == render2.sprite)
        {
            return;
        }

        Sprite tempSprite = render2.sprite;
        render2.sprite = render.sprite;
        render.sprite = tempSprite;

    }

    private GameObject GetAdjacent(Vector2 castDir)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, castDir);
        if (hit.collider != null)
        {
            return hit.collider.gameObject;
        }
        return null;
    }

    private List<GameObject> GetAllAdjacentTiles()
    {
        List<GameObject> adjacentTiles = new List<GameObject>();
        for (int i = 0; i < adjacentDirections.Length; i++)
        {
            adjacentTiles.Add(GetAdjacent(adjacentDirections[i]));
        }
        return adjacentTiles;
    }

    protected List<GameObject> FindMatch(Vector2 castDir)
    { 
        List<GameObject> matchingBalls = new List<GameObject>(); 
        RaycastHit2D hit = Physics2D.Raycast(transform.position, castDir); 
        while (hit.collider != null && hit.collider.GetComponent<SpriteRenderer>().sprite == render.sprite)
        { 
            matchingBalls.Add(hit.collider.gameObject);
            hit = Physics2D.Raycast(hit.collider.transform.position, castDir);
        }
        return matchingBalls; 
    }

    private void ClearMatch(Vector2[] paths) 
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


    public void ClearAllMatches(SpriteRenderer tempRender)
    {
        if (render.sprite == null)
            return;

        ClearMatch(new Vector2[2] { Vector2.left, Vector2.right });
        ClearMatch(new Vector2[2] { Vector2.up, Vector2.down });
        if (matchFound)
        {
            render.sprite = null;
            matchFound = false;
            StopCoroutine(BoardManager.instance.FindNullTiles());
            StartCoroutine(BoardManager.instance.FindNullTiles());


        }
        else {

            StartCoroutine(SwapNoMatch(tempRender));

        }

    }


}
