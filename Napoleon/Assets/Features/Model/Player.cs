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
    [SerializeField] private GameObject SelectorsObject;
    private Selectors selectors;

    void Start()
    {
        GetComponent<PlayerInput>().actions.Enable();
        selectors = SelectorsObject.GetComponent<Selectors>();
        nation = NationObject.GetComponent<Nation>();
        selector = SelectorObject.GetComponent<Selector>();
    }

    private void Awake()
    {
    }

    void Update()
    {
        
    }
    
    public void OnEndTurn(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            Debug.Log("End Turn button pressed!");
            if (selectors.gameObject.activeSelf)
            {
                selectors.OnEndTurn(SelectorObject);
            }
        }
    }

    public void onStartTurn()
    {
        nation.onStartTurn();
        selector.onStartTurn(NationObject);
    }
}
