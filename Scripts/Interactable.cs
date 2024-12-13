using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Dictionary<Vector2Int, List<Vector2Int>> edgesAdded;
    public Dictionary<Vector2Int, List<Vector2Int>> edgesRemoved;
    public Vector2Int location;
    public int utility; 
    public Color color;
    public List<GameObject> hfloors = new List<GameObject>();
    public List<GameObject> vfloors = new List<GameObject>();

    public void Initialize(Vector2Int location, int utility, 
                       Dictionary<Vector2Int, List<Vector2Int>> edgesAdded,
                       Dictionary<Vector2Int, List<Vector2Int>> edgesRemoved, 
                       Color color)
    {
    this.location = location;
    this.utility = utility;
    this.edgesAdded = edgesAdded;
    this.edgesRemoved = edgesRemoved;
    this.color = color;
    }

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