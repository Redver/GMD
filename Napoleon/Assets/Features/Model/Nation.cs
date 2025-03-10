using UnityEngine;

public class Nation : MonoBehaviour
{
    private string nationName;
    private float treasurey;
    private int provinceCount;
    
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void onNationSelected(string nationName)
    {
        this.nationName = nationName;
    }

    void updateProvinceCount()
    {
        
    }


    public string getName()
    {
        return nationName;
    }

}
