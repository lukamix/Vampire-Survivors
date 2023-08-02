using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject prefab;
    private void Start()
    {
        Spawn();
        //InvokeRepeating("Spawn", 0, 2);
    }
    private void Spawn()
    {
        Instantiate(prefab);
    }
}
