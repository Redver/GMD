using UnityEngine;

public class PanelStateMachine
{
    public IPanelState State { get; set; }
    public BuilderMenuUI BuilderMenuUI { get; set; }

    public PanelStateMachine(BuilderMenuUI builderMenuUI)
    {
        BuilderMenuUI = builderMenuUI;
        State = new UnitBuildState();
        State.Enter(builderMenuUI);
    }

    public void SwitchState()
    {
        IPanelState nextState = State.NextState();
        State.Exit(BuilderMenuUI);
        State = nextState;
        State.Enter(BuilderMenuUI);
    }

    public void Build()
    {
        State.BuildSelected(BuilderMenuUI);
    }
}
