using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyVessel : MonoBehaviour
{
    //component
    private ShakeAni shake;
    public Enemy enemy;
    //display
    public Image portrait;
    public int health, maxHealth, strength, endurance;
    public TextMeshProUGUI atkTxt, healthTxt, damageTxt;
    public Slider healthSlider;
    public List<BuffVessel> buffVessels = new List<BuffVessel>();
    public Vector2 buffDisplayInterval;
    //attacking
    public int attackSequence = 0;
    public Vector3[] attacks;
    //status effect
    public Transform statusEffectCanvas;
    public List<StatusEffect> statusEffects;
    //other
    public bool ready = false;
    public Enemy emptyEnemy;
    public GameObject buffVesselPrefab;

    private void Start()
    {
        shake = GetComponent<ShakeAni>();
        InsertEnemy(enemy);
        ready = true;
    }

    public void InsertEnemy(Enemy newEnemy)
    {
        //reallocate enemy
        enemy = newEnemy;
        //initialize stats
        health = enemy.maxHealth;
        maxHealth = enemy.maxHealth;
        attacks = enemy.attacks;
        damageTxt.text = "";
        strength = 0;
        endurance = 0;
        attackSequence = 0;
        statusEffects = new List<StatusEffect>();
        //portrait = newEnemy.portrait;

        Refresh();

    }

    public bool CallAttack()
    {
        if (enemy == emptyEnemy) return false;
        Attack();
        Refresh();
        return true;
    }

    public bool CheckDead()
    {
        bool dead = false;
        //check dead
        if (health <= 0)
        {
            health = 0;
            dead = true;
        }
        else
        {
            dead = false;
        }
        return dead;
    }
    public bool TakeDamage(int damage)
    {
        if (damage == 0 || enemy == emptyEnemy) return false;
        
        //take damage
        health -= damage;
        
        //refresh display
        Refresh();
        //animation
        shake.Shake(ShakeAni.shakeType.strike);
        //check dead
        return CheckDead();

    }
    public void ApplyBuff(StatusEffect buff)
    {
        //check empty
        if (buff.power == 0 || enemy == emptyEnemy) return;
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
                bool foundSameStat = false;
                foreach(StatusEffect stat in statusEffects)
                {
                    if(stat.type == buff.type)
                    {
                        stat.power += buff.power;
                        foundSameStat = true;
                    }
                }
                if(!foundSameStat)statusEffects.Add(buff);
                break;
            default:
                break;
        }
        Refresh();
    }
    public bool TriggerBuff()
    {
        for(int i = statusEffects.Count - 1; i >= 0; i--)
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
        return CheckDead();
    }

    public void Refresh()
    {
        RefreshEnemyDisplay();
        RefreshStatusEffectDisplay();
    }
    private void RefreshEnemyDisplay()
    {
        if (enemy == emptyEnemy)
        {
            atkTxt.text = "";
            healthTxt.text = "";
            healthSlider.gameObject.SetActive(false);
        }
        else
        {
            atkTxt.text = attacks[attackSequence].x + "x" + attacks[attackSequence].y;
            healthTxt.text = health.ToString() + "/" + maxHealth.ToString();
            healthSlider.gameObject.SetActive(true);
            if (maxHealth == 0) healthSlider.value = 0;
            else healthSlider.value = (float)health / (float)maxHealth;
        }
    }
    
    private void RefreshStatusEffectDisplay()
    {
        //check buff vessel count
        while(buffVessels.Count < statusEffects.Count)
        {
            BuffVessel tmp = Instantiate(buffVesselPrefab, statusEffectCanvas).GetComponent<BuffVessel>();
            buffVessels.Add(tmp);
        }
        while(buffVessels.Count > statusEffects.Count)
        {
            GameObject tmp = buffVessels[0].gameObject;
            buffVessels.Remove(buffVessels[0]);
            Destroy(tmp);
        }
        //refresh location
        for (int i = 0; i < buffVessels.Count; i++)
        {
            buffVessels[i].GetComponent<RectTransform>().position = statusEffectCanvas.position + (Vector3)(new Vector2(i/3, i%3) * buffDisplayInterval);
        }
        //Refresh content
        for (int i = 0; i < statusEffects.Count; i++)
        {
            buffVessels[i].buff = statusEffects[i];
            buffVessels[i].Refresh();
        }
    }

    
    public virtual void Attack()
    {
        int damage = (int)(attacks[attackSequence].x * attacks[attackSequence].y);
        FindObjectOfType<BattleManager>().TakeDamage(damage);
        //Next attack
        attackSequence++;
        if (attackSequence >= attacks.Length) attackSequence = 0;
        //aimation
        shake.Shake(ShakeAni.shakeType.twitch);
    }
}
