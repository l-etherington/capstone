using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic; 
using System;

public class controller : MonoBehaviour
{
    public float speed;
    public float jump_height;
    private Transform camera;
    public bool playerOnGround = true;
    public Vector2 gravityDirection; // new Vector2(0, -9.81f) could be the default
    private Rigidbody2D _rb;
    public GameObject trigger_collider;
    public GameObject enemy;
    public MakeMap map;
    public LevelManager ParentLevelManager; 
    public Level level;

    // maps orientation (relative to map) + inputted key to direction (relative to person)
    new public Dictionary<int, Dictionary<string, Vector2>> movement = 
        new Dictionary<int, Dictionary<string, Vector2>>{
            {0,
            new Dictionary<string, Vector2>{
                {"Right", new Vector2(1,0)},
                {"Left", new Vector2(-1,0)},
                {"Up", new Vector2(0,1)},
                {"Down", new Vector2(0,-1)},
            }},
            {90,
            new Dictionary<string, Vector2>{
                {"Right", new Vector2(0,1)},
                {"Left", new Vector2(0,-1)},
                {"Up", new Vector2(-1,0)},
                {"Down", new Vector2(1,0)},
            }},
            {180,
            new Dictionary<string, Vector2>{
                {"Right", new Vector2(-1,0)},
                {"Left", new Vector2(1,0)},
                {"Up", new Vector2(0,-1)},
                {"Down", new Vector2(0,1)},
            }},
            {270,
            new Dictionary<string, Vector2>{
                {"Right", new Vector2(0,-1)},
                {"Left", new Vector2(0,1)},
                {"Up", new Vector2(1,0)},
                {"Down", new Vector2(-1,0)},
            }}
        };

    // Start is called before the first frame update
    void Start()
    {
        gravityDirection = new Vector2(0, -1);
        _rb = this.GetComponent<Rigidbody2D>();
        level = ParentLevelManager.currentLevel;
    }

    // use FixedUpdate for physics stuff
    void FixedUpdate(){  
        Physics2D.gravity = gravityDirection * _rb.gravityScale;
    }
    // Update is called once per frame
    void Update()
    {   
        // playerOnGround = trigger_collider.GetComponent<GroundCheckScript>().onGround;

        string key = "";
        if(Input.GetKey(KeyCode.LeftArrow)){
            key = "Left";
        }
        if(Input.GetKey(KeyCode.UpArrow)){
            key = "Up";
        }
        if(Input.GetKey(KeyCode.RightArrow)){
            key = "Right";
        }
        if(Input.GetKey(KeyCode.DownArrow)){
            key = "Down";
        }

        // get orientation
        int[] angleList = {0, 90, 180, 270};
        double[] bestAngle = {0, 400};
        for (int i=0; i<angleList.Length; i++){
            double diff = Math.Abs(transform.rotation.eulerAngles[2] - (double)angleList[i]);
            if (diff < bestAngle[1]){
                bestAngle[0] = (double)i;
                bestAngle[1] = diff;
            }
        }
        int angle = angleList[(int)bestAngle[0]];

        // determine direction of movement based on key & orientation
        Vector3 dn = transform.TransformDirection(Vector3.down);
        if(key != ""){
            Vector2 movement_vec = movement[angle][key];

            // check if intended direction is facing opposite direction as gravity: non-jump movement
            if((movement_vec[0]==0) != (Physics2D.gravity[0]==0)){
                // GetComponent<Rigidbody2D>().MovePosition(new Vector2(transform.position.x, transform.position.y) + movement_vec * speed * Time.deltaTime);
                GetComponent<Rigidbody2D>().AddForce(movement_vec * speed);
                
            } else {
                List<Collider2D> collidingWith = new List<Collider2D>();
                int numColliding = GetComponent<Rigidbody2D>().Overlap(transform.position, 0, collidingWith);
                for(int i=0; i<numColliding; i++){
                    if(collidingWith[i].CompareTag("ground")){
                        // jump movement
                        GetComponent<Rigidbody2D>().AddForce(movement_vec * jump_height, ForceMode2D.Impulse); 
                    }
                }
            };
        }
       }

    void OnTriggerEnter2D(Collider2D collided)
    {
        if(collided.CompareTag("DirectionArrow")){
            Vector3 arrowDirection = collided.gameObject.transform.rotation.eulerAngles;

            // up
            if (arrowDirection == new Vector3(0, 0, 0))
            {
                gravityDirection = new Vector2(0, 1);
                enemy.GetComponent<Pathfinder>().recalculate(false, level, map.gravityUpGraph);
            }
            // down
            if (arrowDirection == new Vector3(0, 0, 180))
            {
                gravityDirection = new Vector2(0, -1);
                enemy.GetComponent<Pathfinder>().recalculate(false, level, map.gravityDownGraph);
            }
            // right
            if (arrowDirection == new Vector3(0, 0, 270))
            {
                gravityDirection = new Vector2(1, 0);
                enemy.GetComponent<Pathfinder>().recalculate(false, level, map.gravityRightGraph);
            }
            // left
            if (arrowDirection == new Vector3(0, 0, 90))
            {
                gravityDirection = new Vector2(-1, 0);
                enemy.GetComponent<Pathfinder>().recalculate(false, level, map.gravityLeftGraph);
            }
        }

        if(collided.CompareTag("EndZone")){
            enemy.GetComponent<Pathfinder>().recalculate(true, level);
        }
    }

    void OnTriggerExit2D(Collider2D collided)
    {

    }
}
