using System;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    private void Awake()
    {
    }

    public void onInput()
    {
        this.gameObject.transform.position = new Vector3(-1.5f,0,0);
    }
}
