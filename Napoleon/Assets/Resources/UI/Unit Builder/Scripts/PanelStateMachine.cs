
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

    public void switchState()
    {
        IPanelState nextState = State.NextState();
        State.Exit();
        State = nextState;
        State.Enter(BuilderMenuUI);
    }

    public int getCost()
    {
        return State.getCost();
    }

    public void buildSelected()
    {
        if (State.canBuild(BuilderMenuUI.getProvinceOpenOn().GetComponent<Province>()))
        {
            State.BuildSelected(BuilderMenuUI);
            SoundLibrary.Instance.PlayClipAtPoint(SoundLibrary.Instance.GetBuildSfx(),this.BuilderMenuUI.transform.position);
        }
        else
        {
            //unity event to show cannot build here
            SoundLibrary.Instance.PlayClipAtPoint(SoundLibrary.Instance.GetForbiddenSfx(),this.BuilderMenuUI.transform.position);
        }
    }

    public void Build()
    {
        State.BuildSelected(BuilderMenuUI);
    }
}
