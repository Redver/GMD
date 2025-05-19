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

    public void putBoatsAtBottomOfSea()
    {
        Stack<IUnit> infStack = new Stack<IUnit>();
        Stack<IUnit> boatStack = new Stack<IUnit>();
        while(unitStack.Count > 0)
        {
            IUnit topUnit = unitStack.Pop();
            if (topUnit.IsBoat())
            {
                boatStack.Push(topUnit);
            }
            else
            {
                infStack.Push(topUnit);
            }
        }
        while (boatStack.Count > 0)
        {
            unitStack.Push(boatStack.Pop());
        }
        while (infStack.Count > 0)
        {
            unitStack.Push(infStack.Pop());
        }
    }

    public void spreadUnits()
    {
        putBoatsAtBottomOfSea();

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

            GameObject unitObj = ((IUnit)units[i]).getView().gameObject;
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

    public bool isCoastal()
    {
        foreach (Province neigh in NeigbourProvinces)
        {
            if (neigh.gameObject.CompareTag("SeaTile") && this.gameObject.CompareTag("Province"))
            {
                return true;
            }
        }
        return false;
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
        PolygonCollider2D poly = GetComponent<PolygonCollider2D>();
        if (poly == null) return;

        Bounds bounds = poly.bounds;
        Vector2 center = bounds.center;
        Vector2 size = bounds.size * 0.9f;

        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(center, size, 0f);

        foreach (Collider2D hit in hitColliders)
        {
            if (hit.gameObject == gameObject) continue;

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

    public int
        findDistanceBetween(
            Province target) //implementing breadth first search algorithm from medium.com, modified to do with provinces
    {
        List<Province> visited = new List<Province>();

        Queue<(Province province, int distance)> queue = new Queue<(Province, int)>();

        if (this == target)
        {
            return 0;
        }

        queue.Enqueue((this, 0));
        visited.Add(this);

        while (queue.Count > 0)
        {
            var (current, distance) = queue.Dequeue();

            foreach (Province neighbor in current.NeigbourProvinces)
            {
                if (neighbor == target)
                    return distance + 1;

                if (!visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    queue.Enqueue((neighbor, distance + 1));
                }
            }
        }

        return -1;
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
    
    public bool hasBoat()
    {
        if (unitStack.Count > 0)
        {
            foreach (var unit in unitStack)
            {
                if (unit.IsBoat())
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void removeThisUnitFromStack(IUnit unitToRemove)
    {
        Stack<IUnit> tempUnitStack = new Stack<IUnit>();
        while (unitStack.Contains(unitToRemove))
        {
            tempUnitStack.Push(unitStack.Pop());
            if (tempUnitStack.Peek() == unitToRemove)
            {
                tempUnitStack.Pop();
            }
        }

        while (tempUnitStack.Count > 0)
        {
            unitStack.Push(tempUnitStack.Pop());
        }
    }

    public (Stack<IUnit> friendlyStack, Stack<IUnit> enemyStack) splitUnitStackIntoNations()
    {
        Stack<IUnit> tempUnitStack = new Stack<IUnit>();
        Stack<IUnit> friendlyUnitStack = new Stack<IUnit>();
        Stack<IUnit> enemyUnitStack = new Stack<IUnit>();

        while (unitStack.Count > 0)
        {
            IUnit unit = unitStack.Pop();
            if (unit.getNation() == owner)
            {
                friendlyUnitStack.Push(unit);
            }
            else
            {
                enemyUnitStack.Push(unit);
            }

            tempUnitStack.Push(unit); 
        }

        while (tempUnitStack.Count > 0)
        {
            unitStack.Push(tempUnitStack.Pop());
        }

        return (friendlyUnitStack, enemyUnitStack);
    }


    void Update()
    {
        
    }
}
