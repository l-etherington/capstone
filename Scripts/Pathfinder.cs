using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// to add: borders of map (esp in get neighbours)

public class Pathfinder : MonoBehaviour
{
    public GoalTracker goals;
    public MakeMap map;
    public int[,] goalList = Level1.goblin_tokens;
    
    private int manhattan(Vector2Int pos, Vector2Int goal)
    {
        // manhattan distance
        return Math.Abs(pos[1]-goal.x + pos[0]-goal.y);
    }

    private IEnumerator aStar(Vector2Int start, Dictionary<Vector2Int, List<Vector2Int>> graph, int xRes, int yRes)
    {   
        Vector2Int currentPos = new Vector2Int(start[0], start[1]);
        for(int goalInd=0; goalInd<goalList.Length/2; goalInd++){
            Vector2Int goal = new Vector2Int(goalList[goalInd,0], goalList[goalInd,1]);
            yield return new WaitForSecondsRealtime(1);
            // initialize frontier
            PriorityQueue<Vector2Int> frontier = new PriorityQueue<Vector2Int>();
            List<Vector2Int> neighbours = graph[currentPos];
            for (int i = 0; i < neighbours.Count; i++){
                frontier.Enqueue(neighbours[i], manhattan(neighbours[i], goal));
            }
            yield return new WaitForSecondsRealtime(5);
            // go through frontier until path is found
            bool found = false;
            while (found == false && frontier.elements.Count > 0){
                currentPos = frontier.Dequeue();
                Vector3 moveTo = map.toMapPos(currentPos[0],currentPos[1],xRes,yRes);
                transform.position = new Vector3(moveTo.x + xRes/2, moveTo.y - yRes/2, moveTo.z);
                // Debug.Log("A-Star");
                // Debug.Log(pos);
                if(currentPos[1] == goal.y && currentPos[0] == goal.x){
                    found = true;
                } else {
                    yield return new WaitForSecondsRealtime(2);
                    neighbours = graph[new Vector2Int(currentPos[0], currentPos[1])];
                    for (int i = 0; i < neighbours.Count; i++){
                        frontier.Enqueue(neighbours[i], manhattan(neighbours[i], goal));
                    }
                }   
            }
            
        }
    }

    // Start is called before the first frame update
    void Start()
    { 
        goals.goalList = Level1.goblin_tokens;
        int xRes = (int)map.mapSize[0] / MakeMap.horizontal.GetLength(1);
        int yRes = (int)map.mapSize[1] / MakeMap.horizontal.GetLength(0);
        // define starting position
        transform.position = map.toMapPos(MakeMap.horizontal.GetLength(1)-1,MakeMap.horizontal.GetLength(0)-2,xRes,yRes);
        StartCoroutine(aStar(new Vector2Int(1,1), map.gravityDownGraph, xRes, yRes));
    }

    // Update is called once per frame
    void Update()
    {
    }
}
