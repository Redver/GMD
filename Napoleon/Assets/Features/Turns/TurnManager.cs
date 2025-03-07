using System;
using System.Collections.Generic;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    private int playerTurn;
    [SerializeField] private List<GameObject> _playersList;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void Awake()
    {
        _playersList.Add();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
