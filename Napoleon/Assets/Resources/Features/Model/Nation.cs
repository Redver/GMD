using System.Collections;
using System.Collections.Generic;
using Resources.Features.Model.Units;
using Resources.Features.TimeTravel.Clock.SavedData;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Nation : MonoBehaviour
{
    private string nationName;
    private float treasurey;
    private int unitCount;
    private int boatCount;
    private string startingCapital;
    [SerializeField] private int provinceCount;
    [SerializeField] GameObject capitalProvince;
    [SerializeField] private GameObject MoneyText;
    [SerializeField] private GameObject SoldierCurrentMoveableText;
    [SerializeField] private GameObject SoldierAllText;
    [SerializeField] private GameObject BoatCurrentMoveableText;
    [SerializeField] private GameObject BoatAllText;
    [SerializeField] private GameObject ProvinceCurrentOwnedText;
    [SerializeField] private int startingTreasury = 100;
    [SerializeField] private GameObject victoryScreenPrefab;
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
        startingCapital = capitalProvince.name;
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

    public void showVictoryScreen()
    {
        Instantiate(victoryScreenPrefab);
    }

    public bool victoryCheck()
    {
        if (this.provinceCount >= 80 && ownsParisAndLondon())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool ownsParisAndLondon()
    {
        bool ownsParis = false;
        bool ownsLondon = false;
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            Transform child = gameObject.transform.GetChild(i);
            if (child.gameObject.name == "Iledefrance_0")
            {
                ownsParis = true;
            }
            if (child.gameObject.name == "SouthEast_0")
            {
                ownsLondon = true;
            }
        }
        return ownsParis && ownsLondon;
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
    
    public IEnumerator DelayedUiUpdate()
    {
        yield return new WaitForEndOfFrame(); 
        this.updateUi();
    }

    public void refreshProvinceUI()
    {
        provincesOwnedUi.text = provinceCount.ToString();
    }

    public List<IUnit> getAllOwnedUnits()
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
        float expenses = countAllOwnedSoldiers() + (countAllOwnedBoats() * 3);
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
    }

    public void updateUi()
    {
        refreshTreasurey();
        refreshBoatUi();
        refreshSoldierUI();
        refreshProvinceUI();
    }

    public void updateProvinceCount()
    {
        this.provinceCount = transform.childCount;
        refreshProvinceUI();
    }

    public List<Province> getAllOwnedProvinces()
    {
        List<Province> provinces = new List<Province>();

        provinces.AddRange(GetComponentsInChildren<Province>());

        return provinces;
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
        refreshSoldierUI();
        refreshTreasurey();
        refreshBoatUi();
        if (capitalProvince.GetComponent<Province>().getOwner().nationName != this.nationName)
        {
            getNewCapitalProvince();
        }
    }

    public string getName()
    {
        return nationName;
    }

    public void updateWithNation(NationData storedNation)
    {
        this.treasurey = storedNation.Treasurey;
    }


    public float getTreasurey()
    {
        return treasurey;
    }
}
