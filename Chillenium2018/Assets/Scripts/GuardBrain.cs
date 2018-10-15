using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardBrain : MonoBehaviour {

    //Public variables can be edited in the inspector


    GridScript TheGrid; //Welcome to The Grid
    public bool hasBricks; //Is the guard carrying any bricks yet? This determines the "mode" of the AI
    public LiftableSpawner curBrick; //The brick spawner it will use as a navigation target
    public float checkDist; //How far you need to be before the AI considers itself to have arrived
    public float stepSpeed = 2; //how far the Guard moves at one time
    List<point> myPath = new List<point>(); //Stores path info
    
    // Use this for initialization
	void Start () {
        TheGrid = FindObjectOfType<GridScript>(); //Assign the grid to the manager thats in the scene
	}
	
	// Update is called once per frame
	void Update () {
        //Decide which AI mode to use based on whether 
        if (hasBricks) GoBricks();
        else NoBricks();
	}

    //When the AI has bricks, it will pick a point, take them there, and build
    void GoBricks()
    {
        //if we don't have a valid path, pick a location and generate a new path
        if (myPath == null || myPath.Count <= 0)
        {
            Vector3 ranTarget; //Initialize a holder for our target location
            do
            {
                float ranX = Random.Range(0, TheGrid.MapWidth); //Randomize a x location within the grid's width
                float ranY = Random.Range(0, TheGrid.MapHeight); //Randomize a y location within the grid's height

                ranTarget = TheGrid.NearestPointOnGrid(new Vector3(ranX, ranY, 0)); //Turn those elements into a Grid position
            } while (TheGrid.OccupiedGrid[(int)ranTarget.x, (int)ranTarget.y]); //Check if that spot is occupied, and do it again if it is
            Vector3 curpoint = TheGrid.NearestPointOnGrid(new Vector3(ranTarget.x,ranTarget.y,ranTarget.z)); // Once the target is finalized, pass along the target

            myPath = TheGrid.GeneratePath(new point((int)curpoint.x, (int)curpoint.y), new point((int)ranTarget.x, (int)ranTarget.y)); //Generate path to target

        }
        else
        {
            //Set navtarget to current path target.
            Vector3 curNav = new Vector3(myPath[0].x * TheGrid.GridDimension, myPath[0].y * TheGrid.GridDimension,0);
            //Check distance to curNav, if its below checkDIst, build a thing, reset path, and switch modes.
            if (Vector2.Distance(transform.position, curNav) < checkDist)
            {
                hasBricks = false;
                myPath = null;
                //Instantiate(toBuild, TheGrid.NearestPointOnGrid(transform.position), transform.rotation);
            }
            StepForward(); //Move towards nav target
            /*else
            {
                StepForward();
            }*/
        }
    }

    //When AI doesn't have bricks, go get bricks
    void NoBricks()
    {
        //If you don't have a valid path, find one
        if (myPath == null || myPath.Count <= 0)
        {
            //initial implementation: Find the nearest Brickspawner and go to that.
            /*LiftableSpawner[] findBricks = FindObjectsOfType<LiftableSpawner>(); //Create an array of all Spawners in the scene
            Debug.Log(findBricks.Length.ToString());
            float dist = 7000; //Just an absurdly high number; everything should be less than this which is why we start it so high
            LiftableSpawner curBrick = null; //Initialize a holder
            //Check every item in the array
            foreach (LiftableSpawner l in findBricks)
            {
                //Check how far the current spawner is from the player, try and find the closest one by replacing the cur with whatever is closer than cur
                if (Vector3.Distance(transform.position, l.transform.position) < dist)
                {
                    //If a closer one is found, replace cur and dist with the new spawner found and its distance
                    curBrick = l;
                    dist = Vector3.Distance(transform.position, l.transform.position);
                }
            }*/
            //Current (debug) implementation: Present Spawner assigned through inspector
            Vector3 currNav = curBrick.transform.position;
            Debug.Log("Getting Bricks; x: " + currNav.x + ", y: " + currNav.y);
            //Generate path
            myPath = TheGrid.GeneratePath(new point((int)TheGrid.NearestPointOnGrid(transform.position).x, (int)TheGrid.NearestPointOnGrid(transform.position).y), new point((int)currNav.x, (int)currNav.y));
        }
        else
        {
            //Set naviation towards the lowest item in path
            Vector3 curNav = new Vector3(myPath[0].x * TheGrid.GridDimension, myPath[0].y * TheGrid.GridDimension, 0);
            //When close (less than checkDist), switch modes
            if (Vector2.Distance(transform.position, curNav) < checkDist)
            {
                hasBricks = true;
                myPath = null;
                //curNav=Vector2.zero;
            }
            Debug.Log("Currently at; x:" + transform.position.x + ", y: " + transform.position.y);
            //move towards target
            StepForward();
            /*else
            {
                StepForward();
            }*/
        }
    }
    //Handles individual frames of movement. I discovered this is wildly inefficient, especially for Unity2D, so you should do literally anything else.
    void StepForward()
    {
        //to avoid a crash, make sure your path is valid (even though this should only be called when the path is valid, its still good to double-check
        if (myPath!=null && myPath.Count > 0)
        {
            //use Vector2.MoveTowards to change position to move it closer to the target. because MoveTowards is ony a helper function, you still have to assign its return value to something
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), new Vector2(myPath[0].x * TheGrid.GridDimension, myPath[0].y * TheGrid.GridDimension), stepSpeed*Time.deltaTime);
        }
    }

}
