using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public int[][] damageCounter;
    public EnemyVessel[] enemyVesselsConstructor;
    public EnemyVessel[][] enemyVessels;
    public int slotDim = 3;
    public bool attacking = false;
    private Camera cam;
    public Enemy emptyEnemy;
    public bool ready = false;

    private void Start()
    {
        StartCoroutine(Initialize());
    }
    IEnumerator Initialize()
    {
        cam = FindObjectOfType<Camera>();
        //init damage conter
        damageCounter = new int[slotDim][];
        for (int i = 0; i < slotDim; i++) damageCounter[i] = new int[slotDim];
        //init enemyVessel
        enemyVessels = new EnemyVessel[slotDim][];
        for (int i = 0; i < slotDim; i++) enemyVessels[i] = new EnemyVessel[slotDim];
        //construct enemy vessel
        for (int i = 0; i < enemyVessels.Length; i++)
        {
            for(int k = 0; k < enemyVessels[i].Length; k++)
            {
                enemyVessels[i][k] = enemyVesselsConstructor[i * 3 + k];
            }
        }
        ClearDamageCounter();
        ready = true;
        yield return null;
    }
    public void PrepareForBattle(EnemyEncounter enemyEncounter)
    {
        ready = false;
        ClearBattleField();
        if (enemyEncounter.randomPlacement)
        {
            //initialize available spots (0 - 8)
            List<int> available = new List<int>();
            for(int i = 0; i < slotDim*slotDim; i++)
            {
                available.Add(i);
            }
            //pick random spots and insert
            for(int i = 0; i < enemyEncounter.enemies.Length; i++)
            {
                int picked = available[(int)(Random.value * available.Count)];
                available.Remove(picked);
                enemyVessels[picked/ slotDim][picked % slotDim].InsertEnemy(enemyEncounter.enemies[i]);
            }
        }
        else
        {
            for (int i = 0; i < enemyEncounter.enemies.Length; i++)
            {
                enemyVessels[enemyEncounter.position[i] / 3][enemyEncounter.position[i] % 3].InsertEnemy(enemyEncounter.enemies[i]);
            }
        }
        ready = true;
    }
    public void ClearBattleField()
    {
        for(int i = 0; i < slotDim; i++)
        {
            for(int k = 0; k < slotDim; k++)
            {
                enemyVessels[i][k].InsertEnemy(emptyEnemy);
            }
        }
    }
   

    public Vector2 ShowRange(Vector2 position, Array2D[] range, int power) {
        
        //assign the overlapping vessel as center
        Vector2 center = new Vector2(-1,-1);
        for(int i = 0; i < slotDim; i++)
        {
            for(int k = 0; k < slotDim; k++)
            {
                if (enemyVessels[i][k].GetComponent<Collider2D>().OverlapPoint(position))
                {
                    center = new Vector2(i, k);
                }
            }
        }
        //show range
        for (int i = 0; i < slotDim; i++)
        {
            for (int k = 0; k < slotDim; k++)
            {
                if (center.x < 0) {
                    //out of playfield
                    enemyVessels[i][k].damageTxt.text = "";
                    enemyVessels[i][k].GetComponent<SpriteRenderer>().enabled = true;
                    break;
                }

                float weightAtPos = range[i - (int)center.x + 2].floatElement[k - (int)center.y + 2];
                if (weightAtPos > 0) {
                    //in range
                    enemyVessels[i][k].damageTxt.text = (weightAtPos * power).ToString();
                    enemyVessels[i][k].GetComponent<SpriteRenderer>().enabled = false;
                        }
                else
                {
                    //out of range
                    enemyVessels[i][k].damageTxt.text = "";
                    enemyVessels[i][k].GetComponent<SpriteRenderer>().enabled = true;
                }
            }
        }
        return center;
    }
    public void ClearRange()
    {
        for (int i = 0; i < slotDim; i++)
        {
            for (int k = 0; k < slotDim; k++)
            {
                enemyVessels[i][k].damageTxt.text = "";
                enemyVessels[i][k].GetComponent<SpriteRenderer>().enabled = true;
            }
        }
    }

    public void AddDamage(Vector2 center, int damage, Array2D[] range)
    {
        int distrRadius = (range.Length - 1) / 2;
        
        for(int i = 0; i < slotDim; i++)
        {
            for (int k = 0; k < slotDim; k++)
            {
                float weight = range[i - (int)center.x + distrRadius].floatElement[k - (int)center.y + distrRadius];
                damageCounter[i][k] += (int)(damage * weight);
                
            }
        }
        DistributeDamage();
    }
    public void DistributeDamage() {
        for (int i = 0; i < slotDim; i++)
        {
            for (int k = 0; k < slotDim; k++) {
                //apply damage
                if (enemyVessels[i][k].TakeDamage(damageCounter[i][k]))
                {
                    //check dead
                    Debug.Log(enemyVessels[i][k] + "dead");
                    //clear corpse
                    enemyVessels[i][k].InsertEnemy(emptyEnemy);
                }
            }
        }
        //clear damage counter
        ClearDamageCounter();
        //check win state
        CheckWinState();
    }
    private void ClearDamageCounter()
    {
        for (int i = 0; i < damageCounter.Length; i++)
        {
            for (int k = 0; k < damageCounter.Length; k++)
            {
                damageCounter[i][k] = 0;
            }
        }
    }
    public void ApplyBuff(StatusEffect buff, Vector2 center, Array2D[] range)
    {
        int distrRadius = (range.Length - 1) / 2;

        for (int i = 0; i < slotDim; i++)
        {
            for (int k = 0; k < slotDim; k++)
            {
                float weight = range[i - (int)center.x + distrRadius].floatElement[k - (int)center.y + distrRadius];
                if (weight <= 0) continue;
                enemyVessels[i][k].ApplyBuff(new StatusEffect(buff.type, (int)(buff.power * weight)));
            }
        }
    }
    public void TriggerBuff()
    {
        for (int i = 0; i < slotDim; i++)
        {
            for (int k = 0; k < slotDim; k++)
            {
                //trigger effects
                if (enemyVessels[i][k].TriggerBuff())
                {
                    //declare dead
                    Debug.Log(enemyVessels[i][k] + "dead");
                    //clear corpse
                    enemyVessels[i][k].InsertEnemy(emptyEnemy);
                }
            }
        }
    }

    public IEnumerator CallAttack()
    {
        attacking = true;
        for (int i = 0; i < slotDim; i++)
        {
            for (int k = 0; k < slotDim; k++)
            {
                if (enemyVessels[i][k].CallAttack())
                {
                    yield return new WaitForSeconds(0.2f);
                }
            }
        }
        attacking = false;

    }

    private void CheckWinState()
    {
        for (int i = 0; i < slotDim; i++)
        {
            for (int k = 0; k < slotDim; k++)
            {
                if (enemyVessels[i][k].enemy != emptyEnemy) return;
            }
        }
        //win
        StartCoroutine(FindObjectOfType<BattleManager>().Win());
    }
}
