using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffect_Attack : CardEffect
{
    public int power;

    public override void OnPlay(EffectInfo info)
    {
        power = info.power;
        FindObjectOfType<BattleManager>().AttackEnemy(info.center, info.power, info.range);
    }
}
