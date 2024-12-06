using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Dictionary<Vector2Int, List<Vector2Int>> edgesAdded;
    public Dictionary<Vector2Int, List<Vector2Int>> edgesRemoved;
    public Vector2Int location;
    public int utility; 
    public str color;
    public Vector2Int[,] hfloors;
    public Vector2Int[,] vfloors;

    void Awake(){
        // public static create(Vector2Int loc, int util, Dictionary<Vector2Int, List<Vector2Int>> add, Dictionary<Vector2Int, List<Vector2Int>> rem){
        //     location = loc;
        //     utility = util;
        //     edgesRemoved = add;
        //     edgesAdded = rem;
        //     return this;
        // }
    }

    void Update(){

    }
}