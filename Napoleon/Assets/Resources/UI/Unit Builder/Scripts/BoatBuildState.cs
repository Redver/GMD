using Resources.Features.Model.Units;
using UnityEngine;

public class BoatBuildState : IPanelState
{
    private GameObject boatPanel = null;
    private int cost = 15;

    public void Enter(BuilderMenuUI builderMenuUI)
    {
        GameObject[] panels = builderMenuUI.getPanels();
        foreach (GameObject panel in panels)
        {
            if (panel.name == nameof(Boat))
            {
                boatPanel = panel;
            }
        }
        makeBoatSelected();
    }

    public int getCost()
    {
        return cost;
    }

    public IPanelState NextState()
    {
        return new UnitBuildState();
    }
    
    
    
    public bool canBuild(Province here)
    {
        return here.isCoastal();
    }

    public void BuildSelected(BuilderMenuUI builderMenuUI)
    {
        builderMenuUI.getProvinceOpenOn().GetComponent<Province>().getOwner().payForBoat();
        GameObject provinceOpenOn = builderMenuUI.getProvinceOpenOn();
        string path = "";
        if (provinceOpenOn.GetComponent<Province>().getOwner().name == "GreatBritain")
        {
            path = "Features/Model/Units/Unit Assets/uk boat";
        }
        if (provinceOpenOn.GetComponent<Province>().getOwner().name == "France")
        {
            path = "Features/Model/Units/Unit Assets/French Boat";
        }
        GameObject builtUnit = builderMenuUI.instantiatePrefab(builderMenuUI.getBuildingBoatPreFab());
        builtUnit.transform.localPosition = Vector3.zero;
        builtUnit.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = UnityEngine.Resources.Load<Sprite>(path);
        builtUnit.transform.GetComponent<UnitView>().Init(new Boat(), provinceOpenOn.GetComponent<Province>());
        builtUnit.transform.localScale = Vector3.one * 0.06f;
        IUnit unit = builtUnit.transform.GetComponent<UnitView>().getUnitLogic();
        provinceOpenOn.GetComponent<Province>().addUnitToStack(unit);
        unit.getView().greyOutUnit();
    }

    public void Exit()
    {
        makeBoatdeSelected();
    }

    private void makeBoatSelected()
    {
        Vector3 unitPanelPosition = boatPanel.transform.position;
        unitPanelPosition.y += 0.1f;
        boatPanel.transform.position = unitPanelPosition;
        boatPanel.GetComponent<SpriteRenderer>().sprite = UnityEngine.Resources.Load<Sprite>("UI/Unit Builder/Assets/BoatBuilderSelected");
    }

    private void makeBoatdeSelected()
    {
        Vector3 unitPanelPosition = boatPanel.transform.position;
        unitPanelPosition.y -= 0.1f;
        boatPanel.transform.position = unitPanelPosition;
        boatPanel.GetComponent<SpriteRenderer>().sprite = UnityEngine.Resources.Load<Sprite>("UI/Unit Builder/Assets/BoatBuilder");
    }
    
}