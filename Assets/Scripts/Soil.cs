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

    public void Action(GameObject instegator)
    {
        if (Growing)
            return;
        else if (Harvestable)
            Harvest(instegator);
        else if (CoolingDown)
            return;
        else
            Plant(instegator);
    }

    public void Plant(GameObject instegator)
    {
        PlayerController p = instegator.GetComponent<PlayerController>();
        AreaData a = instegator.GetComponent<AreaData>();

        if (p != null)
        {
            if (p.Seeds - 1 < 0)
                return;
            p.Seeds--;
            Growing = true;
        }
        if (a != null)
        {
            if (a.seeds - 1 < 0)
                return;
            a.seeds--;
            Growing = true;
        }
    }

    public void Harvest(GameObject instegator)
    {
        PlayerController p = instegator.GetComponent<PlayerController>();
        AreaData a = instegator.GetComponent<AreaData>();

        Harvestable = false;
        CoolingDown = true;

        if (p != null)
        {
            p.Wheat += player.DamageOnFarm();
            p.Seeds += 2;
            if (p.Tool != null)
                p.Tool.durability -= 0.5f;
        }
        if (a != null)
        {
            a.wheat++;
            a.seeds += 2;
        }
    }
}