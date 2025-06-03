using System.Collections.Generic;
using Resources.Features.Model.Units;
using UnityEngine;

public class gameState
{
    public bool IsHead
    {
        get => isHead;
        set => isHead = value;
    }

    public List<IUnit> AllUnits
    {
        get => allUnits;
        set => allUnits = value;
    }

    public List<Province> AllOwnedProvinces
    {
        get => allOwnedProvinces;
        set => allOwnedProvinces = value;
    }

    public Nation NationOneState
    {
        get => nationOneState;
        set => nationOneState = value;
    }

    public Nation NationTwoState
    {
        get => nationTwoState;
        set => nationTwoState = value;
    }


    private bool isHead;
    private List<IUnit> allUnits = new List<IUnit>();
    private List<Province> allOwnedProvinces = new List<Province>();
    private Nation nationOneState;
    private Nation nationTwoState;

    public gameState(List<IUnit> units, List<Province> provinces, Nation nationOne, Nation nationTwo)
    {
        allUnits = units;
        allOwnedProvinces = provinces;
        nationOneState = nationOne;
        nationTwoState = nationTwo;
    }
}
