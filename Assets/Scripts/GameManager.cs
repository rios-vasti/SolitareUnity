using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public static string[] suits = new string[] {"C", "D", "H", "S"};
    public static string[] values = new string[] {"A", "2", "3", "4", "5","6","7","8","9", "10","J", "Q","K"};
    public List<string> stringDeck;
    public Sprite[] cardSprites;
    public GameObject cardPrefab;
    public float cardYOffset;

    public GameObject[] SpriteCardsInPlayPlaceHolders;
    public GameObject[] SpriteCardsFinishedPlaceHolders;
    public GameObject SpriteDeckPlaceHolder;

    public List<string>[] cardsInPlay;
    public List<string> cardsInPlay0;
    public List<string> cardsInPlay1;
    public List<string> cardsInPlay2;
    public List<string> cardsInPlay3;
    public List<string> cardsInPlay4;
    public List<string> cardsInPlay5;
    public List<string> cardsInPlay6;

    private int deckSortingLayerOrder = 0;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this; 
        }
    }
    void Start()
    {
        this.cardsInPlay = new List<string>[] { cardsInPlay0, cardsInPlay1, cardsInPlay2, cardsInPlay3, cardsInPlay4, cardsInPlay5, cardsInPlay6 };
        this.SetUpGame();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = Camera.main.transform.position.z;
            Ray ray = new Ray(mousePos, new Vector3(0, 0, 1));
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D[] hit = Physics2D.RaycastAll(mousePos2D, Vector2.zero);
            RaycastHit2D theHit = hit[0]; 
            foreach (RaycastHit2D h in hit)
            {
                if (h.collider.gameObject.transform.localPosition.z < theHit.collider.gameObject.transform.localPosition.z)
                {
                    theHit = h;
                }
                //Debug.Log("Hits: " + h.collider.gameObject.name + " Pos: " + h.collider.gameObject.transform.localPosition.z);
            }
            //RaycastHit2D[] hit = Physics2D.GetRayIntersectionAll(ray);
            if (theHit.collider != null)
            {
                //Debug.Log(theHit.collider.gameObject.name);
                if (theHit.collider.gameObject.transform.IsChildOf(SpriteDeckPlaceHolder.transform))
                {
                    //FIXXXXXXXXXXX theHit.collider.gameObject.GetComponent<SortingLayer>() = theHit.collider.gameObject.GetComponent<SpriteRenderer>().sortingOrder + deckSortingLayerOrder;
                    theHit.collider.gameObject.GetComponent<FlipCard>().StartDeckFlip();
                    deckSortingLayerOrder += 1; 
                }
                else
                {
                    theHit.collider.gameObject.GetComponent<FlipCard>().StartFlip();
                }
                
                
            }
            
        }
    }
    public void SetUpGame()
    {
        stringDeck = GenerateStringDeck();
        this.Shuffle(stringDeck);
        this.DistributeCards();
        this.CreateSpriteCards();
        
    }
    public void PrintCards()
    {

        foreach (string card in stringDeck)
        {
            Debug.Log(card);
        }
    }

    public static List<string> GenerateStringDeck()
    {
        List<string> newDeck = new List<string>();
        foreach (string v in values)
        {
            foreach(string s in suits)
            {
                newDeck.Add(s + v);
            }
        }
        return newDeck;
    }
    //will iterate thorugh a list and switch it place with a randomly generated position 
    void Shuffle<T> (List <T> list)
    {
        System.Random rand = new System.Random();
        int num = list.Count -1; //gets the size of the list 
        //^is used to both ensure the random does not get a number greater the num of items in the list and to keep track of what positions in the list we have already shuffled
        while (num > 1)
        {
            int randPos = rand.Next(num); // this will get a random number form 0 up to the max
            T temp = list[randPos];
            list[randPos] = list[num];
            list[num] = temp;
            num--; 
        }
    }
  
    public void CreateSpriteCards()
    {
        //this is to instanciate the cards that are on the player's board 
        float yOffsetCounter = 0;
        float zOffsetCounter = 0;
        for (int i = 0; i < 7; i ++)
        {
            List<string> cardsInPlayStr = cardsInPlay[i];
            foreach (string card in cardsInPlayStr)
            {
                Vector3 ipos = SpriteCardsInPlayPlaceHolders[i].transform.TransformPoint(0, - yOffsetCounter, - zOffsetCounter);
                GameObject newCard = Instantiate(cardPrefab, ipos, Quaternion.identity, SpriteCardsInPlayPlaceHolders[i].transform);
                
                newCard.name = card;
                newCard.GetComponent<FlipCard>().initialZ = zOffsetCounter; 
                (newCard.GetComponent<SpriteRenderer>()).sortingOrder = 2;
                //ADD THE ACTUAL Sprite faces of the cards
                //Why does the back of the card not get set to the correct sorting order??
                //(newCard.GetComponentInChildren<SpriteRenderer>()).sortingOrder = 2;

                yOffsetCounter = yOffsetCounter + cardYOffset;
                zOffsetCounter = zOffsetCounter + 1f;
                //Debug.Log("Z offset: " + zOffsetCounter);
            }
            yOffsetCounter = 0;
            zOffsetCounter = 0;
        }
        //this is to instanciate the cards that are int the remainders deck 
        foreach (string card in stringDeck)
        {
            GameObject newCard = Instantiate(cardPrefab, new Vector3(SpriteDeckPlaceHolder.transform.position.x, SpriteDeckPlaceHolder.transform.position.y - yOffsetCounter, SpriteDeckPlaceHolder.transform.position.z), Quaternion.identity, SpriteDeckPlaceHolder.transform);
            newCard.name = card;
            (newCard.GetComponent<SpriteRenderer>()).sortingOrder = 2;
        }



    }

    public void DistributeCards()
    {
        for (int i = 0; i <7; i++)
        {
            for (int j = i; j < 7; j++)
            {
                cardsInPlay[j].Add(stringDeck[stringDeck.Count - 1]);
                stringDeck.RemoveAt(stringDeck.Count - 1); 

            }
        }

    }

}
