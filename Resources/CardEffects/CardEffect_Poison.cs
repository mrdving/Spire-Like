using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffect_Poison : CardEffect
{
    StatusEffect poison;

    public override void OnPlay(EffectInfo info)
    {
        poison = new StatusEffect(StatusEffect.Type.poison, info.power);
        FindObjectOfType<EnemyManager>().ApplyBuff(poison, info.center, info.range);
    }
}
