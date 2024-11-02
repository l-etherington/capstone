using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// to add: borders of map (esp in get neighbours)

public class GoalTracker : MonoBehaviour
{
    public int goalInd = 0;
    public int[,] goalList;
    public bool goalsLeft = true;

    public Vector2Int getNextGoal(){
        if (goalInd == goalList.Length/2) {
            goalsLeft = false;
            return new Vector2Int(0,0);
        }
        Vector2Int goal = new Vector2Int(goalList[goalInd, 0], goalList[goalInd, 1]);
        goalInd += 1;
        return goal;
    }

    // Start is called before the first frame update
    void Start()
    { 
    }

    // Update is called once per frame
    void Update()
    {
    }
}
