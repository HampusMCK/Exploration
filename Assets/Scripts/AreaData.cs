using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AreaData : MonoBehaviour
{
    public Farm type;
    World world;
    PlayerController player;
    public GameObject barier;

    bool bought;

    private void Awake()
    {
        world = GameObject.Find("World").GetComponent<World>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void Start()
    {
        if (name != "Main")
        {
            int.TryParse(name, out int i);
            type = world.EmptyAreas[i].farmType;
        }
    }

    private void Update()
    {
        if (!bought)
            foreach (Area a in world.OwnedAreas)
                if (a.farmType == type)
                    bought = true;

        if (bought)
            barier.SetActive(false);
    }
}
