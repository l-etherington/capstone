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
    public LevelManager ParentLevelManager;
    public static Level level;
    public TMP_Text coinText;
    public int totalCoins;

    public void Awake()
    {
        coinsCollected = 0;
        level = ParentLevelManager.currentLevel;
        totalCoins = level.goat_tokens.Count;
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

    }
}