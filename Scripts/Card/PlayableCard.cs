using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayableCard : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    //manager
    private PileManager pile;
    private HandManager hand;
    private EnemyManager enemy;
    //self
    private RectTransform cardTrans;
    private Collider2D cardCol;
    private Follow_UI follow;
    private RectTransform origTarget;
    public Card card;
    //other
    private Camera cam;
    public Collider2D playField;

    //state
    public bool ready = false;
    public bool interactable = false;
    public bool dragging = false;
    public bool broken = false;

    //card stat
    public int def;
    public int atk;
    public Array2D[] range = new Array2D[5];

    //location
    public Vector2 center;
    
    

    private void Start()
    {
        //manager
        pile = FindObjectOfType<PileManager>();
        hand = FindObjectOfType<HandManager>();
        enemy = FindObjectOfType<EnemyManager>();
        //self
        cardTrans = GetComponent<RectTransform>();
        cardCol = GetComponent<Collider2D>();
        follow = GetComponent<Follow_UI>();
        origTarget = follow.followTarget;
        //other
        cam = FindObjectOfType<Camera>();
        ready = true;
    }

    private void Update()
    {
        if (!dragging) return;
        //transform to world point
        Vector2 position = cam.ScreenToWorldPoint(Input.mousePosition);
        if (playField.OverlapPoint(position))
        {
            enemy.ShowRange(position, range, atk);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!interactable) return;
        follow.followTarget = FindObjectOfType<Follow_mouse>().rectTransform;
        follow.snap = true;
        dragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;
        if (!interactable)
        {
            follow.followTarget = origTarget;
            follow.snap = false;
            return;
        }

        Vector2 center = enemy.ShowRange(cam.ScreenToWorldPoint(Input.mousePosition), range, atk);
        if (center.x >= 0)
        {
            //play effect
            card.OnPlay(center);
            //call card removal
            hand.RemoveCard(this);
        }
        else
        {
            
        }
        follow.followTarget = origTarget;
        follow.snap = false;
        enemy.ClearRange();

    }

    public int TakeDamage(int damage)
    {
        ShakeAni shake = follow.followTarget.GetComponent<ShakeAni>();
        shake.ResetOriginalPosition();
        if (def >= damage)
        {
            shake.ShakeUI(ShakeAni.shakeType.twitch);
            def -= damage;
            GetComponent<CardVessel>().RefreshInBattleDisplay(this);
            return 0;    //damage blocked
        }
        else
        {
            damage -= def;
            shake.ShakeUI(ShakeAni.shakeType.strike);
            def = 0;
            broken = true;
            GetComponent<CardVessel>().RefreshInBattleDisplay(this);
            card.OnBreak();
            return damage;       //damage not blocked, pass down the damage
        }
    }

    public void RefreshFollow()
    {
        origTarget = follow.followTarget;
    }

    public void RefreshStats()
    {
        EffectInfo info = card.GetEffectInfo(card.playEffects[0]);
        atk = info.power;
        def = card.health;
        range = info.range;
        broken = false;
    }
}
