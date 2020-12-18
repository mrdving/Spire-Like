using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public GameObject[] cards;
    private RectTransform[] cards_pos;
    public PileManager pile;
    private int maxCards;

    public RectTransform[] markerPoints;
    public int activatingMarkers = 0;
    public Vector2 markerCenter;
    public float markerInterval;
    public Vector2 deckCenter;
    //
    public bool ready = false;

    private void Start()
    {
        StartCoroutine(Initialize());
    }

    IEnumerator Initialize()
    {
        ready = false;
        while (!pile.ready) yield return null;
        maxCards = pile.maxCards;
        cards_pos = new RectTransform[maxCards];
        for (int i = 0; i < cards.Length; i++)
        {
            cards_pos[i] = cards[i].GetComponent<RectTransform>();
        }
        //check ready
        for (int i = 0; i < cards.Length; i++)
        {
            while (!cards[i].GetComponent<PlayableCard>().ready) yield return null;
        }
        Refresh();
        //hide cards
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].SetActive(false);
        }
        ready = true;
    }

    public void Refresh()
    {
        RefreshMarker();
        RefreshCardVessel();
        RefreshPlayableCards();
    }
    public void RefreshMarker()
    {
        float cardCount = pile.handPile.Count;
        for (int i = 0; i < maxCards; i++)
        {
            if (i < cardCount)
            {
                markerPoints[i].gameObject.SetActive(true);
                markerPoints[i].GetComponent<ShakeAni>().ClearShake();
                markerPoints[i].anchoredPosition = markerCenter + markerInterval * (i - (cardCount - 1) / 2) * Vector2.right;
            }
            else
            {
                markerPoints[i].gameObject.SetActive(false);
            }

        }
    }
    public void RefreshCardVessel()
    {
        //refresh vessel display
        for (int i = 0; i < maxCards; i++)
        {
            if (i < pile.handPile.Count)
            {
                cards[i].SetActive(true);
                cards[i].GetComponent<CardVessel>().card = pile.handPile[i];
                cards[i].GetComponent<CardVessel>().Refresh();
                
            }
            else
            {
                cards[i].SetActive(false);
                cards[i].GetComponent<RectTransform>().anchoredPosition = deckCenter;
            }
        }
    }
    public void RefreshPlayableCards()
    {
        for (int i = 0; i < maxCards; i++)
        {
            cards[i].GetComponent<PlayableCard>().RefreshFollow();
            if (i < pile.handPile.Count)
            {
                cards[i].GetComponent<PlayableCard>().card = pile.handPile[i];
                cards[i].GetComponent<PlayableCard>().RefreshStats();

            }
        }

    }

    public void RemoveCard(PlayableCard card)
    {
        int i = -1;
        for(int k = 0; k < cards.Length; k++)
        {
            if(cards[k].GetComponent<PlayableCard>() == card)
            {
                i = k;
                break;
            }
        }
        if (i == -1)
        {
            Debug.Log("Error in removing card", this);
            return;
        }
        
        //rearrange card follow
        //do nothing if it's the last card
        if (i == pile.handPile.Count) return;
        //move all following marker to k - 1
        for(int k = i + 1; k < pile.handPile.Count; k++)
        {
            cards[k].GetComponent<Follow_UI>().followTarget = markerPoints[k - 1];
        }
        //assign the removed card to the last slot
        cards[i].GetComponent<Follow_UI>().followTarget = markerPoints[pile.handPile.Count - 1];
        //move the ith card to the last
        GameObject tmp = cards[i];
        for (int k = i ; k < pile.handPile.Count - 1; k++)
        {
            cards[k] = cards[k + 1];
        }
        cards[pile.handPile.Count - 1] = tmp;
        //update markers and vessel displays and card follow
        Refresh();
        
        //call card removal
        if (cards[i].GetComponent<PlayableCard>().card.exhaust) pile.Exhaust(i);
        else pile.Discard(i);

    }

    public void SetInteractable(bool interactable)
    {
        for(int i = 0; i < cards.Length; i++)
        {
            cards[i].GetComponent<PlayableCard>().interactable = interactable;
        }
    }

    public int TakeDamage(int damage)
    {
        for(int i = 0; i < pile.handPile.Count; i++)
        {
            if (cards[i].GetComponent<PlayableCard>().broken) continue;
            damage = cards[i].GetComponent<PlayableCard>().TakeDamage(damage);
            if (damage <= 0) return 0; //block success
        }
        return damage;//block fail
    }
}