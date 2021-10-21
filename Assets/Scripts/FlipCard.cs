using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipCard : MonoBehaviour
{
    public float x, y, z;
    public GameObject cardBack;
    public GameObject cardFront;
    public bool isCardBackActive; 
    public int rotation;
    private GameManager gameManager;
    public float initialZ; 
    // Start is called before the first frame update
    void Start()
    {
        transform.localPosition = transform.localPosition + new Vector3(0, 0, -initialZ);
        //transform.position = transform.TransformPoint(0, 0, -initialZ);
        isCardBackActive = true;
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetMouseButtonDown(0))
        {
            StartFlip();
        }*/
    }
    //corutine is the ability in unity to pause the execution of a function and then continue the execution on the next frame, thus in this case unity will 
    public void StartFlip()
    {
        //simple flip of cards 
/*        transform.Rotate(new Vector3(x, y, z));
        Flip();*/

        StartCoroutine(CalculateInPlaceFlipAnimation());
    }
    public void StartDeckFlip()
    {
        //flip specifically to open a card from a deck 
        StartCoroutine(CalculateDeckFlipAnimation());
        
    }


    public void Flip()
    {
        
        if (isCardBackActive == true)
        {
            cardBack.SetActive(false);
            //cardFront.SetActive(true);
            isCardBackActive = false;

        }
        else
        {
            cardBack.SetActive(true);
            //cardFront.SetActive(false);
            isCardBackActive = true;
        }
        
    }

    

    IEnumerator CalculateInPlaceFlipAnimation()
    {
        
        for (int i = 0; i < 180; i++)
        {
            yield return new WaitForSeconds(0.001f);
            transform.Rotate(new Vector3(x, y, z));
            rotation++;
            //Debug.Log(rotation);
            if (rotation == 90 || rotation == -90)
            {
                Flip();
            }
        }

        rotation = 0;
        
        
    }

    IEnumerator CalculateDeckFlipAnimation()
    {

        for (int i = 0; i < 180; i++)
        {
            yield return new WaitForSeconds(0.001f);
            transform.Rotate(new Vector3(x, y, z));
            rotation++;
            
            if (rotation == 90 || rotation == -90)
            {
                Flip();
            }

            if (transform.position.x < -6)
            {
                transform.position= new Vector3(transform.position.x + 0.017f, transform.position.y , transform.position.z);
            }
        }

        rotation = 0;


    }

}
