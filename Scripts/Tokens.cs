using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tokens : MonoBehaviour
{
    public static int totalCoins; 
    public string canDestroyTag;

    void Awake()
    {
        //Make Collider2D as trigger 
        GetComponent<Collider2D>().isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //Destroy the coin if Object tagged Player comes in contact with it
        if (col.CompareTag(canDestroyTag))
        {
            //Test: Print total number of coins
            Debug.Log(canDestroyTag + " currently has " + totalCoins + " tokens.");
            //Destroy coin
            Destroy(gameObject);
        }
    }
}