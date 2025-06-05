using System;
using System.Collections.Generic;
using Resources.Features.Model.Units;
using Resources.Features.TimeTravel.Clock.SavedData;
using Resources.map_assets.Selector_Scripts.SelectorMVP;
using UnityEngine;

public class Province : MonoBehaviour
{
    [SerializeField] private List<Province> NeigbourProvinces = new List<Province>();
    [SerializeField] private GameObject infantryPrefab;
    [SerializeField] private GameObject boatPrefab;
    private LayerMask provinceLayer;
    private LayerMask seaLayer;
    private Nation owner;
    [SerializeField] private GameObject startingOwner;
    private Stack<IUnit> unitStack = new Stack<IUnit>();
    [SerializeField] private int friendlyUnitCount;
    private int enemyUnitCount;
    private SpriteRenderer sr;
    private Dictionary<string, GameObject> Nations = new Dictionary<string, GameObject>();
    private bool combatInProvince = false;

    public bool CombatInProvince
    {
        get => combatInProvince;
        set => combatInProvince = value;
    }

    private void Awake()
    {
        provinceLayer = LayerMask.GetMask(nameof(Province));
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
        addToEndTurnAsListeners();
    }

    void Start()
    {
        GameObject[] temp = GameObject.FindGameObjectsWithTag(nameof(Nation));
        foreach (GameObject nation in temp)
        {
            Nation nationScript = nation.GetComponent<Nation>();
            Nations.Add(nationScript.getName(), nation);
        }
    }

    public void firstCombat()
    {
        combatInProvince = true;
        foreach (var unit in unitStack)
        {
            unit.beginCombat();
        }
    }

    public void inCombatEndTurn()
    {
        if (this.gameObject.CompareTag("SeaTile"))
        {
            navalCombatTurn();
        }

        if (this.gameObject.CompareTag(nameof(Province)))
        {
            landCombatTurn();
        }
    }

    public Stack<IUnit> removeBoatsFromStack(Stack<IUnit> stack)
    {
        Stack<IUnit> removeStack = new Stack<IUnit>();
        Stack<IUnit> tempStack = new Stack<IUnit>();
        while (stack.Count > 0)
        {
            if (stack.Peek().IsBoat())
            {
                removeStack.Push(stack.Pop());
            }
            else
            {
                tempStack.Push(stack.Pop());
            }
        }
        while (tempStack.Count > 0)
        {
            stack.Push(tempStack.Pop());
        }
        return removeStack;
    }

    public void combatTurn(Stack<IUnit> friendlyStack, Stack<IUnit> enemyStack)
    {
        if (friendlyStack.Count > 0 && enemyStack.Count > 0)
        {
            friendlyStack.Pop().destroy();
            enemyStack.Pop().destroy();
            foreach (var unit in friendlyStack)
            {
                unitStack.Push(unit);
            }
            foreach (var unit in enemyStack)
            {
                unitStack.Push(unit);
            }
        }
        if (friendlyStack.Count <= 0 || enemyStack.Count <= 0)
        {
            foreach (var unit in unitStack)
            {
                unit.endCombat();
            }
            combatInProvince = false;
        }
    }

    public void navalCombatTurn()
    {
        var (friendlies, enemies) = SplitUnitStackByNation();
        unitStack.Clear();
        Stack<IUnit> friendlyCombatUnits = removeBoatsFromStack(friendlies);
        Stack<IUnit> enemyCombatUnits = removeBoatsFromStack(enemies);
        foreach (var unit in friendlies)
        {
            unitStack.Push(unit);
        }
        foreach (var unit in enemies)
        {
            unitStack.Push(unit);
        }
        combatTurn(friendlyCombatUnits,enemyCombatUnits);
    }
    
    

