using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffectTrigger : MonoBehaviour
{
    public void OnPlay(EffectInfo info) {
        string effectName = "CardEffect_" + info.effectName;
        CardEffect toTrigger = FindObjectOfType(System.Type.GetType(effectName)) as CardEffect;
        toTrigger.OnPlay(info);
    }
    public void OnBreak(EffectInfo info)
    {
        string effectName = "CardEffect_" + info.effectName;
        CardEffect toTrigger = FindObjectOfType(System.Type.GetType(effectName)) as CardEffect;
        toTrigger.OnBreak(info);
    }
}
