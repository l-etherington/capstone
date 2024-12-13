using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Endzone : MonoBehaviour
{
    //Keep track of total picked coins (Since the value is static, it can be accessed at "SC_2DCoin.totalCoins" from any script)
    public int coinsCollected;
    public LevelManager ParentLevelManager;
    public Level level;
    public GameObject Player;
    public bool canWin;

    public void Start()
    {
        level = ParentLevelManager.currentLevel;
        canWin = false;
    }

    void Update()
    {
        if(Player.GetComponent<PlayerCollider>().coinsCollected == Player.GetComponent<PlayerCollider>().totalCoins-1){
            gameObject.transform.GetComponent<SpriteRenderer>().material.color = Color.green;
            canWin = true;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && canWin){
            ParentLevelManager.ChangeLevel();
            gameObject.transform.GetComponent<SpriteRenderer>().material.color = Color.white;
            canWin = false;
        }

    }
}