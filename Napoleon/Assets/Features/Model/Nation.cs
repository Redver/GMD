using UnityEngine;

public class Nation : MonoBehaviour
{
    private enum nationType
    {
        France,
        GreatBritain
    };
    private string nationName;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void onNationSelected(nationType nation)
    {
        nationName = nation.ToString();
    }
    
    
}
