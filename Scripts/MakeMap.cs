using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MakeMap : MonoBehaviour
{
    public GameObject rectanglePrefab;
    public GameObject goblinTokenPrefab;
    public GameObject goatTokenPrefab;
    public GameObject buttonPrefab;
    public GameObject arrowPrefab;
    public Vector2 origin;
    public Vector2 mapSize;
    public GameObject endzone_obj;
    
    public LevelManager ParentLevelManager;
    public Level level;
    public static int[,] horizontal;
    public static int[,] vertical;
    public Dictionary<Vector2Int, List<Vector2Int>> gravityDownGraph;
    public Dictionary<Vector2Int, List<Vector2Int>> gravityLeftGraph;
    public Dictionary<Vector2Int, List<Vector2Int>> gravityUpGraph;
    public Dictionary<Vector2Int, List<Vector2Int>> gravityRightGraph;
    public List<GameObject> spawnedObjects;
    public Dictionary<string, List<GameObject>> switchFloors;
    public Dictionary<Vector2Int, Interactable> buttons;

    private Dictionary<Vector2Int, List<Vector2Int>> getNeighbours(Vector2Int pos, Dictionary<Vector2Int, List<Vector2Int>> graph, int[,] horizontalVals, int[,] verticalVals){
        // tricky things to remember:
        //      - the 0 index position is the vertical position
        //      - negative on 0 index is moving UP. L&R are same as intuition
        //      - given you are at pos Y,X in grid (with bigger Y being low), floor is (Y+1,X) and left wall is (Y,X)


        // assume default gravity
        List<Vector2Int> neighbours = new List<Vector2Int>();

        
        if (pos[0] < horizontalVals.GetLength(0)){
            // if tile is ground tile
            if(horizontalVals[pos[0]+1, pos[1]] == 1)
            {
                // left
                if (pos[1] > 0 && verticalVals[pos[0],pos[1]] == 0){
                    neighbours.Add(new Vector2Int(pos[0], pos[1]-1));
                }
                // right
                if((pos[1]<verticalVals.GetLength(1)-2 && verticalVals[pos[0],pos[1]+1] == 0)){
                    neighbours.Add(new Vector2Int(pos[0], pos[1]+1));
                }
                if(pos[0]>0) {
                    // up left - add jump
                    if(pos[1] > 0){
                        if((horizontalVals[pos[0], pos[1]] == 0 && verticalVals[pos[0]-1, pos[1]] == 0) ||
                        ((verticalVals[pos[0], pos[1]]) == 0 && horizontalVals[pos[0], pos[1]-1] == 0)){
                        neighbours.Add(new Vector2Int(pos[0]-1, pos[1]));
                    }
                    // up right - add jump
                    if(((pos[1] < verticalVals.GetLength(1)-2)) && ((horizontalVals[pos[0], pos[1]] == 0) && (verticalVals[pos[0]-1,pos[1]+1] == 0)) ||
                        ((verticalVals[pos[0], pos[1]+1]) == 0 && horizontalVals[pos[0], pos[1]+1] == 0)){
                        neighbours.Add(new Vector2Int(pos[0]-1, pos[1]-1));
                    }}
                    // up
                    if((horizontalVals[pos[0]-1,pos[1]] == 0) || ((pos[1] > 0 && verticalVals[pos[0]-1, pos[1]-1] == 0) || ((pos[1]<verticalVals.GetLength(1)-2) && verticalVals[pos[0]-1, pos[1]+1] == 0))){
                        neighbours.Add(new Vector2Int(pos[0]-1, pos[1]));
                    }
                }
            } else {
                if(pos[0]<horizontalVals.GetLength(0)-2){ 
                    // down 
                    neighbours.Add(new Vector2Int(pos[0]+1, pos[1]));
                }
            }
        }
        graph.Add(pos, neighbours);

        return graph;
        }
    
    public Vector3 toMapPos(int j, int i){
        float y = (origin[1]+mapSize[1]) - (j * (Res().y));
        float x = origin[0] + (i * (Res().x));
        return new Vector3(x, y, 0);
        }

    public Vector2Int fromMapPos(Vector2Int this_origin, Vector3 pos){
        // get map position as grid coordinates
        int y = (int)Math.Floor((pos.y-this_origin.y)/(double)Res().y);
        int x = (int)Math.Floor((pos.x-this_origin.y)/(double)Res().x);
        return new Vector2Int(y, x);
    }


    public Vector2Int Res(){
        int xRes = (int)mapSize[0] / horizontal.GetLength(1);
        int yRes = (int)mapSize[1] / horizontal.GetLength(0);
        return new Vector2Int(xRes, yRes);
    }

    public static int[,] Rotate90Counterclockwise(int[,] array){
        int rows = array.GetLength(0);
        int cols = array.GetLength(1);
        int[,] rotated = new int[cols, rows]; // Create a new array with swapped dimensions

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                rotated[cols - j - 1, i] = array[i, j];
            }
        }

        return rotated;
    }   
    Dictionary<Vector2Int, List<Vector2Int>> GenerateGraph(Dictionary<Vector2Int, List<Vector2Int>> graph, int[,] l_horizontal, int[,] l_vertical, int rotations){
        for(int rotate=0; rotate<rotations; rotate++){
            l_horizontal = Rotate90Counterclockwise(l_horizontal);
            l_vertical = Rotate90Counterclockwise(l_vertical);
        }
    
        if (rotations % 2 == 0) {
            for (int j=0; j<l_vertical.GetLength(0); j++){
                for (int i=0; i<l_horizontal.GetLength(1); i++){
                    graph = getNeighbours(new Vector2Int(j,i), graph, l_horizontal, l_vertical);
            }
        }
        } else {
            for (int j=0; j<l_horizontal.GetLength(0); j++){
                for (int i=0; i<l_vertical.GetLength(1); i++){
                    graph = getNeighbours(new Vector2Int(j,i), graph, l_vertical, l_horizontal);
            }
        }
        }
        return graph;
    }
    
    
    public void Awake()
    {
        gravityDownGraph = new Dictionary<Vector2Int, List<Vector2Int>>();
        gravityLeftGraph = new Dictionary<Vector2Int, List<Vector2Int>>();
        gravityUpGraph = new Dictionary<Vector2Int, List<Vector2Int>>();
        gravityRightGraph = new Dictionary<Vector2Int, List<Vector2Int>>();
        spawnedObjects = new List<GameObject>();
        switchFloors = new Dictionary<string, List<GameObject>>{
            {"H", new List<GameObject>()},
            {"V", new List<GameObject>()},
        };
        buttons = new Dictionary<Vector2Int, Interactable>();

        level = ParentLevelManager.currentLevel;
        horizontal = level.horizontal;
        vertical = level.vertical;
        origin = level.origin;
        endzone_obj.transform.position = toMapPos(level.endzone_pos.x, level.endzone_pos.y);


        if (rectanglePrefab != null)
        {
            // horizontal blocks - TODO: combine with vertical blocks section

            /// PLEASE make a function for instantiating a prefab from a matrix

            Dictionary<object, Interactable> hChanges = new Dictionary<object, Interactable>();
            Dictionary<object, Interactable> vChanges = new Dictionary<object, Interactable>();

            // add buttons
            if (level.change_floor_buttons.Count != 0){
                for (int i=0; i<level.change_floor_buttons["utility"].Count; i++){
                    Vector2Int loc = (Vector2Int)level.change_floor_buttons["location"][i];
                    Vector3 position = toMapPos(loc[0],loc[1]);
                    position.x = position.x + Res().x/2;
                    position.y = position.y - 2*Res().y/3;

                    // Instantiate the button prefab at the specified position and with no rotation
                    GameObject buttonInstance = Instantiate(buttonPrefab, position, Quaternion.identity);
                    
                    // Add button script
                    Interactable newScript = buttonInstance.AddComponent<Interactable>();
                    newScript.Initialize(loc,
                                        (int)level.change_floor_buttons["utility"][i],
                                        (Dictionary<Vector2Int, List<Vector2Int>>)level.change_floor_buttons["edgesAdded"][i],
                                        (Dictionary<Vector2Int, List<Vector2Int>>)level.change_floor_buttons["edgesRemoved"][i],
                                        (Color)level.change_floor_buttons["color"][i]
                                        );
                    spawnedObjects.Add(buttonInstance);

                    // Store floors that button changes
                    List<Vector2Int> hfloors = (List<Vector2Int>)level.change_floor_buttons["hfloors"][i];
                    List<Vector2Int> vfloors = (List<Vector2Int>)level.change_floor_buttons["vfloors"][i];
                    for (int h=0; h<hfloors.Count; h++){
                        hChanges.Add(hfloors[h], newScript);
                    }
                    for (int v=0; v<vfloors.Count; v++){
                        vChanges.Add(vfloors[v], newScript);
                    }

                    // store button with location as reference for pathfinding
                    buttons.Add(loc, newScript);
                }
            }

            // add horizontal blocks
            for (int j=0; j<horizontal.GetLength(0); j++){
                for (int i=0; i<horizontal.GetLength(1); i++){
                    if (horizontal[j, i] == 1){
                        
                        Vector2 position = toMapPos(j, i);
                        position.x = position.x + Res().x/2;

                        // Instantiate the rectangle prefab at the specified position and with no rotation
                        GameObject rectangleInstance = Instantiate(rectanglePrefab, position, Quaternion.identity);

                        // Set the scale of the instantiated rectangle
                        rectangleInstance.transform.localScale = new Vector3(Res().x, Res().y/4, 1);

                        spawnedObjects.Add(rectangleInstance);

                        Vector2Int vec = new Vector2Int(j,i);

                        // Set up changeable floors
                        if (hChanges.ContainsKey(vec)){
                            Interactable buttonObj = hChanges[vec];
                            buttonObj.hfloors.Add(rectangleInstance);
                            rectangleInstance.GetComponent<SpriteRenderer>().color = buttonObj.color;
                        }
                    }
                }
                }

            // add vertical blocks
            for (int j=0; j<vertical.GetLength(0); j++){
                for (int i=0; i<vertical.GetLength(1); i++){
                    if (vertical[j, i] == 1){
                        
                        Vector2 position = toMapPos(j, i);
                        position[1] -= Res().y/2;
                        GameObject rectangleInstance = Instantiate(rectanglePrefab, position, Quaternion.identity);

                        // Set the scale of the instantiated rectangle
                        rectangleInstance.transform.localScale = new Vector3(Res().x/4, Res().y, 1);

                        spawnedObjects.Add(rectangleInstance);

                        Vector2Int vec = new Vector2Int(j,i);

                        // store changes 
                        if (vChanges.ContainsKey(vec)){
                            vChanges[vec].vfloors.Add(rectangleInstance);
                            rectangleInstance.GetComponent<SpriteRenderer>().color = vChanges[vec].color;
                        }
                    }
                }
            }

            // add tokens
            void placeTokens(Dictionary<Vector2Int, bool> tokens, GameObject tokenPrefab){
                foreach (KeyValuePair<Vector2Int, bool> kvp in tokens){
                    Vector3 position = toMapPos(kvp.Key[0],kvp.Key[1]);
                    position.x = position.x + Res().x/2;
                    position.y = position.y -  Res().y/2;
                    // Instantiate the token prefab at the specified position and with no rotation
                    GameObject tokenInstance = Instantiate(tokenPrefab, position, Quaternion.identity);
                    spawnedObjects.Add(tokenInstance);
                }
            }

            // add arrows
            foreach ( KeyValuePair<Vector2Int, int> kvp in level.gravity_arrows ) {

                Vector2 position = toMapPos(kvp.Key[0], kvp.Key[1]);
                position += new Vector2(Res().x/2, Res().y/2);

                // rotate according to direction 
                GameObject arrowInstance = Instantiate(arrowPrefab, position, Quaternion.Euler(0, 0, 90*kvp.Value));

                spawnedObjects.Add(arrowInstance);

            }

            placeTokens(level.goblin_tokens, goblinTokenPrefab);
            placeTokens(level.goat_tokens, goatTokenPrefab);

            // make graphs for pathfinding - rotate arrays to simulate changing gravity
            gravityDownGraph = GenerateGraph(gravityDownGraph, horizontal, vertical, 0);
            gravityLeftGraph = GenerateGraph(gravityLeftGraph, horizontal, vertical, 1);
            gravityUpGraph = GenerateGraph(gravityUpGraph, horizontal, vertical, 2);
            gravityRightGraph = GenerateGraph(gravityRightGraph, horizontal, vertical, 3);
        }
        else
        {
            Debug.LogError("Rectangle prefab not assigned in the Inspector.");
        }

    }
    private void OnLevelEnd()
    {
        foreach (var obj in spawnedObjects){
            Destroy(obj);
        }
        spawnedObjects.Clear();
    }
}