using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Camera : MonoBehaviour
{
    private int playerTurn;
    private int playerCount = 0;
    private List<GameObject> _playersList;

    private void Awake()
    {
        addPlayers();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    
    private void addPlayers()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            _playersList.Add(player);
            playerCount = playerCount + 1;
        }
    }


    void OnSelectProvince()
    {
        
    }

    void OnSelectBuilding()
    {
        
    }

    void OnUiSelect()
    {
        
    }

}
