using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject NationObject;
    [SerializeField] private GameObject SelectorObject;
    private Selector selector;
    private Nation nation;
    [SerializeField] private int playerIndex;
    public static int currentPlayerIndex;

    void Start()
    {
        currentPlayerIndex = 0;
        nation = NationObject.GetComponent<Nation>();
        if (nation != null)
        {
            SetNation(nation); 
        }
        selector = SelectorObject.GetComponent<Selector>();
    }

    private void Awake()
    {
    }

    void Update()
    {
        
    }

    public void SetNation(Nation nation)
    {
        switch (nation.getName())
        {
            case "GreatBritain":
                playerIndex = 0;
                break;
            case "France":
                playerIndex = 1;
                break;
            default:
                playerIndex = -1;
                break; 
        }
    }

    public void onEndTurn(InputAction.CallbackContext context)
    {
        if (playerIndex == currentPlayerIndex)
        {
            nation.onEndTurn();
            currentPlayerIndex = (currentPlayerIndex + 1) % 1;
            List<Player> players = new List<Player>();
            players.Add(GameObject.FindGameObjectWithTag("Players").GetComponent<Player>());
            foreach (var player in players)
            {
                if (player.getPlayerIndex() == currentPlayerIndex)
                {
                    player.onStartTurn();
                }
            }
        }
    }

    public void onStartTurn()
    {
        nation.onStartTurn();
        selector.onChangeTurn(NationObject);
    }

    public int getPlayerIndex()
    {
        return playerIndex;
    }
}
