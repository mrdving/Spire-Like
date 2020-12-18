using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardEffect : MonoBehaviour
{
    public virtual void OnPlay(EffectInfo info) { }

    public virtual void OnBreak(EffectInfo info) { }
}
