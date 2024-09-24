using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic; 

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
    public GameObject top;
    public GameObject right;
    public GameObject left;
    public GameObject bottom;

    // new public Vector2[,] movement = {
    //     {new Vector2(1,0), new Vector2(-1,0), new Vector2(0,1)},
    //     {new Vector2(0,-1), new Vector2(0,1), new Vector2(-1,0)},
    //     {new Vector2(0,1), new Vector2(0,-1), new Vector2(1,0)},
    //     {new Vector2(0,-1), new Vector2(0,1), new Vector2(-1,0)}
    // };

    // new public Dictionary<string, Vector2>[] movement = {
    //     new Dictionary<string, Vector2>{
    //         {"Right", new Vector2(1,0)},
    //         {"Left", new Vector2(1,0)},
    //         {"Up", new Vector2(0,1)},
    //     },
    //     new Dictionary<string, Vector2>{
    //         {"Right", new Vector2(0,-1)},
    //         {"Left", new Vector2(0,1)},
    //         {"Up", new Vector2(-1,0)},
    //     }
    // };

    // maps orientation (relative to map) + inputted key to direction (relative to person)
    new public Dictionary<int, Dictionary<string, Vector2>> movement = 
        new Dictionary<int, Dictionary<string, Vector2>>{
            {45,
            new Dictionary<string, Vector2>{
                {"Right", new Vector2(1,0)},
                {"Left", new Vector2(-1,0)},
                {"Up", new Vector2(0,1)},
            }},
            {135,
            new Dictionary<string, Vector2>{
                {"Right", new Vector2(0,1)},
                {"Left", new Vector2(0,-1)},
                {"Up", new Vector2(1,0)},
            }},
            {225,
            new Dictionary<string, Vector2>{
                {"Right", new Vector2(-1,0)},
                {"Left", new Vector2(1,0)},
                {"Up", new Vector2(0,-1)},
            }},
            {305,
            new Dictionary<string, Vector2>{
                {"Right", new Vector2(0,-1)},
                {"Left", new Vector2(0,1)},
                {"Up", new Vector2(-1,0)},
            }}
        };

    // Start is called before the first frame update
    void Start()
    {
        gravityDirection = new Vector2(0, -9.81f);
        winText.SetActive(false);
        _rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate(){  // use FixedUpdate for physics stuff
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
        playerOnGround = trigger_collider.GetComponent<GroundCheckScript>().onGround;

        string key = "";
        if(Input.GetKey(KeyCode.LeftArrow)){
            Debug.Log("left");
            key = "Left";
        }
        if(Input.GetKey(KeyCode.UpArrow)){
            Debug.Log("up");
            Debug.Log(transform.rotation.eulerAngles);
            key = "Up";
        }
        if(Input.GetKey(KeyCode.RightArrow)){
            Debug.Log("right");
            key = "Right";
        }

        // get orientation
        int angle = 45;
        while (true)
        {
            if(angle > 305){
                angle = 45;
                break;
            }
            if(transform.rotation.eulerAngles[2] < angle){
                break;
            }else{
                angle = angle + 90;
            };
        }

        // determine direction of movement based on key & orientation
        Vector3 dn = transform.TransformDirection(Vector3.down);
        if(key != ""){
            Debug.Log("---------------------angle:");
            Debug.Log(angle);
            Vector2 movement_vec = movement[angle][key];

            // check if intended direction is facing opposite direction as gravity
            if((movement_vec[0]==0) != (Physics2D.gravity[0]==0)){
                // non-jump movement
                Debug.Log("move horizontal, set velocity to:");
                // Debug.Log(movement_vec * speed * Time.deltaTime);
                // Debug.Log(movement_vec);
                Debug.Log(speed);
                // Debug.Log(Time.deltaTime);
                // _rb.velocity = movement_vec * speed;
                GetComponent<Rigidbody2D>().MovePosition(new Vector2(transform.position.x, transform.position.y) + movement_vec * speed * Time.deltaTime);
                
            } else {
                // jump movement
                if (playerOnGround){
                    Debug.Log("JUMP!");
                    GetComponent<Rigidbody2D>().AddForce(movement_vec * 10000 * Time.deltaTime, ForceMode2D.Impulse); 
                }
            };
        }
        // _rb.velocity = Input.GetAxis("Horizontal") * movement_vec * speed; 

        //transform.rotation = Quaternion.identity;
        // if(playerOnGround && Input.GetButton("Jump")){
        //     Debug.Log("-----------------------------------------rotation");
        //     Debug.Log(transform.rotation.eulerAngles);
        //     Debug.Log("-----------------------------------------gravity");
        //     Debug.Log(Physics2D.gravity);
        //     Debug.Log("-----------------------------------------velocity");
        //     Debug.Log(_rb.velocity);
            // GetComponent<Rigidbody2D>().AddForce(transform.up * 10000 * Time.deltaTime, ForceMode2D.Impulse);
            //transform.Translate(Vector2.up * Input.GetAxis("Jump") * jump_height * Time.deltaTime);
            //40000
       }
        //transform.Translate(Vector2.up * Input.GetAxis("Vertical") * jump_height * Time.deltaTime);

    void OnTriggerEnter2D(Collider2D collided)
    {
        if (collided.CompareTag("ground"))
        {
            Debug.Log("Entered ground");
            // playerOnGround = true;
        }
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
            _rb.gravityScale = 11;
        }
        if (collided.CompareTag("LeftArrow"))
        {
            gravityDirection = new Vector2(-100, 0);
            _rb.gravityScale = 11;
        }
    }

    void OnTriggerExit2D(Collider2D collided)
    {
        //Destroy the coin if Object tagged Player comes in contact with it
        if (collided.CompareTag("ground"))
        {
            Debug.Log("exited ground");
            // playerOnGround = false;
        }
    }
}
