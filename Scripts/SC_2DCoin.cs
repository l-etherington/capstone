using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_2DCoin : MonoBehaviour
{
    //Keep track of total picked coins (Since the value is static, it can be accessed at "SC_2DCoin.totalCoins" from any script)
    public static int totalCoins = 0; 
    public string canDestroyTag;

    void Awake()
    {
        totalCoins = 0;
        //Make Collider2D as trigger 
        GetComponent<Collider2D>().isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D c2d)
    {
        //Destroy the coin if Object tagged Player comes in contact with it
        if (c2d.CompareTag(canDestroyTag))
        {
            //Add coin to counter
            totalCoins++;

            //Test: Print total number of coins
            Debug.Log(canDestroyTag + " currently has " + SC_2DCoin.totalCoins + " tokens.");
            //Destroy coin
            Destroy(gameObject);
        }
    }
}