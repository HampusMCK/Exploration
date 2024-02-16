using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class World : MonoBehaviour
{
    [Header("Areas")]
    public List<Area> Areas;
    public List<Area> OwnedAreas;
    public List<int> areasBought;

    [Header("Store")]
    public List<Tools> ToolsInGame;
    public List<int> toolsBought;

    private static World _world;
    public static World Instance { get { return _world; } }
    public SavedData data;
    private PlayerController player;

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

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();

        if (_world != null && _world != this)
            Destroy(gameObject);
        else
            _world = this;

        foreach (Tools t in ToolsInGame)
        {
            t.durability = t.maxDurability;
        }
    }

    private void Start()
    {
        for (int i = 0; i < Areas.Count; i++)
        {
            GameObject p = Instantiate(Areas[i].farmType.Prefab, positions[i], Quaternion.identity, transform);
            p.name = i.ToString();
        }

        // if (Directory.Exists(Application.persistentDataPath + "/saves/"))
        // {
        //     Instance.data = SaveSystem.Load("First");
        //     Instance.areasBought = Instance.data._boughtAreas;
        //     Instance.toolsBought = Instance.data._tools;
        // }
    }

    private void Update()
    {
        // if (toolsBought.Count < player.tools.Count)
        //     foreach (Tools t in player.tools)
        //         for (int i = 0; i < ToolsInGame.Count; i++)
        //             if (t == ToolsInGame[i])
        //                 toolsBought.Add(i);

        // if (areasBought.Count < OwnedAreas.Count)
        //     foreach (Area a in OwnedAreas)
        //         for (int i = 0; i < Areas.Count; i++)
        //             if (a == Areas[i])
        //                 areasBought.Add(i);
    }
}

[Serializable]
public class Area
{
    public Farm farmType;
}