    public void landCombatTurn()
    {         
        var (friendlies, enemies) = SplitUnitStackByNation();
        unitStack.Clear();
        Stack<IUnit> friendlyBoatsUnits = removeBoatsFromStack(friendlies);
        Stack<IUnit> enemyBoatsUnits = removeBoatsFromStack(enemies);
        foreach (var unit in friendlyBoatsUnits)
        {
            unitStack.Push(unit);
        }
        foreach (var unit in enemyBoatsUnits)
        {
            unitStack.Push(unit);
        }
        combatTurn(friendlies,enemies);
    }

    public void onEndTurn()
    {
        if (combatInProvince)
        {
            inCombatEndTurn();
            spreadUnits();
        }
    }

    public bool isInCombat()
    {
        return combatInProvince;
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

                if (unit.canSelect() && getSelector().GetComponent<SelectorView>().getNation() == unit.getNation())
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
        Vector3 center = bounds.center;
        
        var (friendlyStack, enemyStack) = SplitUnitStackByNation();

        IUnit[] friendlyUnits = friendlyStack.ToArray();
        IUnit[] enemyUnits = enemyStack.ToArray();

        float friendlyY = center.y + (enemyUnits.Length > 0 ? 0.2f : 0f);
        float enemyY = center.y - 0.2f;
        
        
        SpreadSymmetrically(friendlyUnits, friendlyY, center ,totalWidth);
        SpreadSymmetrically(enemyUnits, enemyY, center , totalWidth);
    }
    
    private void SpreadSymmetrically(IUnit[] units, float y, Vector3 center ,float totalWidth)
    {
        int count = units.Length;
        if (count == 0) return;

        float spacing = (count > 1) ? totalWidth / (count - 1) : 0;

        for (int i = 0; i < count; i++)
        {
            float x;
            if (count == 1)
            {
                x = center.x;
            }
            else
            {
                x = center.x - totalWidth / 2f + spacing * i;
            }

            GameObject unitObj = units[i].getView().gameObject;
            unitObj.transform.SetParent(this.transform);
            unitObj.transform.position = new Vector3(x, y, transform.position.z);
        }
    }

