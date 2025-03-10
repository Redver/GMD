using System;
using System.Collections.Generic;
using UnityEngine;

public class Province : MonoBehaviour
{
    [SerializeField] private List<Province> NeigbourProvinces = new List<Province>();
    private LayerMask provinceLayer;
    private LayerMask seaLayer;
    private Nation owner;
    [SerializeField] private GameObject startingOwner;
    private int friendlyUnitCount;
    private int enemyUnitCount;
    private SpriteRenderer sr;
    private Dictionary<string, GameObject> Nations;

    private void Awake()
    {
        provinceLayer = LayerMask.GetMask("Province");
        seaLayer = LayerMask.GetMask("SeaTile");
        this.sr = gameObject.GetComponent<SpriteRenderer>();
        if (startingOwner != null)
        {
            owner = startingOwner.GetComponent<Nation>();
            Debug.Log(owner.name);
        }
    }
    
    void Start()
    {
        
        Nations = new Dictionary<string, GameObject>();
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Nation");
        foreach (GameObject nation in temp)
        {
            Nations.Add(nation.name, nation);
        }
        FindNeighbors();
        if (owner != null)
        {
            onChangedOwner(owner);
        }
    }

    void FindNeighbors()
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
                        Debug.Log($"{gameObject.name} added BLOCKER neighbor: {hit.name}");
                    }
                    else if (!IsBlocked(neighbor) || gameObject.layer == LayerMask.NameToLayer("SeaTile"))
                    {
                        NeigbourProvinces.Add(neighbor);
                        Debug.Log($"{gameObject.name} added land neighbor: {hit.name}");
                    }
                }
            }
        }
    }

    bool IsBlocked(Province neighbor)
    { 
        Vector2 start = transform.position;
        Vector2 end = neighbor.transform.position;

        RaycastHit2D hit = Physics2D.Raycast(start, (end - start).normalized, Vector2.Distance(start, end), seaLayer);

        return hit.collider != null; 
    }

    void onChangedOwner(Nation owner)
    { 
        Debug.Log($"{gameObject.name} owned {owner.name}");
        this.owner = owner;
        switch (owner.name)
        {
            case "France": sr.color = Color.blue; break;
            case "GreatBritain": sr.color = Color.red; break;
        }
        gameObject.transform.SetParent(Nations[owner.name].transform);
    }

    void Update()
    {
        
    }
}
