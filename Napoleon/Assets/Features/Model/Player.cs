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

    void Start()
    {
        
    }

    private void Awake()
    {
        nation = NationObject.GetComponent<Nation>();
        if (nation != null)
        {
           SetNation(nation); 
        }
        selector = SelectorObject.GetComponent<Selector>();
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
        }
    }

    public void onEndTurn(InputAction.CallbackContext context)
    {
        nation.onEndTurn();
    }

    public void onStartTurn()
    {
        nation.onStartTurn();
        selector.onChangeTurn(NationObject);
    }
}
