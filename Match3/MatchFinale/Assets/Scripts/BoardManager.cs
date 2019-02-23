using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Retrieved from: https://www.raywenderlich.com/673-how-to-make-a-match-3-game-in-unity

public class BoardManager : MonoBehaviour
{
    public static BoardManager instance;
    public List<Sprite> pokeballs = new List<Sprite>();
    public GameObject ball;
    public int xSize, ySize;
    public Text score;

    public static BoardManager Instance;
    public static BoardManager GetInstance() {
        return Instance;
    }

    private GameObject[,] matriz;

    public bool IsShifting { get; set; }

    private int counter;

    void Start()
    {
        instance = GetComponent<BoardManager>();

        Vector2 offset = ball.GetComponent<SpriteRenderer>().bounds.size;
        CreateBoard(offset.x, offset.y);
    }


    private void CreateBoard(float xOffset, float yOffset)
    {
        matriz = new GameObject[xSize, ySize];

        float startX = transform.position.x;
        float startY = transform.position.y;

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                GameObject newBall = Instantiate(ball, new Vector3(startX + (xOffset * x), startY + (yOffset * y), 0), ball.transform.rotation);
                matriz[x, y] = newBall;

                newBall.transform.parent = transform;

                Sprite newSprite = pokeballs[Random.Range(0, pokeballs.Count)];
                newBall.GetComponent<SpriteRenderer>().sprite = newSprite;

            }
        }
    }

    public IEnumerator FindNullTiles()
    {
        

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                if (matriz[x, y].GetComponent<SpriteRenderer>().sprite == null)
                {
                    
                    counter++;
                    //Debug.Log(counter);
                    yield return StartCoroutine(ShiftTilesDown(x, y));

                    break;
                }
            }
        }
        score.text = "Eliminadas:" + counter.ToString();
    }

    public void Mushroom_attack()
    {
        GameObject[] obj;
        obj = GameObject.FindGameObjectsWithTag("Ball");

        for(int i = 0; i < obj.Length; i++)
        {
            Sprite newSprite = pokeballs[Random.Range(0, pokeballs.Count)];
            obj[i].GetComponent<SpriteRenderer>().sprite = newSprite;
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    private IEnumerator ShiftTilesDown(int x, int yStart, float shiftDelay = .05f)
    {
        IsShifting = true;
        List<SpriteRenderer> renders = new List<SpriteRenderer>();
        int nullCount = 0;

        for (int y = yStart; y < ySize; y++)
        {
            SpriteRenderer render = matriz[x, y].GetComponent<SpriteRenderer>();
            if (render.sprite == null)
            {
                nullCount++;
            }
            renders.Add(render);
        }

        for (int i = 0; i < nullCount; i++)
        {
            yield return new WaitForSeconds(shiftDelay);
            for (int k = 0; k < renders.Count - 1; k++)
            {
                renders[k].sprite = renders[k + 1].sprite;
                renders[k + 1].sprite = GetNewSprite(x, ySize - 1); ;
            }
        }
        IsShifting = false;
    }

    private Sprite GetNewSprite(int x, int y)
    {
        List<Sprite> possiblePokeballs = new List<Sprite>();
        possiblePokeballs.AddRange(pokeballs);

        if (x > 0)
        {
            possiblePokeballs.Remove(matriz[x - 1, y].GetComponent<SpriteRenderer>().sprite);
        }
        if (x < xSize - 1)
        {
            possiblePokeballs.Remove(matriz[x + 1, y].GetComponent<SpriteRenderer>().sprite);
        }
        if (y > 0)
        {
            possiblePokeballs.Remove(matriz[x, y - 1].GetComponent<SpriteRenderer>().sprite);
        }

        return possiblePokeballs[Random.Range(0, possiblePokeballs.Count)];
    }
}