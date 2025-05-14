using System;
using System.Collections;
using Resources.map_assets.Selector_Scripts.SelectorMVP;
using UnityEngine;

public class Selectors : MonoBehaviour
{
    [SerializeField] private GameObject[] selectors;
    
    void Start()
    {
        selectors = GameObject.FindGameObjectsWithTag("Selectors");
        StartCoroutine(WaitOneSecond());
        OnEndTurn();
    }
    
    IEnumerator WaitOneSecond()
    {
        Debug.Log("Loading");
        yield return new WaitForSeconds(1f); // waits for 1 second
        Debug.Log("Done");
    }

    public void OnEndTurn()
    {
        GameObject otherSelector;
        GameObject playersSelector;
        if (selectors[0].activeSelf)
        {
            otherSelector = selectors[1];
            playersSelector = selectors[0];
        }
        else
        {
            otherSelector = selectors[0];
            playersSelector = selectors[1];
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
        selector.GetComponent<SelectorView>().startOnCapital();
    }

    void Update()
    {
        
    }
}
