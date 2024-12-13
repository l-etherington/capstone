using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // public List<string> sceneNames = new List<string>(){"Scene1", "Scene2"};

    public static List<Level> sceneList = new List<Level>(){
        new Level(
            horizontal: new int[,] {
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {1,1,1,1,0,0,0,1,1,1,0,1,1,1,0},
                {0,0,1,1,1,0,1,1,0,0,0,0,0,0,1},
                {1,1,1,0,0,1,0,0,0,0,0,0,0,1,0},
                {0,0,0,1,0,0,1,1,0,0,1,1,0,0,1},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
                },
            vertical: new int[,] {
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                {1,0,0,0,0,0,0,0,0,0,1,0,0,0,0,1},
                {1,0,0,0,0,1,0,0,0,0,1,0,0,1,0,1},
                {1,0,0,0,0,0,0,0,0,0,1,0,0,0,0,1},
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1}
                },
            goblinTokens: new Dictionary<Vector2Int, bool> {{new Vector2Int(4,5), true},
                        {new Vector2Int(2,3), true},
                        {new Vector2Int(4,7), true},
                        {new Vector2Int(1,8), true},
                        {new Vector2Int(0,7), true}},
            goatTokens: new Dictionary<Vector2Int, bool> {{new Vector2Int(1,1), true},
                        {new Vector2Int(3,5), true},
                        {new Vector2Int(2,4), true},
                        {new Vector2Int(3,9), true},
                        {new Vector2Int(2,8), true}},
            changeFloorButtons: new Dictionary<string, List<object>>()
                {
                    // A single location as Vector2Int
                    { "location", new List<object> {new Vector2Int(2, 3)} },

                    // A single color as a string
                    { "color", new List<object> {Color.red} },

                    // Vertical floors as a list of Vector2Int
                    { "vfloors", new List<object> {new List<Vector2Int> { new Vector2Int(3, 5), new Vector2Int(3, 13) } } },

                    // Horizontal floors as a list of Vector2Int
                    { "hfloors", new List<object> {new List<Vector2Int> { new Vector2Int(2, 6) } } },

                    // Utility value as an integer
                    { "utility", new List<object> {30} },

                    // Edges added as a dictionary
                    { "edgesAdded", new List<object>{
                        new Dictionary<Vector2Int, List<Vector2Int>>{ 
                            { new Vector2Int(3, 4), new List<Vector2Int> { new Vector2Int(4, 4) } } 
                        } 
                    }},

                    // Edges removed as a dictionary
                    { "edgesRemoved", new List<object>{
                        new Dictionary<Vector2Int, List<Vector2Int>>{ 
                            { new Vector2Int(3, 4), new List<Vector2Int> { new Vector2Int(3, 3) } } 
                        } 
                    }}
                },
            gravityArrows: new Dictionary<Vector2Int, int>(),
            endzonePos: new Vector2Int(3,7),
            maxMoves: 10,
            PCstart: new Vector2Int(4,0),
            NPCstart: new Vector2Int(3,2),
            origin: new Vector2Int(0,0)
        ),

        new Level(
            horizontal: new int[,] {
                {1,1,1,1,1,1,1,1},
                {0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0},
                {1,1,1,1,1,1,1,1},
                {0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0},
                {1,1,1,1,1,1,1,1}
                },
            vertical: new int[,] {
                {1,0,0,0,0,0,0,0,1},
                {1,0,0,0,0,0,0,0,1},
                {1,0,0,0,0,0,0,0,1},
                {1,0,0,0,0,0,0,0,1},
                {1,0,0,0,0,0,0,0,1},
                {1,0,0,0,0,0,0,0,1},
                {1,0,0,0,0,0,0,0,1},
                {1,0,0,0,0,0,0,0,1},
                {1,0,0,0,0,0,0,0,1},
                {1,0,0,0,0,0,0,0,1},
                },
            goblinTokens: new Dictionary<Vector2Int, bool> {{new Vector2Int(0,7), true},
                                            {new Vector2Int(1,7), true},
                                            {new Vector2Int(2,7), true},
                                            {new Vector2Int(3,7), true},
                                            {new Vector2Int(4,7), true}},
            goatTokens: new Dictionary<Vector2Int, bool> {{new Vector2Int(5,0), true},
                                            {new Vector2Int(6,0), true},
                                            {new Vector2Int(7,0), true},
                                            {new Vector2Int(8,0), true},
                                            {new Vector2Int(9,0), true},
                                            {new Vector2Int(6,7), true},
                                            {new Vector2Int(7,7), true}},
            changeFloorButtons: new Dictionary<string, List<object>>(){}, // no buttons
            gravityArrows: new Dictionary<Vector2Int, int>(){
                {new Vector2Int(9,3), 1},
                {new Vector2Int(9,5), 3},
                {new Vector2Int(6,1), 0},
                {new Vector2Int(6,7), 0}
            },
            endzonePos: new Vector2Int(6,4),
            maxMoves: 10,
            PCstart: new Vector2Int(9,4),
            NPCstart: new Vector2Int(4,4),
            origin: new Vector2Int(500,0)
        )
    };


    // public static LevelManager Instance;
    private int sceneIndex = 0;
    public Level currentLevel = sceneList[0];

    // store all other game objects to reset scenes
    public GameObject PC;
    public GameObject NPC;
    public MakeMap map;
    public GameObject camera;
    public GameObject PC_col;
    public GameObject endzone;

    public GameObject winText;

    public void ChangeLevel(){
        Debug.Log("Leaving Scene"+ sceneIndex);
        sceneIndex += 1;
        if (sceneIndex < sceneList.Count){
            currentLevel = sceneList[sceneIndex];
            map.Awake();
            PC.transform.position = map.toMapPos(currentLevel.PCstart[0], currentLevel.PCstart[1]);
            PC_col.GetComponent<PlayerCollider>().Awake();
            camera.transform.position += new Vector3(currentLevel.origin.x, currentLevel.origin.y, 0);
            NPC.GetComponent<Pathfinder>().Start();
            endzone.GetComponent<Endzone>().Start();
        } else {
            winText.SetActive(true);
        }
        
    }

    private void Awake()
    {
        currentLevel = sceneList[sceneIndex];
    }
}