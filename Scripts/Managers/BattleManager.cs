using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleManager : MonoBehaviour
{
    //components
    private PileManager pile;
    private HandManager hand;
    private EnemyManager enemy;
    private RewardManager reward;
    private PlayerManager player;
    //phases
    public enum BattlePhase { Init, Begin, Draw, Play, Discard, Enemy, Win, Lose}
    public BattlePhase curPhase = BattlePhase.Init;
    //stats
    public int handSize;
    //display
    public Button endTurn;
    public Text phaseDisplay;
    //
    public bool ready = false;
    //functions
    private void Start()
    {
        StartCoroutine(Initialize());
    }
    private IEnumerator Initialize()
    {
        ready = false;
        pile = GetComponent<PileManager>();
        hand = GetComponent<HandManager>();
        enemy = GetComponent<EnemyManager>();
        reward = GetComponent<RewardManager>();
        player = GetComponent<PlayerManager>();
        //wait for initialization
        while (!pile.ready) yield return null;
        while (!hand.ready) yield return null;
        while (!enemy.ready) yield return null;
        while (!reward.ready) yield return null;
        while (!player.ready) yield return null;
        curPhase = BattlePhase.Begin;
        ready = true;
    }
    
    public IEnumerator Begin(EnemyEncounter enemyEncounter)
    {
        enemy.PrepareForBattle(enemyEncounter);
        while (!enemy.ready) yield return null;
        StartCoroutine(pile.PrepareForBattle());
        while (!pile.ready) yield return null;
        StartCoroutine(player.PrepareForBattle());
        while (!player.ready) yield return null;
        curPhase = BattlePhase.Begin;
        NextPhase();
    }

    private IEnumerator Draw()
    {
        pile.Draw(handSize);
        yield return new WaitForSeconds(.5f);
        hand.SetInteractable(true);
        NextPhase();
    }

    private IEnumerator Play()
    {
        endTurn.interactable = true;
        yield return null;
    }
    public void EndTurn()
    {
        endTurn.interactable = false;
        if (curPhase == BattlePhase.Play)
        {
            hand.SetInteractable(false);
            NextPhase();
        }
    }

    private IEnumerator Enemy()
    {
        //yield return new WaitUntil(() => EnemyManager.Attack());
        StartCoroutine(enemy.CallAttack());
        while(enemy.attacking)yield return null;
        yield return new WaitForSeconds(.5f);
        enemy.TriggerBuff();
        yield return new WaitForSeconds(.2f);
        NextPhase();
    }

    private IEnumerator Discard()
    {
        pile.DiscardAll();
        yield return new WaitForEndOfFrame();
        NextPhase();
    }

    public IEnumerator Lose()
    {
        curPhase = BattlePhase.Lose;
        Debug.Log("GameOver ;<");
        RefreshDisplay();
        hand.SetInteractable(false);
        yield return null;
    }
    public IEnumerator Win()
    {
        curPhase = BattlePhase.Win;
        Debug.Log("Battle won");
        RefreshDisplay();
        hand.SetInteractable(false);
        yield return new WaitForSeconds(1f);
        //show reward
        reward.LoadReward(RewardManager.RewardType.better);
        while (!reward.ready) yield return null;
        reward.ShowRewardScreen();
        yield return null;
    }

    public void TakeDamage(int damage)
    {
        player.TakeDamage(hand.TakeDamage(damage));
        CheckHealth();
    }
    private void CheckHealth()
    {
        if (player.IsDead())
        {
            curPhase = BattlePhase.Lose;
            StartCoroutine(Lose());
        }
    }

    public void AttackEnemy(Vector2 center, int attack, Array2D[] range)
    {
        //apply player buff
        attack += player.strength;
        enemy.AddDamage(center, attack, range);
    }
    public void ApplyEnemyBuff(StatusEffect buff , Vector2 center, Array2D[] range)
    {
        enemy.ApplyBuff(buff, center, range);
    }
    public void ApplyPlayerBuff(StatusEffect buff)
    {
        player.ApplyBuff(buff);
    }

    private void NextPhase()
    {
        switch (curPhase)
        {
            case BattlePhase.Begin:
                curPhase = BattlePhase.Draw;
                StartCoroutine(Draw());
                break;
            case BattlePhase.Draw:
                curPhase = BattlePhase.Play;
                StartCoroutine(Play());
                break;
            case BattlePhase.Play:
                curPhase = BattlePhase.Enemy;
                StartCoroutine(Enemy());
                break;
            case BattlePhase.Enemy:
                curPhase = BattlePhase.Discard;
                StartCoroutine(Discard());
                break;
            case BattlePhase.Discard:
                curPhase = BattlePhase.Draw;
                StartCoroutine(Draw());
                break;
            default:
                Debug.Log("You shouldn't be here, Phase : " + curPhase);
                break;
        }
        RefreshDisplay();
    }
    public void RefreshDisplay()
    {
        phaseDisplay.text = curPhase.ToString();
    }
}
