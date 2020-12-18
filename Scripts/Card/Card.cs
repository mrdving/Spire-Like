using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu]
public class Card  : ScriptableObject
{
    public Image portrait;
    public string description, cardName;
    public bool exhaust = false;
    public int health;
    public string[] playEffects;//name, power, range, other
    public string[] breakEffects;//name, power, range, other


    public void OnPlay(Vector2 center) {
        CardEffectTrigger trigger = FindObjectOfType<CardEffectTrigger>();
        foreach (string effect in playEffects)
        {
            EffectInfo info = GetEffectInfo(effect);
            info.center = center;
            trigger.OnPlay(info);
        }
    }
    public void OnBreak() {
        CardEffectTrigger trigger = FindObjectOfType<CardEffectTrigger>();
        foreach (string effect in breakEffects)
        {
            trigger.OnBreak(GetEffectInfo(effect));
        }
    }

    public EffectInfo GetEffectInfo(string effect) {
        EffectInfo toReturn = new EffectInfo();
        string[] parsedString = effect.Split(',');
        try
        {
            toReturn.effectName = parsedString[0];
            toReturn.power = int.Parse(parsedString[1]);
            //center not defined here
            Array2D.Presets presets;
            System.Enum.TryParse(parsedString[2], out presets);
            Array2D rangeGenerator = new Array2D();
            toReturn.range = rangeGenerator.GetRange(presets);
            toReturn.other = parsedString[3];
        }catch(System.Exception ex)
        {
            Debug.Log(ex);
        }
        return toReturn;
    }
}

public struct EffectInfo
{
    public string effectName;
    public int power;
    public Vector2 center;
    public Array2D[] range;
    public string other;
}

[System.Serializable]
public class Array2D
{
    public float[] floatElement;
    public Array2D(float element0, float element1, float element2, float element3, float element4)
    {
        floatElement = new float[5];
        floatElement[0] = element0;
        floatElement[1] = element1;
        floatElement[2] = element2;
        floatElement[3] = element3;
        floatElement[4] = element4;
    }
    public Array2D() { }

    public enum Presets {   ver3, ver5,
                            hor3, hor5,
                            up, down, left, right,
                            ten3, ten5, cross3, cross5,
                            point, ring, all,}
    

