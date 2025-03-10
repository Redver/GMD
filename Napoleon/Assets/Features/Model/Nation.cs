using System;
using UnityEngine;

public class Nation : MonoBehaviour
{
    private string nationName;
    private float treasurey;
    private int provinceCount;

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

    public void updateProvinceCount()
    {
        this.provinceCount = transform.childCount;
    }


    public string getName()
    {
        return nationName;
    }

}
