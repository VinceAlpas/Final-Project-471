using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class meatcon : MonoBehaviour
{
    public Transform cloneObj;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if(gameObject.name== "Meat")
        Instantiate(cloneObj, new Vector3(-3, .1f, 0), cloneObj.rotation);
    }
}
