using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path 
{
    public List<Vector2Int> nodes;
    public double utility;
    public Dictionary<Vector2Int, List<Vector2Int>> graph;
    public Dictionary<Vector2Int, bool> seen = new Dictionary<Vector2Int, bool>();

    public Path(List<Vector2Int> nodes, double utility, Dictionary<Vector2Int, List<Vector2Int>> graph)
        {
        this.nodes = new List<Vector2Int>(nodes);
        this.utility = utility;
        this.graph = new Dictionary<Vector2Int, List<Vector2Int>>(graph);
        }

    void Awake(){

    }

    void Update(){

    }
}