using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeAni : MonoBehaviour
{
    public enum shakeType { twitch, strike, woble, vibrate, none}
    private int curLevel = 0, secLevel = 0;
    private float curPower = 0, curTime = 0;
    private float secPower = 0, secTime = 0;

    private Vector3 origPos;

    public void Shake(shakeType type)
    {
        switch (type)
        {
            case shakeType.twitch:
                AddShake(2, .2f, .3f);
                break;
            case shakeType.strike:
                AddShake(2, .5f, .3f);
                break;
            case shakeType.woble:
                AddShake(1, .3f, 1f);
                break;
            case shakeType.vibrate:
                AddShake(1, .05f, 1f);
                break;
            default:
                ClearShake();
                break;
        }
    }

    public void ShakeUI(shakeType type)
    {
        switch (type)
        {
            case shakeType.twitch:
                AddShake(2, 2f, .3f);
                break;
            case shakeType.strike:
                AddShake(2, 8f, .3f);
                break;
            case shakeType.woble:
                AddShake(1, 3f, 1f);
                break;
            case shakeType.vibrate:
                AddShake(1, .5f, 1f);
                break;
            default:
                ClearShake();
                break;
        }
    }


    private void AddShake(int level, float power, float duration)
    {
        if(level > curLevel)
        {
            secLevel = curLevel;
            secPower = curPower;
            secTime = curTime;
            curLevel = level;
            curPower = power;
            curTime = duration;
        }
        else if(level == curLevel)
        {
            curLevel = level;
            curPower = power;
            curTime = duration;
        }
        else if(level >= secLevel)
        {
            secLevel = level;
            secPower = power;
            secTime = duration;
        }
    }

    public void ClearShake()
    {
        curLevel = 0;
        curPower = 0;
        curTime = 0;
        secLevel = 0;
        secPower = 0;
        secTime = 0;
    }

    private void Start()
    {
        ResetOriginalPosition();
    }

    public void ResetOriginalPosition()
    {
        origPos = transform.position;
    }

    private void Update()
    {
        //shake it
        if(curLevel > 0)
        {
            transform.position = origPos + Random.insideUnitSphere * curPower;
            curTime -= Time.deltaTime;
            secTime -= Time.deltaTime;
        }
        //end current shake
        if(curTime < 0)
        {
            //change to secondary
            if (secLevel > 0)
            {
                curLevel = secLevel;
                curPower = secPower;
                curTime = secTime;
                secLevel = 0;
                secPower = 0;
                secTime = 0;
            }
            else
            {
                //return to original position;
                curTime = 0;
                transform.position = origPos;
            }
        }
    }

}
