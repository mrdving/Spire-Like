using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CardVessel : MonoBehaviour
{
    public Card card;
    public Image portrait;
    public TextMeshProUGUI atk, def, cardName, description;

    private void Start()
    {
        Refresh();            
    }
    public void Refresh()
    {
        if (card == null) return;
        //portrait = card.portrait;
        cardName.text = card.cardName;

        EffectInfo info = card.GetEffectInfo(card.playEffects[0]);
        atk.text = info.power.ToString();
        def.text = card.health.ToString();
        description.text = card.description;
    }

    public void RefreshInBattleDisplay(PlayableCard playableCard)
    {
        atk.text = playableCard.atk.ToString();
        def.text = playableCard.def.ToString();
    }

    public void AddThisCard()
    {
        FindObjectOfType<DeckManager>().AddCard(card);
    }
    public void RemoveThisCard()
    {
        FindObjectOfType<DeckManager>().RemoveCard(card);
    }
    public void PickReward()
    {
        AddThisCard();
        FindObjectOfType<LevelManager>().RefreshAvailablePath();
        FindObjectOfType<LevelManager>().ShowMap();
        FindObjectOfType<RewardManager>().HideRewardScreen();
    }
}
