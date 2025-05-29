using System.Collections.Generic;
using Resources.Features.Model.Units;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Nation : MonoBehaviour
{
    private string nationName;
    private float treasurey;
    private int unitCount;
    private int boatCount;
    [SerializeField] private int provinceCount;
    [SerializeField] GameObject capitalProvince;
    [SerializeField] private GameObject MoneyText;
    [SerializeField] private GameObject SoldierCurrentMoveableText;
    [SerializeField] private GameObject SoldierAllText;
    [SerializeField] private GameObject BoatCurrentMoveableText;
    [SerializeField] private GameObject BoatAllText;
    [SerializeField] private GameObject ProvinceCurrentOwnedText;
    [SerializeField] private int startingTreasury = 100;
    public UnityEvent endTurnEvent; 
    private TextMeshProUGUI moneyUi;
    private TextMeshProUGUI soliderMoveUi;
    private TextMeshProUGUI soldierOwnedUi;
    private TextMeshProUGUI boatMoveUi;
    private TextMeshProUGUI boatOwnedUi;
    private TextMeshProUGUI provincesOwnedUi;


    private void Awake()
    {
        nationName = gameObject.name;
        if (endTurnEvent == null)
        {
            endTurnEvent = new UnityEvent();
        }
        moneyUi = MoneyText.GetComponent<TextMeshProUGUI>();
        soliderMoveUi = SoldierCurrentMoveableText.GetComponent<TextMeshProUGUI>();
        soldierOwnedUi = SoldierAllText.GetComponent<TextMeshProUGUI>();
        boatMoveUi = BoatCurrentMoveableText.GetComponent<TextMeshProUGUI>();
        boatOwnedUi = BoatAllText.GetComponent<TextMeshProUGUI>();
        provincesOwnedUi = ProvinceCurrentOwnedText.GetComponent<TextMeshProUGUI>();
        treasurey = startingTreasury;
        refreshTreasurey();
    }

    void Start()
    {
        refreshProvinceUI();
        updateProvinceCount();
    }

    void Update()
    {
        
    }

    public bool canBuild(int cost)
    {
        if (cost > treasurey)
        {
            return false;
        }
        return true;
    }

    public void refreshTreasurey()
    {
        moneyUi.text = treasurey.ToString();
    }
    public void refreshSoldierUI()
    {
        soliderMoveUi.text = countAllSoldiersWithMoves().ToString();
        soldierOwnedUi.text = countAllOwnedSoldiers().ToString();
    }

    public void refreshBoatUi()
    {
        boatMoveUi.text = countAllBoatsWithMoves().ToString();
        boatOwnedUi.text = countAllOwnedBoats().ToString();
    }

    public void refreshProvinceUI()
    {
        provincesOwnedUi.text = provinceCount.ToString();
    }

    private List<IUnit> getAllOwnedUnits()
    {
        GameObject[] allUnits = GameObject.FindGameObjectsWithTag("Unit");
        List<IUnit> units = new List<IUnit>();
        foreach (var unitGameObject in allUnits)
        {
            IUnit currentUnit = unitGameObject.GetComponent<UnitView>().getUnitLogic();
            if (currentUnit.getNation() == this)
            {
                units.Add(currentUnit);
            }
        }
        return units;
    }

    private List<IUnit> getAllOwnedInfantry()
    {
        List<IUnit> unitList = getAllOwnedUnits();
        List<IUnit> infantry = new List<IUnit>();
        foreach (var unit in unitList)
        {
            if (unit.IsBoat())
            {
                
            }
            else
            {
                infantry.Add(unit);
            }
        }
        return infantry;
    }
    
    private List<IUnit> getAllOwnedBoats()
    {
        List<IUnit> unitList = getAllOwnedUnits();
        List<IUnit> boats = new List<IUnit>();
        foreach (var unit in unitList)
        {
            if (unit.IsBoat())
            {
                boats.Add(unit);
            }
            else
            {
            }
        }
        return boats;
    }

    private int countAllOwnedSoldiers()
    {
        return getAllOwnedInfantry().Count;
    }

    private int countAllUnitsWithMoves(List<IUnit> unitList)
    {
        List<IUnit> unitsWithMoves = new List<IUnit>();
        foreach (var unit in unitList)
        {
            if (unit.getMoves() > 0)
            {
                unitsWithMoves.Add(unit);
            }
        }
        return unitsWithMoves.Count;
    }

    private int countAllSoldiersWithMoves()
    {
        List<IUnit> infantryList = getAllOwnedInfantry();
        return countAllUnitsWithMoves(infantryList);
    }
    
    private int countAllOwnedBoats()
    {
        return getAllOwnedBoats().Count;
    }

    
    private int countAllBoatsWithMoves()
    {
        List<IUnit> boatList = getAllOwnedBoats();
        return countAllUnitsWithMoves(boatList);
    }



    public void updateTreasurey()
    {
        float income = provinceCount * 3;
        float expenses = unitCount + (boatCount * 3);
        float net = income - expenses;
        treasurey += net;
        refreshTreasurey();
    }

    public void payForUnit()
    {
        treasurey -= 5;
        refreshTreasurey();
    }
    
    public void payForBoat()
    {
        treasurey -= 15;
        refreshTreasurey();
    }


    public void onEndTurn()
    { 
        updateProvinceCount();
        updateTreasurey();
        endTurnEvent.Invoke();
        refreshBoatUi();
        refreshSoldierUI();
        refreshProvinceUI();
    }

    public void updateProvinceCount()
    {
        this.provinceCount = transform.childCount;
        refreshProvinceUI();
    }

    public void getNewCapitalProvince()
    {
        capitalProvince = transform.GetChild(0).gameObject;
    }

    public GameObject getCurrentCapitalProvince()
    {
        return capitalProvince;
    }

    public void onStartTurn()
    {
        refreshProvinceUI();
        if (capitalProvince.GetComponent<Province>().getOwner().nationName != this.nationName)
        {
            getNewCapitalProvince();
        }
    }

    public string getName()
    {
        return nationName;
    }

}
