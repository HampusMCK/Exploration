using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;

public class AreaData : MonoBehaviour
{
    [Header("Area Data")]
    public Farm type;
    World world;
    PlayerController player;

    [Header("Object Reference")]
    public GameObject barier;
    public GameObject AutoFarmingSign;
    public GameObject InventoryUI;
    public TMP_Text seedsTXT;
    public TMP_Text wheatTXT;
    public TMP_InputField SeedsToDeposit;

    [Header("Soil")]
    public List<Soil> farmSpots;

    List<Vector3> corners = new List<Vector3>();
    Vector3[] verts;

    [Header("Inventory")]
    public int seeds, wheat;

    bool bought;
    bool autoFarming;
    public bool showInventory;

    private void Awake()
    {
        world = GameObject.Find("World").GetComponent<World>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void Start()
    {
        corners.Add(new Vector3(transform.position.x, transform.position.y, transform.position.z));
        corners.Add(new Vector3(transform.position.x - 20, transform.position.y, transform.position.z));
        corners.Add(new Vector3(transform.position.x, transform.position.y, transform.position.z + 20));
        corners.Add(new Vector3(transform.position.x - 20, transform.position.y, transform.position.z + 20));
        verts = corners.ToArray();
        if (name != "Main")
        {
            int.TryParse(name, out int i);
            type = world.Areas[i].farmType;
        }
    }

    private void Update()
    {
        if (!bought)
            foreach (Area a in world.OwnedAreas)
                if (a.farmType == type)
                    bought = true;

        if (bought)
        {
            if (barier.activeSelf)
                barier.SetActive(false);

            if (farmSpots.Count < type.plantationSize.x * type.plantationSize.y)
                placeFarm();
        }

        if (autoFarming)
        {
            Debug.Log(bought);
            foreach (Soil s in farmSpots)
            {
                s.Action(this.gameObject);
            }
        }

        if (showInventory)
        {
            InventoryUI.SetActive(true);
            seedsTXT.text = seeds.ToString();
            wheatTXT.text = wheat.ToString();
        }
    }

    void placeFarm()
    {
        for (int x = (int)verts[0].x - 1; (x - ((int)verts[0].x - 1)) * -1 < type.plantationSize.x * 2; x -= 2)
            for (int z = (int)verts[0].z + 1; z - ((int)verts[0].z + 1) < type.plantationSize.y * 2; z += 2)
            {
                GameObject g = Instantiate(type.farmDirt, new Vector3(x, 1, z), Quaternion.identity, transform);
                g.name = "Area: " + name + ", slot: " + ((x - ((int)verts[0].x - 1)) * -1 / 2).ToString() + ", " + ((z - ((int)verts[0].z + 1)) / 2).ToString();
                farmSpots.Add(g.GetComponent<Soil>());
            }
    }

    public void closeInventoryUI()
    {
        showInventory = false;
        InventoryUI.SetActive(false);
        player.ViewAreaInventory = false;
    }

    public void withdrawWheat()
    {
        player.Wheat += wheat;
        wheat = 0;
    }

    public void depositSeeds()
    {
        int amount = 0;
        int.TryParse(SeedsToDeposit.text, out amount);
        if (player.Seeds - amount > -1)
        {
            seeds += amount;
            player.Seeds -= amount;
        }
    }

    public void ActivateAutoFarming()
    {
        autoFarming = true;
        AutoFarmingSign.SetActive(false);
    }
}