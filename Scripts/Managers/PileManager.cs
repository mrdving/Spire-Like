using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PileManager : MonoBehaviour
{
    public List<Card> drawPile, discardPile, exhaustPile, handPile;
    private DeckManager deck;
    private HandManager hand;
    public bool ready = false;
    public int maxCards = 5;

    private void Start()
    {
        StartCoroutine(Initialize());
    }

    private IEnumerator Initialize()
    {
        ready = false;
        deck = FindObjectOfType<DeckManager>();
        hand = FindObjectOfType<HandManager>();
        StartCoroutine(PrepareForBattle());
        yield return null;
    }

    public IEnumerator PrepareForBattle()
    {
        ready = false;
        while (!deck.ready) { yield return null; }
        drawPile = new List<Card>();
        discardPile = new List<Card>();
        exhaustPile = new List<Card>();
        handPile = new List<Card>();
        foreach (Card card in deck.deck)
        {
            drawPile.Add(card);
        }
        ready = true;
    }

    public void Shuffle()
    {
        //put discard back to draw
        while(discardPile.Count > 0)
        {
            drawPile.Add(discardPile[0]);
            discardPile.Remove(discardPile[0]);
        }
    }

    public void Draw(int drawNum)
    {
        Card temp;
        for (int i = 0; i < drawNum; i++) {
            //check empty
            if(drawPile.Count == 0)
            {
                Shuffle();
            }
            //exit if still no cards
            if (drawPile.Count == 0)
            {
                Debug.Log("sorry, no cards left ;<");
                break;
            }
            //exit if hand full
            if(handPile.Count >= maxCards)
            {
                Debug.Log("Hand full, you greedy swine");
                break;
            }
            //choose random card from draw pile
            temp = drawPile[Mathf.FloorToInt((Random.value-float.Epsilon) * drawPile.Count)];
            //add to hand and remove from draw
            handPile.Add(temp);
            drawPile.Remove(temp);
            hand.Refresh();
        }
    }

    public void Discard(int i)
    {
        discardPile.Add(handPile[i]);
        handPile.Remove(handPile[i]);
        hand.Refresh();
    }
    public void DiscardAll()
    {
        for(int i = handPile.Count - 1; i >= 0; i--)
        {
            discardPile.Add(handPile[i]);
            handPile.Remove(handPile[i]);
        }
        hand.Refresh();
    }

    public void Exhaust(int i)
    {
        handPile.Remove(handPile[i]);
        hand.Refresh();
    }
}
