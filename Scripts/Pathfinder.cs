using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// to add: borders of map (esp in get neighbours)

public class Pathfinder : MonoBehaviour
{
    public LevelManager ParentLevelManager;
    public MakeMap map;
    public bool floorsSwitched;
    public double maxUtility;
    public List<Vector2Int> maxUtilityPath;
    public double trust = 1.0;
    private double lambda;
    public IEnumerator pathfinderObj;
    public Vector2Int currentPosGlobal;

    public Level level;
    
    private int manhattan(Vector2Int pos, Vector2Int goal)
    {
        // manhattan distance
        return Math.Abs(pos[1]-goal.x + pos[0]-goal.y);
    }

    private IEnumerator move(List<Vector2Int> path){
        for(int i=0; i<path.Count; i++){
            yield return new WaitForSecondsRealtime(2);
            Vector3 moveTo = map.toMapPos(path[i][0],path[i][1]);
            currentPosGlobal = new Vector2Int(path[i][0], path[i][1]);
            transform.position = new Vector3(moveTo.x + map.Res().x/2, moveTo.y - map.Res().y/2, moveTo.z);
        }
    }

    public void recalculate(bool endOfScene, Level level, Dictionary<Vector2Int, List<Vector2Int>> graph = null){
    
        if (endOfScene){ // case where PC ends scene (collecting more tokens not possible)

            // stop movement
            StopCoroutine(pathfinderObj);

            // calculate difference between expected and current utilit
            int currentUtility = GetComponent<PointTracker>().points;
            double diff = maxUtility - currentUtility;

            // if difference is 0, add small boost to trust
            if (diff == 0) {
                // make sure trust does not exceed 1
                trust = Math.Max(1, trust + lambda*0.01);
            } else {
                // adjust diff by maxUtility magnitude, map between 0 and 1 with sigmoid
                // TODO: avoid maxUtility division by 0 error possibility
                trust -= lambda * (1/(1+Math.Exp(diff/maxUtility)));
            }
            // difference can never be positive in end of scene case

        } else { // case where PC changes gravity
            double oldUtil = maxUtility;

            Vector2Int newPos = map.fromMapPos(level.origin, transform.position);

            aStar(new Vector2Int(newPos[0], newPos[1]), graph, level);
            StopCoroutine(pathfinderObj);
            pathfinderObj = move(maxUtilityPath);
            StartCoroutine(pathfinderObj);

            // get difference between former and current max utility
            double diff = maxUtility - oldUtil;

            // add or subtract adjusted trust modifier
            trust += Math.Sign(diff) * lambda * (1/(1+Math.Exp(diff/maxUtility)));

            // should always be between 0 and 1
            trust = Math.Clamp(trust, 0, 1);
            
        }
    }

    private void aStar(Vector2Int currentPos, Dictionary<Vector2Int, List<Vector2Int>> graph, Level level)
    {   
        // initialize frontier and path
        PriorityQueue<Path> frontier = new PriorityQueue<Path>();
        Path path = new Path(new List<Vector2Int>(){currentPos}, 0, graph);
        frontier.Enqueue(path, path.utility);

        while (frontier.elements.Count > 0) {
            path = frontier.Dequeue();
            currentPos = path.nodes[path.nodes.Count-1];

            // base case: depth limit reached
            if(level.max_moves <= path.nodes.Count){
                if(path.utility > maxUtility){
                    maxUtility = path.utility;
                    maxUtilityPath = path.nodes;
                }
            } 
            
            // keep searching path
            else {

                // if button exists on node
                if(map.buttons.ContainsKey(currentPos)){

                    // modify map as needed given button 

                    // add edges
                    foreach(KeyValuePair<Vector2Int, List<Vector2Int>> kvp in map.buttons[currentPos].edgesAdded){
                        for(int j=0; j < kvp.Value.Count; j++){
                            path.graph[kvp.Key].Add(kvp.Value[j]);
                        }
                    }

                    // remove edges
                    foreach(KeyValuePair<Vector2Int, List<Vector2Int>> kvp in map.buttons[currentPos].edgesRemoved){
                        for(int j=0; j < kvp.Value.Count; j++){
                            path.graph[kvp.Key].Remove(kvp.Value[j]);
                        }
                    }
                }

                // get neighbours
                List<Vector2Int> neighbours = path.graph[currentPos];

                for (int i = 0; i < neighbours.Count; i++){
                    double utility = 0;
                    bool add = true;
                    // check if seen more than twice
                    if (path.seen.ContainsKey(neighbours[i])){

                        // discontinue path if node seen twice
                        if (path.seen[neighbours[i]] == true){
                            add = false;
                            continue;
                        } else {
                            // increment number of times seen
                            path.seen[neighbours[i]] = true;
                            utility -= 5;
                        }
                    } else {
                        // check for utility modifier of node
                        if(level.goblin_tokens.ContainsKey(neighbours[i])){
                            utility += 10;
                        } else if(map.buttons.ContainsKey(neighbours[i])){
                            utility += trust * (double)map.buttons[neighbours[i]].utility;
                        }
                        // increment number of times seen
                        path.seen.Add(neighbours[i], false);
                    }

                    if (add){
                        Path newPath = new Path(path.nodes, path.utility+utility, path.graph);
                        newPath.nodes.Add(neighbours[i]);
                        frontier.Enqueue(newPath, newPath.utility);
                    }
                    
                }
            }
        }
    }

    // Start is called before the first frame update
    public void Start()
    { 
        level = ParentLevelManager.currentLevel;
        floorsSwitched = false;
        maxUtility = 0;
        transform.position = map.toMapPos(level.NPCstart[0], level.NPCstart[1]);
        aStar(new Vector2Int(level.NPCstart[0], level.NPCstart[1]), map.gravityDownGraph, level);

        pathfinderObj = move(maxUtilityPath);
        StartCoroutine(pathfinderObj);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col){
        if (col.CompareTag("button") && floorsSwitched == false)
        {
            // switchFloorsDo(map.switchFloors["H"], "H");
            // switchFloorsDo(map.switchFloors["V"], "V");
            switchFloorsDo(col.gameObject.GetComponent<Interactable>().hfloors, "H");
            switchFloorsDo(col.gameObject.GetComponent<Interactable>().vfloors, "H");
            col.gameObject.SetActive(false);
        }
    }

    void switchFloorsDo(List<GameObject> lst, string dir){

        for(int i=0; i<lst.Count; i++){
            
            GameObject block = lst[i];
            Vector3 position = block.transform.position;
            Vector3 scale;
            if(dir == "H"){
                scale = new Vector3(map.Res().x/4, map.Res().y, 1);
                position.x = position.x - 5;
            }else{
                scale = new Vector3(map.Res().x, map.Res().y/4, 1);
                position.x = position.x + 5;
            }
            block.transform.localScale = scale;
            block.transform.position = position;
        }
    }
}
