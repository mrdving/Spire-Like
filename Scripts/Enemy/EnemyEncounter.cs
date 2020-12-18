using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyEncounter : ScriptableObject
{
    public Enemy[] enemies;
    public bool randomPlacement;
    public int[] position;
}
