using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuilderMenuUI : MonoBehaviour
{
    private bool IsOpen { get; set; }
    private PanelStateMachine panelStateMachine;
    [SerializeField] private GameObject[] panels;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    void Awake()
    {
        panelStateMachine = new PanelStateMachine(this);
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public bool IsMenuOpen()
    {
        return IsOpen; 
    }

    public void setOpen()
    {
        IsOpen = true;
    }

    public void ChangeSelected(InputAction.CallbackContext context)
    {
        if (panelStateMachine.getStateTime() <= 0 && IsOpen)
        {
            panelStateMachine.SwitchState();
            panelStateMachine.resetCooldown();
            StartCoroutine(panelStateMachine.cooldownRoutine());
        }
    }

    public void setClose()
    {
        IsOpen = false;
    }

    public GameObject[] getPanels()
    {
        return panels;
    }


}
