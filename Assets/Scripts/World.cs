using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public List<Area> EmptyAreas;
    public List<Area> OwnedAreas;

    public static readonly Vector3[] positions = new Vector3[8]
    {
    new Vector3(-10f, 0f, -10f),
    new Vector3(30f, 0f, -10f),
    new Vector3(10f, 0f, -30f),
    new Vector3(10f, 0f, 10f),
    new Vector3(-10f, 0f, 10f),
    new Vector3(-10f, 0f, -30f),
    new Vector3(30f, 0f, 10f),
    new Vector3(30f, 0f, -30f)
    };

    private void Start()
    {
        for (int i = 0; i < EmptyAreas.Count; i++)
        {
            GameObject p = Instantiate(EmptyAreas[i].farmType.Prefab, positions[i], Quaternion.identity);
            p.name = i.ToString();
        }
    }
}

[Serializable]
public class Area
{
    public Farm farmType;
}