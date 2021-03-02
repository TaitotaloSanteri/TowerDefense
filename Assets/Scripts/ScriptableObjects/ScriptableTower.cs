using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ScriptableTower")]
public class ScriptableTower : ScriptableObject
{
    public string towerName;
    public int towerCost;
    public Vector2Int size = Vector2Int.one;
    public BaseTower towerPrefab;
    public float fireRate, damage, rotationSpeed, range;
}
