using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MakeMap : MonoBehaviour
{
    // Drag your prefab here in the Unity Inspector
    public GameObject rectanglePrefab;
    public GameObject goblinTokenPrefab;
    public GameObject goatTokenPrefab;
    public GameObject buttonPrefab;
    public Vector2 origin;
    public Vector2 mapSize;
    public Dictionary<Vector2Int, List<Vector2Int>> gravityDownGraph = new Dictionary<Vector2Int, List<Vector2Int>>();
    public List<GameObject> spawnedObjects = new List<GameObject>();
    public Dictionary<string, List<GameObject>> switchFloors = new Dictionary<string, List<GameObject>>{
        {"H", new List<GameObject>()},
        {"V", new List<GameObject>()},
    };
    public static int[,] horizontal = Level1.horizontal;
    public static int[,] vertical = Level1.vertical;

    private void getNeighbours(Vector2Int pos, Dictionary<Vector2Int, List<Vector2Int>> graph){
        // tricky things to remember:
        //      - the 0 index position is the vertical position
        //      - negative on 0 index is moving UP. L&R are same as intuition
        //      - given you are at pos Y,X in grid (with bigger Y being low), floor is (Y+1,X) and left wall is (Y,X)


        // assume default gravity
        List<Vector2Int> neighbours = new List<Vector2Int>();

        // if tile is ground tile
        if ((pos[0] < horizontal.GetLength(0)) && horizontal[pos[0]+1, pos[1]] == 1)
        {
            // left
            if (pos[1] > 0 && vertical[pos[0],pos[1]] == 0){
                neighbours.Add(new Vector2Int(pos[0], pos[1]-1));
            }
            // right
            if((pos[1]<vertical.GetLength(1)-2 && vertical[pos[0],pos[1]+1] == 0)){
                neighbours.Add(new Vector2Int(pos[0], pos[1]+1));
            }
            if(pos[0]>0) {
                // up left - add jump
                if(((pos[1] > 0) && ((horizontal[pos[0], pos[1]] == 0) && (vertical[pos[0]-1, pos[1]] == 0)) ||
                    ((vertical[pos[0], pos[1]]) == 0 && horizontal[pos[0], pos[1]-1] == 0))){
                    neighbours.Add(new Vector2Int(pos[0]-1, pos[1]));
                }
                // up right - add jump
                if(((pos[1] < vertical.GetLength(1)-2)) && ((horizontal[pos[0], pos[1]] == 0) && (vertical[pos[0]-1,pos[1]+1] == 0)) ||
                    ((vertical[pos[0], pos[1]+1]) == 0 && horizontal[pos[0], pos[1]+1] == 0)){
                    neighbours.Add(new Vector2Int(pos[0]-1, pos[1]-1));
                }
                // up
                if((horizontal[pos[0]-1,pos[1]] == 0) || ((pos[1] > 0 && vertical[pos[0]-1, pos[1]-1] == 0) || ((pos[1]<vertical.GetLength(1)-2) && vertical[pos[0]-1, pos[1]+1] == 0))){
                    neighbours.Add(new Vector2Int(pos[0]-1, pos[1]));
                }
            }
        }
        else if(pos[0]<horizontal.GetLength(0)-2){ 
            // down left
            if(pos[1]>0 && vertical[pos[0]+1, pos[1]] == 0){
                neighbours.Add(new Vector2Int(pos[0]+1, pos[1]-1));
            }

            // down right
            if(pos[1]<vertical.GetLength(1)-2 && vertical[pos[0]+1, pos[1]+1] == 0){
                neighbours.Add(new Vector2Int(pos[0]+1, pos[1]+1));
            }

            // down 
            neighbours.Add(new Vector2Int(pos[0]+1, pos[1]));
        }
        graph.Add(pos, neighbours);
        }
    
    public Vector3 toMapPos(int j, int i, int xRes, int yRes){
        float y = (origin[1]+mapSize[1]) - (j * (yRes));
        float x = origin[0] + (i * (xRes) - 1);
        return new Vector3(x, y, 0);
        }


    void Start()
    {
        // Spawn a rectangle at position (0, 0) with no rotation
        // find center of position given matrix pos with toMapPos
        // offset if vertical
        // determine scale based on vertical or horizontal
        if (rectanglePrefab != null)
        {

            int xRes = (int)mapSize[0] / horizontal.GetLength(1);
            int yRes = (int)mapSize[1] / horizontal.GetLength(0);
            // horizontal blocks - TODO: combine with vertical blocks section
            Debug.Log(new Vector2(xRes, yRes));

            /// PLEASE make a function for instantiating a prefab from a matrix

            // add horizontal blocks
            for (int j=0; j<horizontal.GetLength(0); j++){
                for (int i=0; i<horizontal.GetLength(1); i++){
                    if (horizontal[j, i] == 1){
                        
                        Vector2 position = toMapPos(j, i, xRes, yRes);
                        position.x = position.x + xRes/2;

                        // Instantiate the rectangle prefab at the specified position and with no rotation
                        GameObject rectangleInstance = Instantiate(rectanglePrefab, position, Quaternion.identity);
                        // Set the scale of the instantiated rectangle
                        rectangleInstance.transform.localScale = new Vector3(xRes, yRes/4, 1);

                        spawnedObjects.Add(rectangleInstance);
                    }
                    else if (horizontal[j, i] == 2){
                        
                    Vector2 position = toMapPos(j, i, xRes, yRes);
                    position.x = position.x + xRes/2;

                    // Instantiate the rectangle prefab at the specified position and with no rotation
                    GameObject rectangleInstance = Instantiate(rectanglePrefab, position, Quaternion.identity);
                    // Set the scale of the instantiated rectangle
                    rectangleInstance.transform.localScale = new Vector3(xRes, yRes/4, 1);
                    rectangleInstance.transform.GetComponent<SpriteRenderer>().material.color = Color.red;
                    spawnedObjects.Add(rectangleInstance);
                    switchFloors["H"].Add(rectangleInstance);
                }
                }
            }

            // add vertical blocks
            for (int j=0; j<vertical.GetLength(0); j++){
                for (int i=0; i<vertical.GetLength(1); i++){
                    if (vertical[j, i] == 1){
                        
                        Vector2 position = toMapPos(j, i, xRes, yRes);
                        GameObject rectangleInstance = Instantiate(rectanglePrefab, position, Quaternion.Euler(0, 0, 0));
                        // Set the scale of the instantiated rectangle
                        rectangleInstance.transform.localScale = new Vector3(xRes/4, yRes, 1);

                        spawnedObjects.Add(rectangleInstance);
                    }
                    else if (vertical[j, i] == 2){
                        
                    Vector2 position = toMapPos(j, i, xRes, yRes);
                    GameObject rectangleInstance = Instantiate(rectanglePrefab, position, Quaternion.Euler(0, 0, 0));
                    // Set the scale of the instantiated rectangle
                    rectangleInstance.transform.localScale = new Vector3(xRes/4, yRes, 1);
                    rectangleInstance.transform.GetComponent<SpriteRenderer>().material.color = Color.red;
                    spawnedObjects.Add(rectangleInstance);
                    switchFloors["V"].Add(rectangleInstance);
                }
                }
            }

            // add button
            for (int i=0; i<Level1.change_floor_buttons.Length/2; i++){
                Vector3 position = toMapPos(Level1.change_floor_buttons[i,0],Level1.change_floor_buttons[i,1],xRes,yRes);
                position.x = position.x + xRes/2;
                position.y = position.y - yRes/2;
                // Instantiate the token prefab at the specified position and with no rotation
                GameObject tokenInstance = Instantiate(buttonPrefab, position, Quaternion.identity);
                spawnedObjects.Add(tokenInstance);
            }
            // add tokens
            void placeTokens(int[,] tokenList, GameObject tokenPrefab){
                for (int i=0; i<tokenList.Length/2; i++){
                    Vector3 position = toMapPos(tokenList[i,0],tokenList[i,1],xRes,yRes);
                    position.x = position.x + xRes/2;
                    position.y = position.y - yRes/2;
                    // Instantiate the token prefab at the specified position and with no rotation
                    GameObject tokenInstance = Instantiate(tokenPrefab, position, Quaternion.identity);
                    spawnedObjects.Add(tokenInstance);
                }
            }

            placeTokens(Level1.goblin_tokens, goblinTokenPrefab);
            placeTokens(Level1.goat_tokens, goatTokenPrefab);

            // make graph for pathfinding
            for (int j=0; j<vertical.GetLength(0); j++){
                for (int i=0; i<horizontal.GetLength(1); i++){
                    getNeighbours(new Vector2Int(j,i), gravityDownGraph);
                }
            }
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
