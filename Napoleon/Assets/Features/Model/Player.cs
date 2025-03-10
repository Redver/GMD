using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    private Nation Nation;

    void Start()
    {
        
    }

    private void Awake()
    {

    }

    void Update()
    {
        
    }

    void SetNation(Nation nation)
    {
        Nation = nation;
    }

    void onEndTurn()
    {
        Nation.updateProvinceCount();
    }
}
