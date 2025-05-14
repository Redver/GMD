using System;
using UnityEngine;
using UnityEngine.Events;

public class Nation : MonoBehaviour
{
    private string nationName;
    private float treasurey;
    private int unitCount;
    private int boatCount;
    [SerializeField] private int provinceCount;
    [SerializeField] GameObject capitalProvince;
    public UnityEvent endTurnEvent; 


    private void Awake()
    {
        nationName = gameObject.name;
        if (endTurnEvent == null)
        {
            endTurnEvent = new UnityEvent();
        }
    }

    void Start()
    {
    }

    void Update()
    {
        
    }

    public void updateTreasurey()
    {
        float income = provinceCount * 3;
        float expenses = unitCount + (boatCount * 3);
        float net = income - expenses;
        treasurey += net;
    }

    public void payForUnit()
    {
        treasurey -= 10;
    }
    
    public void payForBoat()
    {
        treasurey -= 15;
    }


    public void onEndTurn()
    { 
        updateProvinceCount();
        updateTreasurey();
        endTurnEvent.Invoke();
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
