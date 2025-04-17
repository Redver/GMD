using Resources.Features.Model.Units;
using UnityEngine;

public class UnitBuildState : IPanelState
{
    private GameObject unitPanel = null;
    
    public void Enter(BuilderMenuUI builderMenuUI)
    {
        GameObject[] panels = builderMenuUI.getPanels();
        foreach (GameObject panel in panels)
        {
            if (panel.name == "Unit")
            {
                unitPanel = panel;
            }
        }
        makeUnitSelected();
    }

    public void BuildSelected(BuilderMenuUI builderMenuUI)
    {
        builderMenuUI.getProvinceOpenOn().GetComponent<Province>().getOwner().payForUnit();
        GameObject provinceOpenOn = builderMenuUI.getProvinceOpenOn();
        string path = "";
        if (provinceOpenOn.GetComponent<Province>().getOwner().name == "GreatBritain")
        {
            path = "Features/Model/Units/Unit Assets/UK flag unit";
        }
        if (provinceOpenOn.GetComponent<Province>().getOwner().name == "France")
        {
            path = "Features/Model/Units/Unit Assets/FRflag";
        }

        GameObject builtUnit = builderMenuUI.instantiatePrefab(builderMenuUI.getBuildingUnitPreFab());
        builtUnit.transform.localPosition = Vector3.zero;
        builtUnit.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = UnityEngine.Resources.Load<Sprite>(path);
        builtUnit.transform.GetComponent<UnitView>().Init(new Infantry(), provinceOpenOn.GetComponent<Province>());
        IUnit unit = builtUnit.transform.GetComponent<UnitView>().getUnitLogic();
        provinceOpenOn.GetComponent<Province>().addUnitToStack(unit);
    }

    public void Exit()
    {
        makeUnitdeSelected();
    }

    public IPanelState NextState()
    {
        return new BoatBuildState();
    }

    private void makeUnitSelected()
    {
        Vector3 unitPanelPosition = unitPanel.transform.position;
        unitPanelPosition.y += 0.1f;
        unitPanel.transform.position = unitPanelPosition;
        unitPanel.GetComponent<SpriteRenderer>().sprite = UnityEngine.Resources.Load<Sprite>("UI/Unit Builder/Assets/UnitBuildSquareSelected");
    }
    
    private void makeUnitdeSelected()
    {
        Vector3 unitPanelPosition = unitPanel.transform.position;
        unitPanelPosition.y -= 0.1f;
        unitPanel.transform.position = unitPanelPosition;
        unitPanel.GetComponent<SpriteRenderer>().sprite = UnityEngine.Resources.Load<Sprite>("UI/Unit Builder/Assets/UnitBuildSquare");
    }
}
