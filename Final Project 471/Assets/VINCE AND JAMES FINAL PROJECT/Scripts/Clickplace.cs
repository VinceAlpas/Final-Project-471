using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickplace : MonoBehaviour
{
    public Transform cloneObj;
    public int foodValue;

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
        if(gameObject.name== "bunBottom")
        Instantiate(cloneObj, new Vector3(0, .10f, 0), cloneObj.rotation);

        if(gameObject.name== "bunTop")
        Instantiate(cloneObj, new Vector3(0, .60f, 0), cloneObj.rotation);

        if(gameObject.name== "Cheese")
        Instantiate(cloneObj, new Vector3(0, .62f, -.05f), cloneObj.rotation);

        if(gameObject.name== "Egg")
        {
            Instantiate(cloneObj, new Vector3(-.1f, .62f, 0), cloneObj.rotation);
            Instantiate(cloneObj, new Vector3(.1f, .62f, 0), cloneObj.rotation);
        }

        gameflow.plateValue += foodValue;
        Debug.Log(gameflow.plateValue+"  "+gameflow.orderValue);
    }
}
