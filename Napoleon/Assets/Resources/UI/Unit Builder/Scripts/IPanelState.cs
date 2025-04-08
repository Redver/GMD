using UnityEngine;

public interface IPanelState
{
    void Enter(BuilderMenuUI builderMenuUI);
    void BuildSelected(BuilderMenuUI builderMenuUI);
    void Exit(BuilderMenuUI builderMenuUI);
    
    IPanelState NextState();
}
