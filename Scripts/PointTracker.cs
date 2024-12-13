using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTracker: MonoBehaviour
{
    public int points;


    void Start() {
        points = 0;
    }

    void OnTriggerEnter2D(Collider2D col){
        if (col.CompareTag("gobTok")){
            points ++;
        }
    }
}