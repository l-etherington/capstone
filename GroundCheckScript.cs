using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheckScript : MonoBehaviour
{

    public bool onGround = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
        //Destroy the coin if Object tagged Player comes in contact with it
        if (col.CompareTag("ground"))
        {
            onGround = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        //Destroy the coin if Object tagged Player comes in contact with it
        if (col.CompareTag("ground"))
        {
            onGround = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
