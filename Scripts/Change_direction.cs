using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Directions : MonoBehaviour
{
    //Keep track of total picked coins (Since the value is static, it can be accessed at "SC_2DCoin.totalCoins" from any script)
    public static int totalCoins = 0; 

    void Awake()
    {
        totalCoins = 0;
        //Make Collider2D as trigger 
        GetComponent<Collider2D>().isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        //Destroy the coin if Object tagged Player comes in contact with it
        if (collider.CompareTag("Player"))
        {
            //Add coin to counter
            totalCoins++;
        }
    }
}