using UnityEngine;
using UnityEngine.InputSystem;

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
        if (context.canceled && IsOpen)
        {
            panelStateMachine.switchState();
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

    public void startBuildingOnProvince(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            if (IsOpen && provinceOpenOn.GetComponent<Province>().getOwner().canBuild(panelStateMachine.getCost()))
            {
                panelStateMachine.buildSelected();
            }
        }
    }
}
