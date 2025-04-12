using System;
using UnityEngine;

public class Nation : MonoBehaviour
{
    private string nationName;
    private float treasurey;
    [SerializeField] private int provinceCount;
    [SerializeField] GameObject capitalProvince;

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

    public void getNewCapitalProvince()
    {
        capitalProvince = transform.GetChild(0).gameObject;
    }

    public GameObject getCurrentCapitalProvince()
    {
        return capitalProvince;
    }

    public void onStartTurn()
    {
        if (capitalProvince.GetComponent<Province>().getOwner().nationName != this.nationName)
        {
            getNewCapitalProvince();
        }
    }

    public string getName()
    {
        return nationName;
    }

}
