using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Soil : MonoBehaviour
{
    public GameObject plant;
    public GameObject stateText;

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

    private void FixedUpdate()
    {
        if (stateText.activeSelf)
            stateText.transform.LookAt(Camera.main.transform);
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

        if (stateText.activeSelf)
        {
            if (Harvestable)
                state = "Can Be Harvested!";
            else if (Growing)
                state = "Can Be Harvested In:\n" + (timeToGrow - (int)clock) + " Seconds!";
            else if (CoolingDown)
                state = "Cooling Down! Ready To Plant In:\n" + (coolDown - (int)clock) + " Seconds!";
            else
                state = "Ready To Be Planted!";
        }
        stateText.GetComponentInChildren<TMP_Text>().text = state;

        if (player.soil != this)
            stateText.SetActive(false);
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
            Plant(0);
    }

    public void Plant(int actor)
    {
        if (actor == 0)
        {
            if (player.Seeds - 1 < 0)
                return;
            player.Seeds--;
        }
        Growing = true;
    }

    public void Harvest()
    {
        Harvestable = false;
        CoolingDown = true;
        player.Wheat += player.DamageOnFarm();
        player.Seeds += 2;
        if (player.Tool != null)
            player.Tool.durability -= 0.5f;
    }
}