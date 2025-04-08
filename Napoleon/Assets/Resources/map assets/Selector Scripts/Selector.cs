using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;

public class Selector : MonoBehaviour
{
    [SerializeField] private GameObject selectedProvinceObject;
    [SerializeField] private Nation currentCountryNation;
    [SerializeField] private GameObject startProvince;
    [SerializeField] private Vector2 input;
    [SerializeField] private float cooldownBaseValue = 0.2f;
    [SerializeField] private float inputCooldown = 0.0f;
    [SerializeField] private float movementTime = 0.2f;
    [SerializeField] private bool unitSelected = false;
    [SerializeField] private GameObject buildMenu;
    private Dictionary<string, GameObject> capitalProvinces;

    
    void Start()
    {
        ChangeSelectionParent(startProvince);
        currentCountryNation = startProvince.GetComponentInParent<Nation>();
        StartCoroutine(startCooldown());
        capitalProvinces = new Dictionary<string, GameObject>
        {
            { "France", GameObject.Find("Iledefrance_0") },
            { "GreatBritain", GameObject.Find("SouthEast_0") }
        };
    }

    void Update()
    {
        
    }

    public void ToggleBuildMenu()
    {
        if (isActiveAndEnabled)
        {
            if (buildMenu.GetComponent<BuilderMenuUI>().IsMenuOpen())
            {
                buildMenu.GetComponent<BuilderMenuUI>().setClose();
                buildMenu.SetActive(false);
            }
            else
            {
                buildMenu.SetActive(true);
                Vector3 menuPosition = this.transform.position;
                menuPosition.x += 0.8f;
                menuPosition.y += 0.3f;
                buildMenu.transform.position = menuPosition;
                buildMenu.GetComponent<BuilderMenuUI>().setOpen();
            }
        }
    }

    public void OnInput(InputAction.CallbackContext context)
    {
        if (this.isActiveAndEnabled && !buildMenu.GetComponent<BuilderMenuUI>().IsMenuOpen())
        {
            if (inputCooldown == 0f)
            {
                this.input = context.ReadValue<Vector2>();
                moveSelector(input);
                inputCooldown = cooldownBaseValue;
                StartCoroutine(startCooldown());
            }
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
        startOnCapital(nation.name);
        inputCooldown = 0;
    }

    public void startOnCapital(string nation)
    {
        if (capitalProvinces.TryGetValue(nation, out GameObject capitalProvince))
        {
            ChangeSelectionParent(capitalProvince);
        }
        else
        {
            UnityEngine.Debug.LogWarning($"Could not find capital for nation: {nation}");
        }
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

    public void startNewTurnOnCurrentNationCapital()
    {
        startOnCapital(currentCountryNation.name);
    }
}
