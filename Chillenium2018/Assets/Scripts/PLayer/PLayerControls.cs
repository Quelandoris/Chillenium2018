using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLayerControls : MonoBehaviour {

    //PLayerControl (mistyped initially but now too far in to fix it) handles player controls, movement, and actions

    //Public Variables 

    //Controls Variables
    public float speed = 3; //How fast player accelerates immediately as you hit an arrow key
    public float topSpeed = 5; //Top speed a player can reach, prevents character going supersonic
    public float slowAmount = 5; //How quickly thr player stops after releasing a movement key
    public float GrabDistance = 5; //How far away the player can grab objects with the Liftable.cs component

    //Private variables
    Transform myTransform;
    Rigidbody2D myRB;
    Liftable carrying; //The object the player is carrying
    GridScript TheGrid; //Welcome to the Grid
    bool movelock = false; //When the player is building, we dont want them to be able to move. If this is true, they can't move

    // Use this for initialization
    void Start () {
        //Assign things by finding them on the player object or in the scene
        myTransform = GetComponent<Transform>();
        myRB = GetComponent<Rigidbody2D>();
        TheGrid = FindObjectOfType<GridScript>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!movelock) PlayerMovement(); //If player isn't building, run the Movement function
        Pickup(); //Check for input, then pick up the object
        CheckGrabbable(); //Check for things you can gab near by
        Build(); //Check for input, then start building a buildable object
	}

    void CheckGrabbable()
    {
        //If you're carrying something, don't bother with this check.
        if (!carrying)
        {
            //Check in a circle around you for any Colliders
            Collider2D[] circleHit = Physics2D.OverlapCircleAll(transform.position, GrabDistance);
            //Check each Collider to see if it has a Liftable.cs Component on it. If there is, make its UI appear. Break afterwards so that only one UI appears at a time
            foreach (Collider2D coll in circleHit)
            {
                if (coll.GetComponent<Liftable>())
                {
                    coll.GetComponent<Liftable>().near = true;
                    break;
                }
            }
        }
    }

    //Build with Z
    void Build()
    {
        //If Z is pressed...
        if (Input.GetKey(KeyCode.Z))
        {
            movelock = true; //Player can't move while building
            bool anyBuildable=false; //Prepare to search for buildable objects
            Liftable buildable = null; //Prepare a holder for the object we're building
            Collider2D[] circleHit = Physics2D.OverlapCircleAll(transform.position, GrabDistance); //Do a circle check, same as above
            foreach (Collider2D coll in circleHit)
            {
                if (coll.GetComponent<Liftable>())
                {
                    //Assign the thing we're going to build, then break so we only build one thing at a time.
                    anyBuildable = true;
                    buildable = coll.GetComponent<Liftable>();
                    break;
                }
            }
            //If we have something to build, build it. Refer to Liftable.cs for details
            if (anyBuildable)
            {
                movelock = false;
                buildable.Build();
            }
        }
        //If Z is released prematurely, reset build progress
        if (Input.GetKeyUp(KeyCode.Z))
        {
            //Check for things nearby
            Collider2D[] circleHit = Physics2D.OverlapCircleAll(transform.position, GrabDistance);
            foreach (Collider2D coll in circleHit)
            {
                if (coll.GetComponent<Liftable>())
                {
                    //If there's an aplicable object, reset its build progress
                    coll.GetComponent<Liftable>().CancelBuild();
                }
            }
        }
    }

    //X to pickup object when nearby
    void Pickup()
    {

        if (Input.GetKeyDown(KeyCode.X))
        {
            
            //Object that isnt carried, pick up first object on the stack
            if (carrying == null)
            {
                Collider2D[] circleHit = Physics2D.OverlapCircleAll(transform.position, GrabDistance);
                Liftable LiftableHit=null;
                //Find the first Liftable in the area and assign it
                foreach (Collider2D coll in circleHit)
                {
                    
                    if (coll.GetComponent<Liftable>() != null)
                    {
                        
                        LiftableHit = coll.GetComponent<Liftable>();
                    }
                }
                //If something was found,  Start carrying it, forcing it to follow the player's position
                if (LiftableHit != null)
                {
                   
                    if (LiftableHit.Carrier == null)
                    {
                       
                        LiftableHit.Carrier = gameObject;
                        carrying = LiftableHit;
                        carrying.carried = true;
                    }
                }
            }
            //Drop object when button is pressed but already holding something
            else
            {
                carrying.Carrier = null; //remove Carrier assignment
                carrying.carried = false; //Set caried false
                carrying.transform.position=TheGrid.NearestPointOnGrid(carrying.transform.position); //Snap the dropped bricks to the grid
                carrying = null;
            }
        }
    }

    //Get input to set where the players moves and how fast
    void PlayerMovement()
    {
        //Check if anything at all is being pressed for movement, otherwise start applying slowMovement
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
        {
            //Move up: UpArrow
            if (Input.GetKey(KeyCode.UpArrow))
            {
                //AddRelativeForce lets you treat the force as though the player is like a rock in a slingshot, rather than a world position
                myRB.AddRelativeForce(Vector2.up * speed);
            }
            //Move down: DownArrow
            if (Input.GetKey(KeyCode.DownArrow))
            {
                myRB.AddRelativeForce(Vector2.down * speed);
            }
            //Move left: LeftArrow
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                myRB.AddRelativeForce(Vector2.left * speed);
            }
            //Move Right: RightArrow
            if (Input.GetKey(KeyCode.RightArrow))
            {
                myRB.AddRelativeForce(Vector2.right * speed);
            }

            //Velocity.magnitude tracks the player's velocity without regard to direction, making it ideal for comparing and clamping after comparing to topSpeed
            if (myRB.velocity.magnitude > topSpeed)
            {
                //ClampMagnitude is an easy way to adjust velocity using easily readible magnitude values
                myRB.velocity = Vector2.ClampMagnitude(myRB.velocity, topSpeed);
            }
        }
        //If nothing is pressed, begin slowing things down
        else
        {
            myRB.velocity = myRB.velocity * slowAmount;
            //Since we dont want to be slowing things forever (Just using the multiplication method above would never end) we need to manually zero it
            if (myRB.velocity.magnitude <= 0.1f) myRB.velocity = Vector2.zero;
        }
    }

    //Debug shit
    /*void OnDrawGizmosSelected()
    {
         // Draw a yellow sphere at the transform's position
         Gizmos.color = Color.yellow;
         Gizmos.DrawWireSphere(transform.position, GrabDistance);
         Gizmos.color = Color.blue;
         Gizmos.DrawLine(transform.position, transform.InverseTransformDirection(myRB.velocity)+ transform.position);
    }*/
}
