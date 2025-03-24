using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Selector : MonoBehaviour
{
    [SerializeField] private GameObject selectedProvinceObject;
    [SerializeField] private Nation currentCountryNation;
    [SerializeField] private GameObject startProvince;
    [SerializeField] private Vector2 input;
    [SerializeField] private float cooldownBaseValue = 0.2f;
    [SerializeField] private float inputCooldown = 0.2f;
    [SerializeField] private float movementTime = 0.2f;
    [SerializeField] private bool unitSelected = false;
    
    void Start()
    {
        ChangeSelectionParent(startProvince);
        currentCountryNation = startProvince.GetComponentInParent<Nation>();
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
                GameObject hitProvinceObject = hit.collider.gameObject;
                Province hitProvince = hitProvinceObject.GetComponent<Province>();
                Province selectedProvince = selectedProvinceObject.GetComponent<Province>();
                if (hitProvinceObject != selectedProvinceObject && (hitProvinceObject.GetComponentInParent<Nation>() == currentCountryNation || hitProvinceObject.tag == "SeaTile" || unitSelected && hitProvince.getNeighbours().Contains(selectedProvince)))
                {
                    ChangeSelectionParent(hitProvinceObject);
                    return;
                }
            }
        }
    }

    public void onStartTurn(GameObject nation)
    {
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
        selectedProvinceObject = province;
        gameObject.transform.SetParent(selectedProvinceObject.transform);
        Vector3 oldPosition = gameObject.transform.position;
        Vector3 newPosition = gameObject.transform.parent.position;
        StartCoroutine(LerpSelector(oldPosition, newPosition, movementTime));
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
