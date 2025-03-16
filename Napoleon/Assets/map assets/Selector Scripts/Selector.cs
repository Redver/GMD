using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Selector : MonoBehaviour
{
    private GameObject selectedProvince;
    private GameObject currentCountryNation;
    [SerializeField] private GameObject startProvince;
    [SerializeField] private Vector2 input;
    private Dictionary<Province, float> directions = new Dictionary<Province, float>();
    private float cooldownBaseValue = 0.5f;
    [SerializeField] private float inputCooldown = 0.5f;
    [SerializeField] private float provinceDirection; //remove after testing

    
    void Start()
    {
        ChangeSelectionParent(startProvince);
        StartCoroutine(startCooldown());
    }

    void Update()
    {
        
    }
    
    public void OnInput(InputAction.CallbackContext context)
    {
        if (inputCooldown == 0f)
        {
            this.input = context.ReadValue<Vector2>();
            moveSelector(input);
            inputCooldown = cooldownBaseValue;
            StartCoroutine(startCooldown());
        }
    }
    
    private IEnumerator startCooldown()
    {
        while (inputCooldown > 0f)
        {
            inputCooldown -= Time.deltaTime; 

            yield return null;
        }

        inputCooldown = 0f;
    }

    private void moveSelector(Vector2 input)
    {
        Province toMoveTo = gameObject.GetComponent<Province>();
        float angleOfInput = Vector2.SignedAngle(Vector2.up, input);
        float lowestDistance = float.MaxValue;
        foreach (var keyValue in directions)
        {
            float angle = keyValue.Value;
            float diff = Math.Abs(angle - angleOfInput);
            if (diff < lowestDistance)
            {
                lowestDistance = diff;
                toMoveTo = keyValue.Key;
            }
        }
        provinceDirection = directions[toMoveTo];
        ChangeSelectionParent(toMoveTo.gameObject);
    }

    private void getNeighbourAngles()
    {
        List<Province> neighbours = selectedProvince.GetComponent<Province>().getNeighbours();
        Dictionary<Province, float> directions = new Dictionary<Province, float>();

        foreach (var province in neighbours)
        {
            Vector2 direction = (province.transform.position - selectedProvince.transform.position);
            float angle = Vector2.SignedAngle(Vector2.up, direction); 
            directions.Add(province, angle);
        }

        this.directions = directions; 
    }

    public void onChangeTurn(GameObject nation)
    {
        currentCountryNation = nation;
        startOnCapital(nation);
    }

    public void startOnCapital(GameObject nation)
    {
        GameObject capitalProvince = new GameObject();
        switch (nation.GetComponent<Nation>().name)
        {
            case "France":
                capitalProvince = GameObject.Find("Iledefrance_0");
                break;
            case "GreatBritain":
                capitalProvince = GameObject.Find("SouthEast_0");
                break;
        }
        ChangeSelectionParent(capitalProvince);
    }

    public void ChangeSelectionParent(GameObject province)
    {
        selectedProvince = province;
        gameObject.transform.SetParent(selectedProvince.transform);
        Vector3 oldPosition = gameObject.transform.position;
        Vector3 newPosition = gameObject.transform.parent.position;
        StartCoroutine(LerpSelector(oldPosition, newPosition, 0.2f));
        getNeighbourAngles();
    }

    private IEnumerator LerpSelector(Vector3 from, Vector3 to, float timeForMovement)
    {
        float elapsedTime = 0f;
        while (elapsedTime < timeForMovement)
        {
            gameObject.transform.position = Vector3.Lerp(from, to, elapsedTime / timeForMovement);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        gameObject.transform.position = to;
    }
}
