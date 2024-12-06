using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheckScript : MonoBehaviour
{

    public bool onGround = true;

    void Start(){}
    
    void OnTriggerEnter2D(Collider2D col)
    {
        // When trigger meets collider, if ground, change onGround variable
        if (col.CompareTag("ground"))
        {
            onGround = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        // Change onGround variable once leaving ground
        if (col.CompareTag("ground"))
        {
            onGround = false;
        }
    }

    // Update is called once per frame
    void Update(){
    }
}
