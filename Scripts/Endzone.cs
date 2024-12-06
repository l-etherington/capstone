using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Endzone : MonoBehaviour
{
    //Keep track of total picked coins (Since the value is static, it can be accessed at "SC_2DCoin.totalCoins" from any script)
    public int coinsCollected;
    public static int totalCoins = Level1.goat_tokens.Length;
    public GameObject coinCounter;
    public bool canWin = false;
    public GameObject winText;

    void Awake()
    {
        Debug.Log(totalCoins);
    }

    void Update()
    {
        if(coinCounter.GetComponent<PlayerCollider>().coinsCollected == PlayerCollider.totalCoins-1){
            gameObject.transform.GetComponent<SpriteRenderer>().material.color = Color.green;
            canWin = true;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && canWin){
            winText.SetActive(true);
        }

    }
}