using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PanelStateMachine
{
    public IPanelState State { get; set; }
    public BuilderMenuUI BuilderMenuUI { get; set; }
    private float stateCooldown = 0.2f;
    private float stateTime = 0;

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

    public void buildSelected()
    {
        if (State.canBuild(BuilderMenuUI.getProvinceOpenOn().GetComponent<Province>()))
        {
            State.BuildSelected(BuilderMenuUI);
        }
        else
        {
            //unity event to show cannot build here
        }
    }

    public IEnumerator cooldownRoutine()
    {
        while (stateTime >= 0)
        {
            stateTime -= Time.deltaTime;
            yield return null;
        }
    }

    public void resetCooldown()
    {
        stateTime = stateCooldown;
    }

    public float getStateTime()
    {
        return stateTime;
    }

    public void Build()
    {
        State.BuildSelected(BuilderMenuUI);
    }
}
