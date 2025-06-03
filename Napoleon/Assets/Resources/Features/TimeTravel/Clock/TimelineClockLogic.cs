using System;
using System.Collections.Generic;
using Resources.Features.Model.Units;
using Resources.map_assets.Selector_Scripts.SelectorMVP;
using TMPro;
using UnityEngine;

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
        
        GameObject[] nations = GameObject.FindGameObjectsWithTag("Nations");
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

    public List<IUnit> getAllUnitsOnlyOnBoard()
    {
        List<IUnit> allUnits = getAllUnits();
        GameObject activeSelectorObject = GameObject.FindGameObjectWithTag("Selectors");
        
        List<IUnit> allUnitsInSelector = new List<IUnit>();
        allUnitsInSelector.AddRange(activeSelectorObject.transform.GetComponentsInChildren<IUnit>());

        allUnits.RemoveAll(u => allUnitsInSelector.Contains(u));
        
        return allUnits;
    }

    public gameState makeGameState()
    {
        Nation[] nations = getNationOneAndTwo();
        
        Nation nationOne = nations[0];
        Nation nationTwo = nations[1];
        
        List<Province> allOwnedProvinces = getAllOwnedProvinces();
        List<IUnit> allUnits = getAllUnitsOnlyOnBoard();
        
        return new gameState(allUnits,allOwnedProvinces,nationOne,nationTwo);
    }

    public void updateGameStateTable(gameState newGameState)
    {
        gameStateTable.saveGameState(newGameState,currentTurn,currentTimeline);
    }

    public void unHeadOldGameStateAndSaveNewHead(gameState newGameState)
    {
        gameStateTable.getGameState(currentTimeline, (currentTurn - 1)).IsHead = false;
        newGameState.IsHead = true;
        updateGameStateTable(newGameState);
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

    public void loadNewBoard(int timeline, int turn)
    {
        gameState loadedGameState = gameStateTable.getGameState(timeline,turn);
        foreach (var province in loadedGameState.AllOwnedProvinces)
        {
            province.setLandOwner(province.getOwner());   
        }

        foreach (var unit in loadedGameState.AllUnits)
        {
            unit.getCurrentProvince().GetComponent<Province>().summonUnit(unit);
        }

        Nation[] currentNations = getNationOneAndTwo();
        Nation nationOne = currentNations[0];
        Nation nationTwo = currentNations[1];

        nationOne.updateWithNation(loadedGameState.NationOneState);
        nationTwo.updateWithNation(loadedGameState.NationTwoState);
    }

    public void unloadBoardAndLoadDifferantTurnOrTimeline(int timeline, int turn)
    {
        //if new timeline is loaded load the head, not the current turn
    }

    public void endTurn()
    {
        unHeadOldGameStateAndSaveNewHead(makeGameState());
    }

    public void endTurnAcrossAllTimelines()
    {
        //for each timeline, end turn, i.e make that nation do their unity event?
    }

    public void showPreviousTurn()
    {
        
    }

    public void showNextTurn()
    {
        
    }

    public void showNextTimeline()
    {
        
    }

    public void showPreviousTimeline()
    {
        
    }
}
