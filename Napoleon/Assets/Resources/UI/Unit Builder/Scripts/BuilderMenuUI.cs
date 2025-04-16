using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using IUnit = Resources.Features.Model.Units.IUnit;

public class BuilderMenuUI : MonoBehaviour
{
    private bool IsOpen { get; set; }
    private GameObject provinceOpenOn;
    private PanelStateMachine panelStateMachine;
    [SerializeField]private GameObject buildingUnitPreFab;
    [SerializeField]private GameObject buildingBoatPreFab;
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
            panelStateMachine.switchState();
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

    public GameObject getProvinceOpenOn()
    {
        return provinceOpenOn;
    }

    public GameObject getBuildingUnitPreFab()
    {
        return buildingUnitPreFab;
    }

    public GameObject getBuildingBoatPreFab()
    {
        return buildingBoatPreFab;
    }

    public GameObject instantiatePrefab(GameObject prefab)
    {
        return Instantiate(prefab, provinceOpenOn.transform);
    }

    public void setProvinceOpenOn(GameObject provinceOpenOn)
    {
        this.provinceOpenOn = provinceOpenOn;
    }

    public void startBuildingOnProvince()
    {
        if (IsOpen)
        {
            panelStateMachine.buildSelected();
        }
    }
}
