using UnityEngine;

public interface IPanelState
{
    void Enter(BuilderMenuUI builderMenuUI);
    void BuildSelected(BuilderMenuUI builderMenuUI);
    void Exit();

    bool canBuild(Province here);
    
    int getCost();
    
    IPanelState NextState();
}
