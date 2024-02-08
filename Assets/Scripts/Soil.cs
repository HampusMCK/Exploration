using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class Soil : MonoBehaviour
{
    public GameObject plant;

    PlayerController player;

    [NonSerialized]
    public bool Growing, Harvestable, CoolingDown;

    public int timeToGrow = 12;
    public int coolDown = 5;
    public string state;
    float clock;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (Growing)
        {
            clock += Time.deltaTime;

            if (clock >= timeToGrow)
            {
                Harvestable = true;
                Growing = false;
                clock = 0;
            }
        }

        if (CoolingDown)
        {
            clock += Time.deltaTime;
            if (clock >= coolDown)
            {
                CoolingDown = false;
                clock = 0;
            }
        }

        if (Harvestable)
            state = "Can Be Harvested!";
        else if (Growing)
            state = "Can Be Harvested In: " + (timeToGrow - (int)clock) + "Seconds!";
        else if (CoolingDown)
            state = "Cooling Down! Ready To Plant In: " + (coolDown - (int)clock) + "Seconds!";
        else
            state = "Ready To Be Planted!";
    }

    public void Action()
    {
        if (Growing)
            return;
        else if (Harvestable)
            Harvest();
        else if (CoolingDown)
            return;
        else
            Plant();
    }

    public void Plant()
    {
        if (player.Seeds - 1 < 0)
            return;
        Growing = true;
        player.Seeds--;
    }

    public void Harvest()
    {
        Harvestable = false;
        CoolingDown = true;
        player.Wheat++;
        player.Seeds += 2;
    }
}