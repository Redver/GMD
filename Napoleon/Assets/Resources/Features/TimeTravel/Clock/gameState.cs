using System.Collections.Generic;
using Resources.Features.Model.Units;
using Resources.Features.TimeTravel.Clock.SavedData;
using UnityEngine;

public class gameState
{
    public bool IsHead
    {
        get => isHead;
        set => isHead = value;
    }

    public List<UnitData> AllUnits
    {
        get => allUnits;
        set => allUnits = value;
    }

    public List<ProvinceData> AllOwnedProvinces
    {
        get => allOwnedProvinces;
        set => allOwnedProvinces = value;
    }

    public NationData NationOneState
    {
        get => nationOneState;
        set => nationOneState = value;
    }

    public NationData NationTwoState
    {
        get => nationTwoState;
        set => nationTwoState = value;
    }


    private bool isHead;
    private List<UnitData> allUnits = new List<UnitData>();
    private List<ProvinceData> allOwnedProvinces = new List<ProvinceData>();
    private NationData nationOneState;
    private NationData nationTwoState;

    public gameState(List<UnitData> units, List<ProvinceData> provinces, NationData nationOne, NationData nationTwo)
    {
        allUnits = units;
        allOwnedProvinces = provinces;
        nationOneState = nationOne;
        nationTwoState = nationTwo;
    }
}
