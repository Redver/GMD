using UnityEngine;
using System;
using System.Collections.Generic;
using Resources.map_assets.Selector_Scripts.SelectorMVP;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject NationObject;
    [SerializeField] private GameObject SelectorObject;
    [SerializeField] private GameObject SelectorsObject;

    public UnityEvent onEndTurnUnityEvent; 

    private SelectorView selector;
    private Nation nation;
    private Selectors selectors;

    void Start()
    {
        GetComponent<PlayerInput>().actions.Enable();
        selectors = SelectorsObject.GetComponent<Selectors>();
        nation = NationObject.GetComponent<Nation>();
        selector = SelectorObject.GetComponent<SelectorView>();
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
            onEndTurnUnityEvent.Invoke();
        }
    }

    public void onStartTurn()
    {
        nation.onStartTurn();
        selector.onStartTurn(NationObject);
    }
}
