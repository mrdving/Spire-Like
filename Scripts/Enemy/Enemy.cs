using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class Enemy : ScriptableObject
{
    public Image portrait;
    public string description, enemyName;
    public int maxHealth;
    public Vector3[] attacks;//damage,time, buff/debuff index
    

}
