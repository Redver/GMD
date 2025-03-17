using System;
using UnityEngine;

public class Nation : MonoBehaviour
{
    private string nationName;
    private float treasurey;
    [SerializeField] private int provinceCount;

    private void Awake()
    {
        nationName = gameObject.name;
    }

    void Start()
    {

    }

    void Update()
    {
        
    }

    public void onEndTurn()
    {
        updateProvinceCount();
    }

    public void updateProvinceCount()
    {
        this.provinceCount = transform.childCount;
    }

    public void onStartTurn()
    {
        new NotImplementedException();
    }

    public string getName()
    {
        return nationName;
    }

}
