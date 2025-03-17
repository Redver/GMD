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
    private float cooldownBaseValue = 0.2f;
    [SerializeField] private float inputCooldown = 0.2f;
    private float movementTime = 0.2f;
    
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
        Vector3 rayDirection = new Vector3(input.x, input.y, 0f).normalized; 
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, rayDirection, 100f);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null)
            {
                GameObject hitProvince = hit.collider.gameObject;

                if (hitProvince != selectedProvince)
                {
                    ChangeSelectionParent(hitProvince);
                    return;
                }
            }
        }
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
        StartCoroutine(LerpSelector(oldPosition, newPosition, movementTime));
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
