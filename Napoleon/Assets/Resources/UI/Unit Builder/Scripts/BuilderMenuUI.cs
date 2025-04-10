using System;
using Unity.VisualScripting;
using UnityEngine;

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
        panelStateMachine.SwitchState();
    }

    public void ChangeSelected()
    {
        panelStateMachine.SwitchState();
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
