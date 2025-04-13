using UnityEngine;

public class BoatBuildState : IPanelState
{
    private GameObject boatPanel = null;
    
    public void Enter(BuilderMenuUI builderMenuUI)
    {
        GameObject[] panels = builderMenuUI.getPanels();
        foreach (GameObject panel in panels)
        {
            if (panel.name == "Boat")
            {
                boatPanel = panel;
            }
        }
        makeBoatSelected();
    }
    
    public IPanelState NextState()
    {
        return new UnitBuildState();
    }

    public void BuildSelected(BuilderMenuUI builderMenuUI)
    {
        throw new System.NotImplementedException();
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