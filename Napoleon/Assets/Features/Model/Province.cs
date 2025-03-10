using System;
using System.Collections.Generic;
using UnityEngine;

public class Province : MonoBehaviour
{
    [SerializeField] private List<Province> NeigbourProvinces = new List<Province>();

    private void Awake()
    {
        
    }
    
    void Start()
    {
        FindNeighbors();
    }

    void FindNeighbors()
    {
        BoxCollider col = GetComponent<BoxCollider>();
        if (col == null) return;

        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale, Quaternion.identity);
        int i = 0;
        while (i < hitColliders.Length)
        {
            Debug.Log("Hit from "+ gameObject.name+" : " + hitColliders[i].name + i);
            if (hitColliders[i].name != gameObject.name)
            {
                NeigbourProvinces.Add(hitColliders[i].GetComponent<Province>());
            }
            i++;
        }
        //if name = gameobject name ignore, else add to neigbours list
    }

    void Update()
    {
        
    }
}
