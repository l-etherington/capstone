using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerCollider : MonoBehaviour
{
    //Keep track of total picked coins (Since the value is static, it can be accessed at "SC_2DCoin.totalCoins" from any script)
    public int coinsCollected = 0;
    public static int totalCoins = Level1.goat_tokens.Length/2;
    public TMP_Text coinText;

    void Awake()
    {
        coinText.text = coinsCollected.ToString();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //Destroy the coin if Object tagged Player comes in contact with it
        if (col.CompareTag("pcTok"))
        {
            //Add coin to counter
            coinsCollected++;
            coinText.text = coinsCollected.ToString();
        }

        if (col.CompareTag("EndZone") && coinsCollected == totalCoins-1){
            Debug.Log("You win!");
        }

    }
}