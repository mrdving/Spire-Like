using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public List<Card> deck;
    public CardLibrary lib;
    public Card[] cards;
    public bool ready = false;
    public int deckSize = 10;

    private void Start()
    {
        StartCoroutine(Initialize());
    }

    IEnumerator Initialize()
    {
        while (!lib.ready) { yield return null; }
        //initialize deck
        //for testting////////////
        cards = new Card[lib.cardDic.Values.Count];
        lib.cardDic.Values.CopyTo(cards, 0);
        for(int i = 0; i < deckSize; i++)
        {
            deck.Add(cards[(int)(Random.value * cards.Length)]);
        }
        //////////////////////////
        ready = true;
    }

    public void AddCard(Card card)
    {
        deck.Add(card);
    }

    public void RemoveCard(Card card)
    {
        deck.Remove(card);
    }

}
