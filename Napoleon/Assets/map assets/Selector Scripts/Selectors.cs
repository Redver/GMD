using System;
using UnityEngine;

public class Selectors : MonoBehaviour
{
    [SerializeField] private GameObject[] selectors;
    
    void Start()
    {
        selectors = GameObject.FindGameObjectsWithTag("Selectors");
        OnEndTurn(selectors[1]);
    }
    
    public void OnEndTurn(GameObject playersSelector)
    {
        Debug.Log("End Turn selector called!");
        GameObject otherSelector;

        if (selectors[0].activeSelf)
        {
            otherSelector = selectors[1];
        }
        else
        {
            otherSelector = selectors[0];
        }
        activateSelector(otherSelector);
        deactivateSelector(playersSelector);
    }

    public void deactivateSelector(GameObject selector)
    {
        selector.SetActive(false);
    }

    public void activateSelector(GameObject selector)
    {
        selector.SetActive(true);
        selector.GetComponent<Selector>().startNewTurnOnCurrentNationCapital();
    }

    void Update()
    {
        
    }
}
