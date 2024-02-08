using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Farm", menuName = "Exploration/Areas/Farm")]
public class Farm : ScriptableObject
{
    public GameObject Prefab;
    public Vector2 plantationSize;
    public int cost;
    public string[] animals;

}