    public GameObject getSelector()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag(nameof(Selectors)) && child.gameObject.activeInHierarchy)
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
            if (neigh.gameObject.CompareTag("SeaTile") && this.gameObject.CompareTag(nameof(Province)))
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
        Vector2 size = bounds.size * 1.1f;

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
        PolygonCollider2D myCollider = GetComponent<PolygonCollider2D>();
        PolygonCollider2D neighborCollider = neighbor.GetComponent<PolygonCollider2D>();

        if (myCollider == null || neighborCollider == null) return true;

        foreach (Vector2 myPoint in GetWorldPoints(myCollider))
        {
            foreach (Vector2 neighborPoint in GetWorldPoints(neighborCollider))
            {
                Vector2 direction = neighborPoint - myPoint;
                float distance = direction.magnitude;

                RaycastHit2D hit = Physics2D.Raycast(myPoint, direction.normalized, distance, seaLayer);
                if (hit.collider == null)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private List<Vector2> GetWorldPoints(PolygonCollider2D collider)
    {
        List<Vector2> worldPoints = new List<Vector2>();
        for (int i = 0; i < collider.pathCount; i++)
        {
            Vector2[] path = collider.GetPath(i);
            foreach (Vector2 point in path)
            {
                worldPoints.Add(collider.transform.TransformPoint(point));
            }
        }
        return worldPoints;
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

    public void clearOwner()
    {
        if (this.transform.CompareTag("Province"))
        {
            this.owner = null;
            sr.color = Color.white;
            gameObject.transform.SetParent(GameObject.Find("Land").transform);
        }
    }

    public void setLandOwner(Nation owner)
    {
        if (this.transform.CompareTag("Province"))
        {
            onChangedOwner(owner);
        }
    }

    public void deleteUnits()
    {
        UnitView[] unitChildren = gameObject.GetComponentsInChildren<UnitView>();
        foreach (var unit in unitChildren)
        {
            if (unit.getUnitLogic().isHeldBySelector())
            {
                
            }
            else
            {
                removeThisUnitFromStack(unit.getUnitLogic());
                Destroy(unit.gameObject);
            }
        }
        unitStack.Clear();
        updateUnitCount();
    }

    public void addToEndTurnAsListeners()
    {
        GameObject[] nationsGameObjects = GameObject.FindGameObjectsWithTag(nameof(Nation));
        foreach (GameObject nation in nationsGameObjects)
        {
            nation.GetComponent<Nation>().endTurnEvent.AddListener(this.onEndTurn);
        }
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
    
    public bool hasFriendlyBoat(IUnit unit)
    {
        if (unitStack.Count > 0)
        {
            foreach (var unitFromStack in unitStack)
            {
                if (unitFromStack.IsBoat() && unitFromStack.getNation() == unit.getNation())
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
    public (Stack<IUnit> friendlies, Stack<IUnit> enemies) SplitUnitStackByNation()
    {
        Stack<IUnit> friendlies = new Stack<IUnit>();
        Stack<IUnit> enemies = new Stack<IUnit>();
        Stack<IUnit> tempUnitStack = new Stack<IUnit>();

        Nation referenceNation = null;

        while (unitStack.Count > 0)
        {
            IUnit unit = unitStack.Pop();
            tempUnitStack.Push(unit);

            Nation unitNation = unit.getNation();

            if (owner == null)
            {
                if (referenceNation == null)
                {
                    referenceNation = unitNation;
                    friendlies.Push(unit);
                }
                else if (unitNation == referenceNation)
                {
                    friendlies.Push(unit);
                }
                else
                {
                    enemies.Push(unit);
                }
            }
            else
            {
                if (unitNation == owner)
                {
                    friendlies.Push(unit);
                }
                else
                {
                    enemies.Push(unit);
                }
            }
        }

        while (tempUnitStack.Count > 0)
        {
            unitStack.Push(tempUnitStack.Pop());
        }

        return (friendlies, enemies);
    }

    public void summonUnit(UnitData unitToSummon)
    {
        string path = "";
        GameObject prefabToSummon = null;
        IUnit unitImplementationToSummon = null;
        if (unitToSummon.IsBoat)
        {
            if (unitToSummon.Nation.name == "GreatBritain")
            {
                path = "Features/Model/Units/Unit Assets/uk boat";
            }

            if (unitToSummon.Nation.name == "France")
            {
                path = "Features/Model/Units/Unit Assets/French Boat";
            }

            unitImplementationToSummon = new Boat();
            prefabToSummon = boatPrefab;
        }
        else
        {
            if (unitToSummon.Nation.name == "GreatBritain")
            {
                path = "Features/Model/Units/Unit Assets/UK flag unit";
            }

            if (unitToSummon.Nation.name == "France")
            {
                path = "Features/Model/Units/Unit Assets/FRflag";
            }
            
            unitImplementationToSummon = new Infantry();
            prefabToSummon = infantryPrefab;
        }

        GameObject builtUnit = Instantiate(prefabToSummon);
        builtUnit.transform.localPosition = Vector3.zero;
        builtUnit.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = UnityEngine.Resources.Load<Sprite>(path);
        builtUnit.transform.GetComponent<UnitView>().InitWithNation(unitImplementationToSummon, this, unitToSummon.Nation);
        builtUnit.transform.localScale = Vector3.one * 0.02f;
        addUnitToStack(unitImplementationToSummon);
        if (unitToSummon.InCombat)
        {
            unitImplementationToSummon.setInCombat();
        }
        else
        {
            unitImplementationToSummon.setNotInCombat();
        }

        if (unitToSummon.Moves == 0)
        {
            unitImplementationToSummon.getView().greyOutUnit();
        }
        else
        {
            unitImplementationToSummon.setMoves(unitToSummon.Moves);
        }
    }

    void Update()
    {
        
    }
}
