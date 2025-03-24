using UnityEngine;

public class Selectors : MonoBehaviour
{
    [SerializeField] private GameObject[] selectors;
    
    void Start()
    {
        selectors = GameObject.FindGameObjectsWithTag("Selectors");
        onEndTurn(selectors[1]);
    }

    public void onEndTurn(GameObject playersSelector)
    {
        GameObject otherSelector;
        if (selectors[0].activeSelf)
        {
            otherSelector = selectors[0];
        }
        else
        {
            otherSelector = selectors[1];
        }
        deactivateSelector(playersSelector);
        activateSelector(otherSelector);
    }

    public void deactivateSelector(GameObject selector)
    {
        selector.SetActive(false);
    }

    public void activateSelector(GameObject selector)
    {
        selector.SetActive(true);
    }

    void Update()
    {
        
    }
}
