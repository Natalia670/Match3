using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    private static Color selectedColor = new Color(.5f, .5f, .5f, 1.0f);
    private static Ball previousSelected = null;

    private SpriteRenderer render;
    private bool isSelected = false;

    private Vector2[] adjacentDirections = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    void Awake()
    {
        render = GetComponent<SpriteRenderer>();
    }

    private void Select()
    {
        isSelected = true;
        render.color = selectedColor;
        previousSelected = gameObject.GetComponent<Ball>();
    }

    private void Deselect()
    {
        isSelected = false;
        render.color = Color.white;
        previousSelected = null;
        BgManager.selected = null;
        BgManager.onTarget = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {
        //Debug.Log(this.GetComponent<SpriteRenderer>().sprite.name);
        //Debug.Log("hola");

        if (BgManager.selected == null)
        {
            BgManager.selected = this.gameObject;
            Debug.Log("Esta seleccionado");
            Select();

        }
        else
        {
            BgManager.onTarget = this.gameObject;
            //Debug.Log("On target");
      

            if (GetAllAdjacentTiles().Contains(BgManager.onTarget.gameObject))
            {
                Debug.Log("YEEEY");
                SwapSprite(previousSelected.render);
                
                previousSelected.Deselect();
            }
            else
            { 
                previousSelected.GetComponent<Ball>().Deselect();
                Select();
                Debug.Log("NOOO");
            } 
           
            //SwapSprite(previousSelected.render);
            //Deselect();
        }

    }

    public void SwapSprite(SpriteRenderer render2)
    { 
        if (render.sprite == render2.sprite)
        { 
            return;
        }

        Sprite tempSprite = render2.sprite; 
        render2.sprite = render.sprite; 
        render.sprite = tempSprite;
        render2.color = Color.white;
        render.color = Color.white;

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
            Debug.Log(GetAdjacent(adjacentDirections[i]));
        }
        return adjacentTiles;
    }


}
