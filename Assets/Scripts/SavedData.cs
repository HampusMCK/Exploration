using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SavedData
{
    public string worldName = "First";

    //World Data
    public List<int> _boughtAreas;

    //Player Data
    public List<int> _tools = new();
    public int _money;
    public int _wheat;
    public int _seeds;

    public SavedData(string name)
    {
        worldName = name;
    }

    public SavedData(SavedData s)
    {
        worldName = s.worldName;
        _boughtAreas = s._boughtAreas;
        _tools = s._tools;
        _money = s._money;
        _wheat = s._wheat;
        _seeds = s._seeds;
    }

    public void getDataFromWorld(List<int> boughtAreas, List<int> boughtTools)
    {
        _boughtAreas = boughtAreas;
        _tools = boughtTools;
    }

    public void getDataFromPlayer(int money, int wheat, int seeds)
    {
        _money = money;
        _wheat = wheat;
        _seeds = seeds;
    }
}