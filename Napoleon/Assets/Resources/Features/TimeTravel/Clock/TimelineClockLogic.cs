using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Resources.Features.Model.Units;
using Resources.Features.TimeTravel.Clock.SavedData;
using Resources.map_assets.Selector_Scripts.SelectorMVP;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class TimelineClockLogic : MonoBehaviour
{
    private int currentTurn;
    private int currentTimeline;
    [SerializeField] TextMeshProUGUI turnText;
    [SerializeField] TextMeshProUGUI timelineText;
    private gameStateTable gameStateTable = new gameStateTable();

    public void Start()
    {
        updateCurrentTurn(0);
        updateCurrentTimeline(0);

        gameState initialState = makeGameState();
        initialState.IsHead = true;
        gameStateTable.saveGameState(initialState, 0, 0);
    }

    public int getCurrentTurn()
    {
        return currentTurn;
    }

    public Nation[] getNationOneAndTwo()
    {
        
        GameObject[] nations = GameObject.FindGameObjectsWithTag("Nation");
        Nation nationOne;
        Nation nationTwo;
        
        if (nations[0].name == "GreatBritain")
        {
            nationOne = nations[0].GetComponent<Nation>();
            nationTwo = nations[1].GetComponent<Nation>();
        }
        else
        {
            nationTwo = nations[0].GetComponent<Nation>();
            nationOne = nations[1].GetComponent<Nation>();
        }
        return new Nation[] { nationOne, nationTwo };
    }

    public List<IUnit> getAllUnits()
    {
        
        Nation[] nations = getNationOneAndTwo();
        
        Nation nationOne = nations[0];
        Nation nationTwo = nations[1];
        
        List<IUnit> nationOneUnits = new List<IUnit>();
        List<IUnit> nationTwoUnits = new List<IUnit>();

        nationOneUnits = nationOne.getAllOwnedUnits();
        nationTwoUnits = nationTwo.getAllOwnedUnits();

        List<IUnit> allUnits = new List<IUnit>();
        
        allUnits.AddRange(nationOneUnits);
        allUnits.AddRange(nationTwoUnits);

        return allUnits;
    }

    public List<ProvinceData> getAllProvincesData()
    {
        List<ProvinceData> allProvinceData = new List<ProvinceData>();
        List<Province> allProvince = getAllProvinces();

        foreach (var province in allProvince)
        {
            allProvinceData.Add(new ProvinceData(province.name,province.getOwner(),province.isInCombat()));
        }

        return allProvinceData;
    }

    public List<IUnit> getAllUnitsOnlyOnBoard()
    {
        List<IUnit> allUnits = getAllUnits();
        GameObject activeSelectorObject = GameObject.FindGameObjectWithTag("Selectors");
        
        List<UnitView> allUnitViewInSelector = new List<UnitView>();
        List<IUnit> allUnitsInSelector = new List<IUnit>();
        allUnitViewInSelector.AddRange(activeSelectorObject.transform.GetComponentsInChildren<UnitView>());
        foreach (var unitView in allUnitViewInSelector)
        {
            allUnitsInSelector.Add(unitView.getUnitLogic());
        }

        allUnits.RemoveAll(u => allUnitsInSelector.Contains(u));
        
        return allUnits;
    }

    public List<UnitData> getAllUnitData()
    {
        List<UnitData> allUnitData = new List<UnitData>();
        List<IUnit> allUnits = getAllUnitsOnlyOnBoard();
        foreach (var unit in allUnits)
        {
            allUnitData.Add(new UnitData(unit.getCurrentProvince().GetComponent<Province>(),unit.getNation(),unit.IsBoat(),unit.getMoves(),unit.isInCombat()));
        }
        return allUnitData;
    }

    public gameState makeGameState()
    {
        Nation[] nations = getNationOneAndTwo();

        NationData nationOne = new NationData(nations[0].getName(), nations[0].getTreasurey());
        NationData nationTwo = new NationData(nations[1].getName(), nations[1].getTreasurey());
        
        List<ProvinceData> allProvincesWithData = getAllProvincesData();
        List<UnitData> allUnits = getAllUnitData();
        
        return new gameState(allUnits,allProvincesWithData,nationOne,nationTwo);
    }

    public void updateGameStateTable(gameState newGameState)
    {
        gameStateTable.saveGameState(newGameState,currentTimeline, currentTurn);
    }

    public void addGameStateToNewTimeline(gameState newGameState, int timeline, int turn)
    {
        gameStateTable.saveGameState(newGameState,timeline,turn);
    }

    public void updateUis()
    {
        turnText.text = currentTurn.ToString();
        timelineText.text = currentTimeline.ToString();
        Nation[] nations = getNationOneAndTwo();
        nations[0].updateUi();
        nations[1].updateUi();
    }

    public void updateCurrentTurn(int currentTurn)
    {
        this.currentTurn = currentTurn;
        turnText.text = this.currentTurn.ToString();
    }

    public void updateCurrentTimeline(int currentLine)
    {
        this.currentTimeline = currentLine;
        timelineText.text = currentTimeline.ToString();
    }

    public List<Province> getAllProvinces()
    {
        List<Province> allLandProvinces = new List<Province>();
        List<Province> allSeaTiles = new List<Province>();
        List<Province> allProvinces = new List<Province>();

        GameObject[] landList = GameObject.FindGameObjectsWithTag("Province");
        GameObject[] seaList = GameObject.FindGameObjectsWithTag("SeaTile");

        foreach (GameObject land in landList)
        {
            allLandProvinces.Add(land.GetComponent<Province>());
        }

        foreach (GameObject sea in seaList)
        {
            allSeaTiles.Add(sea.GetComponent<Province>());
        }

        allProvinces.AddRange(allLandProvinces);
        allProvinces.AddRange(allSeaTiles);

        return allProvinces;
    }

    public void unloadBoard()
    {
        List<Province> allProvinces = getAllProvinces();
        foreach (var province in allProvinces)
        {
            province.clearOwner();
            province.deleteUnits();
        }
    }

    public void loadBoardFromGamestateStore(int timeline, int turn)
    {
        gameState loadedGameState = gameStateTable.getGameState(timeline,turn);
        
        List<Province> allProvinces = getAllProvinces();

        foreach (var province in allProvinces)
        {
            ProvinceData thisProvince = loadedGameState.AllOwnedProvinces.Find(e => e.ProvinceName == province.name);
            if (thisProvince.Owner != null)
            {
                province.setLandOwner(thisProvince.Owner);
            }
            province.CombatInProvince = thisProvince.IsInCombat;
        }
        
        foreach (var unit in loadedGameState.AllUnits)
        {
            unit.Province.GetComponent<Province>().summonUnit(unit);
            unit.Province.GetComponent<Province>().spreadUnits();
        }

        Nation[] currentNations = getNationOneAndTwo();
        Nation nationOne = currentNations[0];
        Nation nationTwo = currentNations[1];

        nationOne.updateWithNation(loadedGameState.NationOneState);
        nationTwo.updateWithNation(loadedGameState.NationTwoState);
    }

    public void unloadBoardAndLoadTurn(int turn)
    {
        unloadBoard();
        loadBoardFromGamestateStore(this.currentTimeline, turn);
    }

    public void unloadBoardAndLoadTimeline(int timeline)
    {
        unloadBoard();
        int turn = gameStateTable.GetTurnOfHead(timeline);
        loadBoardFromGamestateStore(timeline,turn);
    }
    public void endTurnAcrossAllTimelines()
    {
        updateGameStateTable(makeGameState());
        StartCoroutine(EndTurnRoutine());
        updateUis();
    }

    private IEnumerator EndTurnRoutine()
    {
        int maxTimeline = gameStateTable.getNumberOfTimelines();

        for (int timeline = 0; timeline < maxTimeline; timeline++)
        {
            unloadBoard(); 
            yield return new WaitForEndOfFrame(); 

            loadBoardFromGamestateStore(timeline, gameStateTable.GetTurnOfHead(timeline));
            yield return new WaitForEndOfFrame(); 

            gameState oldHead = gameStateTable.GetHeadOfTimeline(timeline);
            int oldTurn = gameStateTable.GetTurnOfHead(timeline);

            if (oldHead != null)
                oldHead.IsHead = false;

            Nation[] currentNations = getNationOneAndTwo();

            if (oldTurn % 2 == 0)
                currentNations[0].onEndTurn();
            else
                currentNations[1].onEndTurn();

            yield return new WaitForEndOfFrame(); 
            
            currentNations[0].updateUi();
            currentNations[1].updateUi();

            currentTurn = gameStateTable.GetTurnOfHead(timeline);
            gameState newState = makeGameState();
            newState.IsHead = true;

            gameStateTable.saveGameState(newState, timeline, oldTurn + 1);
            updateCurrentTurn(currentTurn + 1);
            updateCurrentTimeline(timeline);
        }
        unloadBoard(); 
        loadBoardFromGamestateStore(0, gameStateTable.GetTurnOfHead(0));
        currentTimeline = 0;
        currentTurn = gameStateTable.GetTurnOfHead(0);
        updateUis();
    }
    
    private void SaveIfOnHeadTurn()
    {
        if (gameStateTable.GetTurnOfHead(currentTimeline) == currentTurn)
        {
            updateGameStateTable(makeGameState());
        }
    }

    public void makeNewTimeline()
    {
        gameState alternateGameState = makeGameState();
        alternateGameState.IsHead = true;
        int newTimeLine = gameStateTable.getNumberOfTimelines();
        addGameStateToNewTimeline(alternateGameState, newTimeLine , currentTurn);
        showNewestTimeline();
    }

    public void showPreviousTurn()
    {
        int minTurn = gameStateTable.getTurnOfMin(currentTimeline);
        SaveIfOnHeadTurn();
        if (currentTurn > minTurn)
        {
            unloadBoardAndLoadTurn(currentTurn - 1);
            updateCurrentTurn(currentTurn - 1);
            updateUis();
        }
    }

    public void showNextTurn()
    {
        int maxTurn = gameStateTable.GetTurnOfHead(currentTimeline);
        if (currentTurn < maxTurn)
        {
            unloadBoardAndLoadTurn(currentTurn + 1);
            updateCurrentTurn(currentTurn + 1);
            updateUis();
        }
    }

    public void showPreviousTimeline()
    {
        SaveIfOnHeadTurn();
        if (currentTimeline > 0)
        {
            unloadBoardAndLoadTimeline(currentTimeline - 1);
            updateCurrentTimeline(currentTimeline - 1);
            updateCurrentTurn(gameStateTable.GetTurnOfHead(currentTimeline));
            updateUis();
        }
    }

    public void showNextTimeline()
    {
        SaveIfOnHeadTurn();
        int maxTimeline = gameStateTable.getNumberOfTimelines();
        if (currentTimeline + 1 < maxTimeline)
        {
            unloadBoardAndLoadTimeline(currentTimeline + 1);
            updateCurrentTimeline(currentTimeline + 1);
            updateCurrentTurn(gameStateTable.GetTurnOfHead(currentTimeline));
            updateUis();
        }
    }

    public void showNewestTimeline()
    {
        SaveIfOnHeadTurn();
        int maxTimeline = gameStateTable.getNumberOfTimelines();
        unloadBoardAndLoadTimeline(maxTimeline - 1);
        updateCurrentTimeline(maxTimeline - 1);
        updateUis();
    }

    public int getCurrentTimeline()
    {
        return currentTimeline;
    }

}
