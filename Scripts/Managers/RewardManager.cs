using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    //components
    private CardLibrary lib;
    public int choiceNum = 3;
    public CardVessel[] cardVessels;
    public GameObject[] rewardScreen;
    //adjustment variables
    public int level;
    //data holders
    public enum RewardType { allBasic, normal, better, allLegend }
    private float[] chances = new float[4];//(basic, common, rare, legend)
    private CardLibrary.Rarity[] rarity = new CardLibrary.Rarity[5];//which rarity to pick
    //states
    public bool ready = false;
    //functions
    private void Start()
    {
        StartCoroutine(Initialize());
    }
    IEnumerator Initialize()
    {
        lib = FindObjectOfType<CardLibrary>();
        HideRewardScreen();
        yield return null;
        ready = true;
    }

    public void LoadReward(RewardType type)
    {
        ready = false;
        //load chances
        GetChanceArray(type);
        //load rarity
        GetRarityArray();
        //get cards
        for(int i = 0; i < cardVessels.Length; i++)
        {
            if(i < choiceNum)
            {
                cardVessels[i].gameObject.SetActive(true);
                cardVessels[i].card = lib.GetRandomCard(rarity[i]);
                cardVessels[i].Refresh();
            }
            else
            {
                cardVessels[i].gameObject.SetActive(false);
            }
        }
        ready = true;
    }
    private void GetChanceArray(RewardType type)
    {
        switch (type)
        {
            case RewardType.allBasic:
                chances[0] = 1; //basic
                chances[1] = 0; //common
                chances[2] = 0; //rare
                chances[3] = 0; //legend
                break;
            case RewardType.normal:
                chances[0] = 0; //basic
                chances[1] = .7f; //common
                chances[2] = .9f; //rare
                chances[3] = 1f; //legend
                break;
            case RewardType.better:
                chances[0] = 0; //basic
                chances[1] = .2f; //common
                chances[2] = .8f; //rare
                chances[3] = 1f; //legend
                break;
            case RewardType.allLegend:
                chances[0] = 0; //basic
                chances[1] = 0; //common
                chances[2] = 0; //rare
                chances[3] = 1; //legend
                break;
            default:
                chances[0] = 0; //basic
                chances[1] = 1; //common
                chances[2] = 0; //rare
                chances[3] = 0; //legend
                break;
        }
        //Debug.Log(chances[0] + "," + chances[1] + "," + chances[2] + "," + chances[3]);
    }
    private void GetRarityArray()
    {
        for (int i = 0; i < choiceNum; i++)
        {
            float value = Random.value;
            
            if (value < chances[0]) {                
                rarity[i] = CardLibrary.Rarity.basic;
            }
            else if(value < chances[1])
            {
                rarity[i] = CardLibrary.Rarity.common;
            }
            else if (value < chances[2])
            {
                rarity[i] = CardLibrary.Rarity.rare;
            }
            else if (value < chances[3])
            {
                rarity[i] = CardLibrary.Rarity.legend;
            }
            else
            {
                Debug.Log("WTF");
                rarity[i] = CardLibrary.Rarity.common;
            }
            //Debug.Log(value.ToString() + rarity[i]);
        }
    }

    public void ShowRewardScreen()
    {
        foreach(GameObject obj in rewardScreen)
        {
            obj.SetActive(true);
        }
    }
    public void HideRewardScreen()
    {
        foreach(GameObject obj in rewardScreen){
            obj.SetActive(false);
        }
    }

    public void Test()
    {
        LoadReward(RewardType.normal);
    }
}
