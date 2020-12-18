using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckDisplay : MonoBehaviour
{
    private DeckManager deck;
    public GameObject container;
    public GameObject cardVesselPrefab;
    public GameObject[] displayScreen;
    private List<GameObject> cardVessels = new List<GameObject>();
    public Vector2 padding, interval;
    public int cardsPerRow = 5;
    public float scale;

    private void Start()
    {
        StartCoroutine(Initialize());
    }
    IEnumerator Initialize()
    {
        deck = FindObjectOfType<DeckManager>();
        while (!deck.ready) yield return null;
        Refresh();
        HideDisplay();
    }

    public void Refresh()
    {
        //resize content
        RectTransform containerRect = container.GetComponent<RectTransform>();
        containerRect.sizeDelta = new Vector2(containerRect.sizeDelta.x, -2*padding.y + interval.y * (deck.deck.Count/cardsPerRow));
        //correct vessel numbers
        while(deck.deck.Count > cardVessels.Count)
        {
            GameObject toInst = Instantiate(cardVesselPrefab, container.transform);
            cardVessels.Add(toInst);
            toInst.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
            toInst.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
            toInst.GetComponent<RectTransform>().localScale = new Vector2(scale, scale);
        }
        while(deck.deck.Count < cardVessels.Count)
        {
            cardVessels.Remove(cardVessels[0]);
        }
        //import cards
        for(int i = 0; i < cardVessels.Count; i++)
        {
            cardVessels[i].GetComponent<CardVessel>().card = deck.deck[i];
            cardVessels[i].GetComponent<CardVessel>().Refresh();
        }
        //position cards
        for (int i = 0; i < cardVessels.Count; i++)
        {
            cardVessels[i].GetComponent<RectTransform>().anchoredPosition = padding + interval * new Vector2(i % cardsPerRow, -i / cardsPerRow);
        }
    }


    public void HideDisplay()
    {
        foreach (GameObject display in displayScreen)
        {
            display.SetActive(false);
        }
    }
    public void ShowDisplay()
    {
        Refresh();
        foreach(GameObject display in displayScreen)
        { 
            display.SetActive(true);
        }
    }
}
