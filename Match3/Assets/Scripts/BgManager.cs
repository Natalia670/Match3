using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgManager : MonoBehaviour
{

    public int x;
    public int y;
    public GameObject[,] matriz;
    public GameObject ball;
    public static GameObject selected;
    public static GameObject onTarget;
    public Sprite [] pokeballs = new Sprite[4];


    // Start is called before the first frame update
    void Start()
    {
        SetUp();
    }

    // Update is called once per frame
    void Update()
    {
        /*for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                    if (matriz[i, j].GetComponent<SpriteRenderer>().sprite.name == matriz[i, j + 1].GetComponent<SpriteRenderer>().sprite.name)
                    {

                    }
             

            }
        }*/
    }

    void SetUp()
    {
        matriz = new GameObject[x, y];
   

        for (int i = 0; i<x; i++)
        {
            for (int j = 0; j < y; j++)
            {

                Sprite newSprite = pokeballs[Random.Range(0,pokeballs.Length)];
                ball.GetComponent<SpriteRenderer>().sprite = newSprite;

                GameObject o = Instantiate(ball, new Vector2(i*2, j*2), Quaternion.identity);

                matriz[i, j] = o;       

            }
        }

    }
}
