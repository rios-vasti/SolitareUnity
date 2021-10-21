using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateCard : MonoBehaviour
{
    //public Sprite cardFace;
    //public Sprite cardBack;
    public SpriteRenderer spriteRenderer;
    //maybe add a separate selectrable??
    private GameManager gameManager;
    private List<string> newDeck;
    // Start is called before the first frame update
    void Start()
    {
        newDeck = GameManager.GenerateStringDeck();
        gameManager = FindObjectOfType<GameManager>();
        assignCardFace();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void assignCardFace()
    {

        for (int i = 0; i < newDeck.Count; i++)
        {
            if (name == newDeck[i])
            {
                this.GetComponent<SpriteRenderer>().sprite = gameManager.cardSprites[i];
                break;
            }
        }
    }
}
