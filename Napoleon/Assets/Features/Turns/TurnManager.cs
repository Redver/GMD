using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class NewMonoBehaviourScript : MonoBehaviour
{
    private int playerTurn;
    private int playerCount = 0;
    private List<GameObject> _playersList;
    private GameObject _UIManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _UIManager = GameObject.FindGameObjectWithTag("UIManager");
    }

    private void Awake()
    {

    }

    public void addPlayersToTurnManager()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            _playersList.Add(player);
            playerCount = playerCount + 1;
        }
    }

    public void changeTurn()
    {
        _playersList[playerTurn].GetComponent<PlayerTurnScript>().disableControl();
        
        playerTurn = playerTurn++ % 2;

        _playersList[playerTurn].GetComponent<PlayerTurnScript>().enableControl();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
