using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CardManagerScript : MonoBehaviour
{
    public List<RectTransform> evenPositions;
    public List<RectTransform> oddPositions;
    private CardScript[] cards;
    // Start is called before the first frame update
    void Start()
    {
        CardUsed(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CardUsed(bool firstTime){
        CardScript[] originalArray = GetComponentsInChildren<CardScript>();
        cards = GetComponentsInChildren<CardScript>();
        bool[] isCardActive = new bool[cards.Length];
        int numCardsActive = 0;
        foreach(CardScript c in cards){
            isCardActive[Array.IndexOf(cards, c)] = c.gameObject.activeInHierarchy;
            if(isCardActive[Array.IndexOf(cards, c)]){
                numCardsActive++;
            }
        }

        if(numCardsActive == 0){
            return;
        }
        if(numCardsActive % 2 == 0){
            int counter = 0;
            foreach(CardScript card in cards){
                if(isCardActive[Array.IndexOf(cards, card)]){ 
                    card.originalPosition = evenPositions[counter].localPosition;
                    counter++;
                }
            }
        }
        else{
            int counter = 0;
            foreach(CardScript card in cards){
                if(isCardActive[Array.IndexOf(cards, card)]){ 
                    card.originalPosition = oddPositions[counter].localPosition;
                    counter++;
                }
            }
        }
    }
}
