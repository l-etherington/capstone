using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic; 
using System;

public class controller : MonoBehaviour
{
    public float speed;
    public float jump_height;
    public GameObject winText;
    private Transform camera;
    // public GameObject groundCheck;
    public bool playerOnGround = true;
    public Vector2 gravityDirection; // new Vector2(0, -9.81f) could be the default
    private Rigidbody2D _rb;
    public GameObject trigger_collider;
    // public GameObject top;
    // public GameObject right;
    // public GameObject left;
    // public GameObject bottom;


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
        gravityDirection = new Vector2(0, -9.81f);
        winText.SetActive(false);
        _rb = GetComponent<Rigidbody2D>();
    }

    // use FixedUpdate for physics stuff
    void FixedUpdate(){  
        Physics2D.gravity = gravityDirection;

        // transform.Translate(Vector2.right * Input.GetAxis("Horizontal") * speed * Time.deltaTime);
    //     if(playerOnGround && Input.GetKeyDown(KeyCode.Space)){
    //         Debug.Log("jumping");
    //         GetComponent<Rigidbody2D>().AddForce(transform.up * 20000 * Time.deltaTime, ForceMode2D.Impulse);
    //         playerOnGround = false;
    //         //transform.Translate(Vector2.up * Input.GetAxis("Jump") * jump_height * Time.deltaTime);
    //         //40000
    //    }
    }
    // Update is called once per frame
    void Update()
    {   
        // playerOnGround = trigger_collider.GetComponent<GroundCheckScript>().onGround;

        string key = "";
        if(Input.GetKey(KeyCode.LeftArrow)){
            // Debug.Log("left");
            key = "Left";
        }
        if(Input.GetKey(KeyCode.UpArrow)){
            // Debug.Log("up");
            // Debug.Log(transform.rotation.eulerAngles);
            key = "Up";
        }
        if(Input.GetKey(KeyCode.RightArrow)){
            // Debug.Log("right");
            key = "Right";
        }
        if(Input.GetKey(KeyCode.DownArrow)){
            // Debug.Log("left");
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
            // Debug.Log("---------------------angle:");
            Debug.Log(transform.rotation.eulerAngles[2]);
            Debug.Log(angle);
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
        if (collided.CompareTag("UpArrow"))
        {
            gravityDirection = new Vector2(0, 100);
            _rb.gravityScale = 11;
        }
        if (collided.CompareTag("DownArrow"))
        {
            gravityDirection = new Vector2(0, -100);
            _rb.gravityScale = 11;
        }
        if (collided.CompareTag("RightArrow"))
        {
            gravityDirection = new Vector2(100, 0);
            _rb.gravityScale = 5;
        }
        if (collided.CompareTag("LeftArrow"))
        {
            gravityDirection = new Vector2(-100, 0);
            _rb.gravityScale = 11;
        }
    }

    void OnTriggerExit2D(Collider2D collided)
    {

    }
}
