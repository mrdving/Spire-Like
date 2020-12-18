using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CardLibrary : MonoBehaviour
{
    public Dictionary<string, Card> cardDic = new Dictionary<string, Card>();
    
    string path ;
    public bool ready = false;
    public List<string> basic, common, rare, legend, special;
    public enum Rarity { basic, common, rare, legend, special}

    public GameObject cardEffectContainer;

    private void Start()
    {
        LoadEffects();
        StartCoroutine(Initialize());        
    }
    IEnumerator Initialize()
    {
        path = "Cards/Basic";
        LoadCards(basic, path);
        yield return null;
        path = "Cards/Common";
        LoadCards(common, path);
        yield return null;
        path = "Cards/Rare";
        LoadCards(rare, path);
        yield return null;
        path = "Cards/Legend";
        LoadCards(legend, path);
        yield return null;
        path = "Cards/Special";
        LoadCards(special, path);
        yield return null;
        ready = true;
    }

    private void LoadCards(List<string> keyType, string path)
    {
        Card[] cards = Resources.LoadAll<Card>(path);
        foreach (Card card in cards)
        {
            cardDic.Add(card.name, card);
            keyType.Add(card.name);
        }
    }
    private void LoadEffects()
    {
        //find files
        path = "./Assets/Resources/CardEffects";
        DirectoryInfo dir = new DirectoryInfo(path);
        FileInfo[] file = dir.GetFiles(".");
        //load files as components and add to cardEffectContainer
        foreach(FileInfo f in file)
        {
            string[] compType = f.Name.Split('.');
            try
            {
                System.Type type = System.Type.GetType(compType[0]);
                if (cardEffectContainer.GetComponent(type) == null)
                {
                    //insert type
                    cardEffectContainer.AddComponent(type);
                }
            }
            catch(System.Exception ex)
            {
                Debug.Log("Error " + ex, this);
            }
        }
    }

    public Card GetRandomCard(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.basic:
                return cardDic[basic[(int)(Random.value * basic.Count)]];
                break;
            case Rarity.common:
                return cardDic[common[(int)(Random.value * common.Count)]];
                break;
            case Rarity.rare:
                return cardDic[rare[(int)(Random.value * rare.Count)]];
                break;
            case Rarity.legend:
                return cardDic[legend[(int)(Random.value * legend.Count)]];
                break;
            case Rarity.special:
                return cardDic[special[(int)(Random.value * special.Count)]];
                break;
            default:
                Debug.Log("Error getting card", this);
                return null;
                break;
        }
        
    }

}
