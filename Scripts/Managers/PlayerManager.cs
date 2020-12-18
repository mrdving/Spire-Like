using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    //components
    BattleManager battleM;
    //stats
    public int health;
    public int maxHealth;
    public int strength = 0;
    public int endurance = 0;
    public List<StatusEffect> statusEffects;
    //display
    public Slider healthSlider;
    public TextMeshProUGUI healthText;
    //
    public bool ready = false;

    private IEnumerator Initialize()
    {
        ready = false;
        battleM = FindObjectOfType<BattleManager>();
        StartCoroutine(PrepareForBattle());
        yield return null;
    }
    public IEnumerator PrepareForBattle()
    {
        ready = false;
        statusEffects = new List<StatusEffect>();
        strength = 0;
        endurance = 0;
        RefreshHealthDisplay();
        ready = true;
        yield return null;
    }

    public void TakeDamage(int damage)
    {
        health = Mathf.Max(0,health - damage);
        RefreshHealthDisplay();
    }
    public bool IsDead()
    {
        if (health <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void RefreshHealthDisplay()
    {
        healthSlider.value = maxHealth == 0 ? 0 : (float)health / (float)maxHealth;
        healthText.text = health + " / " + maxHealth;
    }


    public void ApplyBuff(StatusEffect buff)
    {
        //check empty
        if (buff.power == 0) return;
        switch (buff.type)
        {
            case StatusEffect.Type.strength:
                strength += buff.power;
                break;
            case StatusEffect.Type.endurance:
                endurance += buff.power;
                break;
            case StatusEffect.Type.weaken:
            case StatusEffect.Type.vulnerable:
            case StatusEffect.Type.poison:
                statusEffects.Add(buff);
                break;
            default:
                break;
        }
    }
    public void TriggerBuff()
    {
        for (int i = statusEffects.Count - 1; i >= 0; i++)
        {
            StatusEffect buff = statusEffects[i];
            //apply buff effect
            switch (statusEffects[i].type)
            {
                case StatusEffect.Type.weaken:
                    buff.power = 0;
                    break;
                case StatusEffect.Type.vulnerable:
                    buff.power--;
                    break;
                case StatusEffect.Type.poison:
                    //Take poison damage, return if dead
                    TakeDamage(buff.power);
                    buff.power--;
                    break;
                default:
                    Debug.Log("Trigger buff error: " + buff.type.ToString());
                    buff.power = 0;
                    break;
            }
            //expire buffs
            if (statusEffects[i].power <= 0) statusEffects.Remove(statusEffects[i]);
        }
    }

}
