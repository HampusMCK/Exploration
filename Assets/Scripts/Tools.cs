using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tools", menuName = "Exploration/Tools")]
public class Tools : ScriptableObject
{
    [Header("String Data")]
    public string toolName;
    public string type;
    public string description;

    [Header("Active Data")]
    public int level;
    public int cost;
    public float durability;
    public float maxDurability;

    [Header("Mesh Data")]
    public Mesh Mesh;
    public List<Material> Mats;
    public Sprite HotbarTexture;
}
