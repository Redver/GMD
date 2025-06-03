using System;
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

    public void incrementTurn()
    {
        currentTurn++;
    }

    public void decrementTurn()
    {
        currentTurn--;
    }

    public void changeToTurn(int turn)
    {
        currentTurn = turn;
    }
    
    public int getCurrentTimeline()
    {
        return currentTimeline;
    }

    public void incrementTimeline()
    {
        currentTimeline++;
    }

    public void decrementTimeline()
    {
        currentTimeline--;
    }

    public void changeToTimeline(int timeline)
    {
        currentTimeline = timeline;
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

    public List<Province> getAllOwnedProvinces()
    {
        Nation[] nations = getNationOneAndTwo();
        
        Nation nationOne = nations[0];
        Nation nationTwo = nations[1];
        
        List<Province> nationOneProvinces = new List<Province>();
        List<Province> nationTwoProvinces = new List<Province>();

        nationOneProvinces = nationOne.getAllOwnedProvinces();
        nationTwoProvinces = nationTwo.getAllOwnedProvinces();

        List<Province> allProvinces = new List<Province>();
        
        allProvinces.AddRange(nationOneProvinces);
        allProvinces.AddRange(nationTwoProvinces);

        return allProvinces;
    }

    public List<ProvinceData> getAllProvincesData()
    {
        List<ProvinceData> allProvinceData = new List<ProvinceData>();
        List<Province> allProvinces = getAllOwnedProvinces();

        foreach (var province in allProvinces)
        {
            allProvinceData.Add(new ProvinceData(province.name,province.getOwner()));
        }

        return allProvinceData;
    }

    public List<IUnit> getAllUnitsOnlyOnBoard()
    {
        List<IUnit> allUnits = getAllUnits();
        GameObject activeSelectorObject = GameObject.FindGameObjectWithTag("Selectors");
        
        List<IUnit> allUnitsInSelector = new List<IUnit>();
        allUnitsInSelector.AddRange(activeSelectorObject.transform.GetComponentsInChildren<IUnit>());

        allUnits.RemoveAll(u => allUnitsInSelector.Contains(u));
        
        return allUnits;
    }

    public List<UnitData> getAllUnitData()
    {
        List<UnitData> allUnitData = new List<UnitData>();
        List<IUnit> allUnits = getAllUnitsOnlyOnBoard();
        foreach (var unit in allUnits)
        {
            allUnitData.Add(new UnitData(unit.getCurrentProvince().GetComponent<Province>(),unit.getNation(),unit.IsBoat(),unit.getMoves()));
        }
        return allUnitData;
    }

    public gameState makeGameState()
    {
        Nation[] nations = getNationOneAndTwo();

        NationData nationOne = new NationData(nations[0].getName(), nations[0].getTreasurey());
        NationData nationTwo = new NationData(nations[1].getName(), nations[1].getTreasurey());
        
        List<ProvinceData> allOwnedProvinces = getAllProvincesData();
        List<UnitData> allUnits = getAllUnitData();
        
        return new gameState(allUnits,allOwnedProvinces,nationOne,nationTwo);
    }

    public void updateGameStateTable(gameState newGameState)
    {
        gameStateTable.saveGameState(newGameState,currentTurn,currentTimeline);
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
        updateGameStateTable(makeGameState());
        List<Province> allProvinces = getAllProvinces();
        foreach (var province in allProvinces)
        {
            province.clearOwner();
            province.deleteUnits();
        }
    }

    public List<Province> getAllLandProvinces()
    {
        List<Province> allLandProvinces = new List<Province>();
        GameObject[] landList = GameObject.FindGameObjectsWithTag("Province");
        
        foreach (GameObject land in landList)
        {
            allLandProvinces.Add(land.GetComponent<Province>());
        }

        return allLandProvinces;
    }

    public void loadNewBoard(int timeline, int turn)
    {
        gameState loadedGameState = gameStateTable.getGameState(timeline,turn);
        
        List<Province> allProvinces = getAllLandProvinces();
        
        foreach (var province in allProvinces)
        {
            bool foundOwner = false;

            foreach (var loadedProvince in loadedGameState.AllOwnedProvinces)
            {
                if (province.name == loadedProvince.ProvinceName)
                {
                    province.setLandOwner(loadedProvince.Owner);
                    foundOwner = true;
                    break; 
                }
            }

            if (!foundOwner)
            {
                province.clearOwner();
            }
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
        loadNewBoard(this.currentTimeline, turn);
    }

    public void unloadBoardAndLoadTimeline(int timeline)
    {
        unloadBoard();
        int turn = gameStateTable.GetTurnOfHead(timeline);
        loadNewBoard(timeline,turn);
    }

    public void endTurnAcrossAllTimelines()
    {
        int maxTimeline = gameStateTable.getNumberOfTimelines();
        unloadBoardAndLoadTimeline(0);
        
        for (int timeline = 0; timeline < maxTimeline; timeline++)
        {

            unloadBoardAndLoadTimeline(timeline);

            gameState oldHead = gameStateTable.GetHeadOfTimeline(timeline);
            if (oldHead != null)
            {
                oldHead.IsHead = false;
            }

            int oldTurn = gameStateTable.GetTurnOfHead(timeline);

            gameState newState = makeGameState();
            newState.IsHead = true;
            gameStateTable.saveGameState(newState, timeline, oldTurn + 1);

            Nation[] currentNations = getNationOneAndTwo();
            if ((oldTurn + 1) % 2 == 0)
            {
                currentNations[0].onEndTurn();
            }
            else
            {
                currentNations[1].onEndTurn();
            }
            
        }

        updateCurrentTurn(currentTurn + 1);
    }

    public void showPreviousTurn()
    {
        if (gameStateTable.GetTurnOfHead(currentTimeline) == currentTurn)
        {
            updateGameStateTable(makeGameState());
        }
        if (currentTurn > 0)
        {
            unloadBoardAndLoadTurn(currentTurn - 1);
            updateCurrentTurn(currentTurn - 1);
        }
    }

    public void showNextTurn()
    {
        int maxTurn = gameStateTable.GetTurnOfHead(currentTimeline);
        if (currentTurn < maxTurn)
        {
            unloadBoardAndLoadTurn(currentTurn + 1);
            updateCurrentTurn(currentTurn + 1);
        }
    }

    public void showPreviousTimeline()
    {
        if (gameStateTable.GetTurnOfHead(currentTimeline) == currentTurn)
        {
            updateGameStateTable(makeGameState());
        }
        if (currentTimeline > 0)
        {
            unloadBoardAndLoadTimeline(currentTimeline - 1);
            updateCurrentTimeline(currentTimeline - 1);
        }
    }

    public void showNextTimeline()
    {
        if (gameStateTable.GetTurnOfHead(currentTimeline) == currentTurn)
        {
            updateGameStateTable(makeGameState());
        }
        int maxTimeline = gameStateTable.getNumberOfTimelines();
        if (currentTimeline + 1 < maxTimeline)
        {
            unloadBoardAndLoadTimeline(currentTimeline + 1);
            updateCurrentTimeline(currentTimeline + 1);
        }
    }
}
