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
        throw new System.NotImplementedException();
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