    public Array2D[] GetRange(Presets rangeType)
    {
        Array2D[] toReturn = new Array2D[5];
        switch (rangeType)
        {
            case Presets.ver3:
                toReturn[0] = new Array2D(0, 0, 0, 0, 0);
                toReturn[1] = new Array2D(0, 0, 1, 0, 0);
                toReturn[2] = new Array2D(0, 0, 1, 0, 0);
                toReturn[3] = new Array2D(0, 0, 1, 0, 0);
                toReturn[4] = new Array2D(0, 0, 0, 0, 0);
                break;
            case Presets.ver5:
                toReturn[0] = new Array2D(0, 0, 1, 0, 0);
                toReturn[1] = new Array2D(0, 0, 1, 0, 0);
                toReturn[2] = new Array2D(0, 0, 1, 0, 0);
                toReturn[3] = new Array2D(0, 0, 1, 0, 0);
                toReturn[4] = new Array2D(0, 0, 1, 0, 0);
                break;
            case Presets.hor3:
                toReturn[0] = new Array2D(0, 0, 0, 0, 0);
                toReturn[1] = new Array2D(0, 0, 0, 0, 0);
                toReturn[2] = new Array2D(0, 1, 1, 1, 0);
                toReturn[3] = new Array2D(0, 0, 0, 0, 0);
                toReturn[4] = new Array2D(0, 0, 0, 0, 0);
                break;
            case Presets.hor5:
                toReturn[0] = new Array2D(0, 0, 0, 0, 0);
                toReturn[1] = new Array2D(0, 0, 0, 0, 0);
                toReturn[2] = new Array2D(1, 1, 1, 1, 1);
                toReturn[3] = new Array2D(0, 0, 0, 0, 0);
                toReturn[4] = new Array2D(0, 0, 0, 0, 0);
                break;
            case Presets.up:
                toReturn[0] = new Array2D(0, 0, 1, 0, 0);
                toReturn[1] = new Array2D(0, 0, 1, 0, 0);
                toReturn[2] = new Array2D(0, 0, 1, 0, 0);
                toReturn[3] = new Array2D(0, 0, 0, 0, 0);
                toReturn[4] = new Array2D(0, 0, 0, 0, 0);
                break;
            case Presets.down:
                toReturn[0] = new Array2D(0, 0, 0, 0, 0);
                toReturn[1] = new Array2D(0, 0, 0, 0, 0);
                toReturn[2] = new Array2D(0, 0, 1, 0, 0);
                toReturn[3] = new Array2D(0, 0, 1, 0, 0);
                toReturn[4] = new Array2D(0, 0, 1, 0, 0);
                break;
            case Presets.left:
                toReturn[0] = new Array2D(0, 0, 0, 0, 0);
                toReturn[1] = new Array2D(0, 0, 0, 0, 0);
                toReturn[2] = new Array2D(1, 1, 1, 0, 0);
                toReturn[3] = new Array2D(0, 0, 0, 0, 0);
                toReturn[4] = new Array2D(0, 0, 0, 0, 0);
                break;
            case Presets.right:
                toReturn[0] = new Array2D(0, 0, 0, 0, 0);
                toReturn[1] = new Array2D(0, 0, 0, 0, 0);
                toReturn[2] = new Array2D(0, 0, 1, 1, 1);
                toReturn[3] = new Array2D(0, 0, 0, 0, 0);
                toReturn[4] = new Array2D(0, 0, 0, 0, 0);
                break;
            case Presets.ten3:
                toReturn[0] = new Array2D(0, 0, 0, 0, 0);
                toReturn[1] = new Array2D(0, 0, 1, 0, 0);
                toReturn[2] = new Array2D(0, 1, 1, 1, 0);
                toReturn[3] = new Array2D(0, 0, 1, 0, 0);
                toReturn[4] = new Array2D(0, 0, 0, 0, 0);
                break;
            case Presets.ten5:
                toReturn[0] = new Array2D(0, 0, 1, 0, 0);
                toReturn[1] = new Array2D(0, 0, 1, 0, 0);
                toReturn[2] = new Array2D(1, 1, 1, 1, 1);
                toReturn[3] = new Array2D(0, 0, 1, 0, 0);
                toReturn[4] = new Array2D(0, 0, 1, 0, 0);
                break;
            case Presets.cross3:
                toReturn[0] = new Array2D(0, 0, 0, 0, 0);
                toReturn[1] = new Array2D(0, 1, 0, 1, 0);
                toReturn[2] = new Array2D(0, 0, 1, 0, 0);
                toReturn[3] = new Array2D(0, 1, 0, 1, 0);
                toReturn[4] = new Array2D(0, 0, 0, 0, 0);
                break;
            case Presets.cross5:
                toReturn[0] = new Array2D(1, 0, 0, 0, 1);
                toReturn[1] = new Array2D(0, 1, 0, 1, 0);
                toReturn[2] = new Array2D(0, 0, 1, 0, 0);
                toReturn[3] = new Array2D(0, 1, 0, 1, 0);
                toReturn[4] = new Array2D(1, 0, 0, 0, 1);
                break;
            case Presets.ring:
                toReturn[0] = new Array2D(0, 0, 0, 0, 0);
                toReturn[1] = new Array2D(0, 1, 1, 1, 0);
                toReturn[2] = new Array2D(0, 1, 0, 1, 0);
                toReturn[3] = new Array2D(0, 1, 1, 1, 0);
                toReturn[4] = new Array2D(0, 0, 0, 0, 0);
                break;
            case Presets.all:
                toReturn[0] = new Array2D(1, 1, 1, 1, 1);
                toReturn[1] = new Array2D(1, 1, 1, 1, 1);
                toReturn[2] = new Array2D(1, 1, 1, 1, 1);
                toReturn[3] = new Array2D(1, 1, 1, 1, 1);
                toReturn[4] = new Array2D(1, 1, 1, 1, 1);
                break;
            default:
                toReturn[0] = new Array2D(0, 0, 0, 0, 0);
                toReturn[1] = new Array2D(0, 0, 0, 0, 0);
                toReturn[2] = new Array2D(0, 0, 1, 0, 0);
                toReturn[3] = new Array2D(0, 0, 0, 0, 0);
                toReturn[4] = new Array2D(0, 0, 0, 0, 0);
                break;
        }
        return toReturn;
    }
}
