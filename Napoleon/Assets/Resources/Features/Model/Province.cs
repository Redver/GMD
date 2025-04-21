using System;
using System.Collections.Generic;
using Resources.Features.Model.Units;
using UnityEditor.PackageManager;
using UnityEngine;

public class Province : MonoBehaviour
{
    [SerializeField] private List<Province> NeigbourProvinces = new List<Province>();
    private LayerMask provinceLayer;
    private LayerMask seaLayer;
    private Nation owner;
    [SerializeField] private GameObject startingOwner;
    private Stack<IUnit> unitStack = new Stack<IUnit>();
    [SerializeField] private int friendlyUnitCount;
    private int enemyUnitCount;
    private SpriteRenderer sr;
    private Dictionary<string, GameObject> Nations = new Dictionary<string, GameObject>();

    private void Awake()
    {
        provinceLayer = LayerMask.GetMask("Province");
        seaLayer = LayerMask.GetMask("SeaTile");
        sr = gameObject.GetComponent<SpriteRenderer>();
        if (startingOwner != null)
        {
            owner = startingOwner.GetComponent<Nation>();
        }
        FindNeighbors();
        if (owner != null)
        {
            setStartingOwner();
        }
    }
    
    void Start()
    {
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Nation");
        foreach (GameObject nation in temp)
        {
            Nation nationScript = nation.GetComponent<Nation>();
            Nations.Add(nationScript.getName(), nation);
        }
    }

    public bool canSelectUnit()
    {
        if (unitStack.Count > 0)
        {
            Queue<IUnit> unitQueue = new Queue<IUnit>();
            IUnit canUnit = null;
                
            while (unitStack.Count > 0)
            {
                IUnit unit = unitStack.Pop();

                if (unit.canSelect())
                {
                    canUnit = unit;
                    break;
                }
                unitQueue.Enqueue(unit);
            }
                
            foreach (var unit in unitQueue)
            {
                unitStack.Push(unit);
            }
                
            if (canUnit != null)
            {
                unitStack.Push(canUnit);
                return true;
            }
        }
        return false;
    }

    public void addUnitToStack(IUnit unit)
    {
        unitStack.Push(unit);
        updateUnitCount();
    }

    public void spreadUnits()
    {
        int unitCount = unitStack.Count;
        if (unitCount == 0) return;

        Collider2D col = GetComponent<Collider2D>();
        if (col == null) return;

        Bounds bounds = col.bounds;
        float totalWidth = bounds.size.x * 0.4f;
        float spacing = (unitCount > 1) ? totalWidth / (unitCount - 1) : 0;

        Vector3 startPos = new Vector3(bounds.center.x - totalWidth / 2f, bounds.center.y, transform.position.z);

        IUnit[] units = unitStack.ToArray();

        for (int i = 0; i < unitCount; i++)
        {
            Vector3 targetPos;

            if (unitCount == 1)
            {
                targetPos = new Vector3(bounds.center.x, bounds.center.y, transform.position.z);
            }
            else
            {
                float x = startPos.x + spacing * i;
                targetPos = new Vector3(x, bounds.center.y, transform.position.z);
            }

            GameObject unitObj = ((Unit)units[i]).getView().gameObject;
            unitObj.transform.SetParent(this.transform);
            unitObj.transform.position = targetPos;
        }
    }

    public GameObject getSelector()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Selectors"))
            {
                return child.gameObject;
            }
        }
        throw new Exception("Selectors not found");
    }

    public void updateUnitCount()
    {
        this.friendlyUnitCount = unitStack.Count;
        spreadUnits();
    }

    public IUnit selectNextUnit()
    {
        IUnit unit = unitStack.Pop();
        updateUnitCount();
        return unit;
    }

    public void deselectUnit(IUnit unit)
    {
        unitStack.Push(unit);
        updateUnitCount();
    }

    public void FindNeighbors()
    {
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        if (col == null) return;
        Vector2 position = col.bounds.center;
        Vector2 size = new Vector2(col.bounds.size.x, col.bounds.size.y);
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(position, size, 0);
        foreach (Collider2D hit in hitColliders)
        {
            if (hit.gameObject == gameObject)
            {
            }
            else
            {
                Province neighbor = hit.GetComponent<Province>();
                if (neighbor != null && !NeigbourProvinces.Contains(neighbor))
                {
                    if (hit.gameObject.layer == LayerMask.NameToLayer("SeaTile"))
                    {
                        NeigbourProvinces.Add(neighbor);
                    }
                    else if (!IsBlocked(neighbor) || gameObject.layer == LayerMask.NameToLayer("SeaTile"))
                    {
                        NeigbourProvinces.Add(neighbor);
                    }
                }
            }
        }
    }

    public bool IsBlocked(Province neighbor)
    { 
        Vector2 start = transform.position;
        Vector2 end = neighbor.transform.position;

        RaycastHit2D hit = Physics2D.Raycast(start, (end - start).normalized, Vector2.Distance(start, end), seaLayer);

        return hit.collider != null; 
    }

    public void setStartingOwner()
    {
        switch (owner.name)
        {
            case "France": sr.color = Color.blue; break;
            case "GreatBritain": sr.color = Color.red; break;
        }
        gameObject.transform.SetParent(startingOwner.transform);
    }

    public void onChangedOwner(Nation owner)
    { 
        this.owner = owner;
        switch (owner.name)
        {
            case "France": sr.color = Color.blue; break;
            case "GreatBritain": sr.color = Color.red; break;
        }
        gameObject.transform.SetParent(Nations[owner.getName()].transform);
    }
    
    public List<Province> getNeighbours()
    {
        return NeigbourProvinces;
    }

    public Nation getOwner()
    {
        return owner;
    }

    public GameObject getOwnerGameObject()
    {
        return this.transform.parent.gameObject;
    }

    public int getFriendlyUnitCount()
    {
        return friendlyUnitCount;
    }

    void Update()
    {
        
    }
}
