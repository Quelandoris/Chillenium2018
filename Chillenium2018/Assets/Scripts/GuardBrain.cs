using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardBrain : MonoBehaviour {

    GridScript TheGrid; //Welcome to The Grid
    public bool hasBricks;
    public LiftableSpawner curBrick;
    public float checkDist;
    public float stepSpeed = 2;
    List<point> myPath = new List<point>();
    
    // Use this for initialization
	void Start () {
        TheGrid = FindObjectOfType<GridScript>();
	}
	
	// Update is called once per frame
	void Update () {
        if (hasBricks) GoBricks();
        else NoBricks();
	}

    void GoBricks()
    {
        if (myPath == null || myPath.Count <= 0)
        {
            Vector3 ranTarget;
            do
            {
                float ranX = Random.Range(0, TheGrid.MapWidth);
                float ranY = Random.Range(0, TheGrid.MapHeight);

                ranTarget = TheGrid.NearestPointOnGrid(new Vector3(ranX, ranY, 0));
            } while (TheGrid.OccupiedGrid[(int)ranTarget.x, (int)ranTarget.y]);
            Vector3 curpoint = TheGrid.NearestPointOnGrid(new Vector3(ranTarget.x,ranTarget.y,ranTarget.z));

            myPath = TheGrid.GeneratePath(new point((int)curpoint.x, (int)curpoint.y), new point((int)ranTarget.x, (int)ranTarget.y));

        }
        else
        {
            Vector3 curNav = new Vector3(myPath[0].x * TheGrid.GridDimension, myPath[0].y * TheGrid.GridDimension,0);
            if (Vector2.Distance(transform.position, curNav) < checkDist)
            {
                hasBricks = false;
                myPath = null;
                //curNav=Vector2.zero;
            }
            StepForward();
            /*else
            {
                StepForward();
            }*/
        }
    }

    void NoBricks()
    {
        if (myPath == null || myPath.Count <= 0)
        {
            /*LiftableSpawner[] findBricks = FindObjectsOfType<LiftableSpawner>();
            Debug.Log(findBricks.Length.ToString());
            float dist = 7000;
            LiftableSpawner curBrick = null;
            foreach (LiftableSpawner l in findBricks)
            {
                if (Vector3.Distance(transform.position, l.transform.position) < dist)
                {
                    curBrick = l;
                    dist = Vector3.Distance(transform.position, l.transform.position);
                }
            }*/
            Vector3 currNav = curBrick.transform.position;
            Debug.Log("Getting Bricks; x: " + currNav.x + ", y: " + currNav.y);
            myPath = TheGrid.GeneratePath(new point((int)TheGrid.NearestPointOnGrid(transform.position).x, (int)TheGrid.NearestPointOnGrid(transform.position).y), new point((int)currNav.x, (int)currNav.y));
        }
        else
        {
            
            Vector3 curNav = new Vector3(myPath[0].x * TheGrid.GridDimension, myPath[0].y * TheGrid.GridDimension, 0);
            if (Vector2.Distance(transform.position, curNav) < checkDist)
            {
                hasBricks = true;
                myPath = null;
                //curNav=Vector2.zero;
            }
            Debug.Log("Currently at; x:" + transform.position.x + ", y: " + transform.position.y);
            StepForward();
            /*else
            {
                StepForward();
            }*/
        }
    }
    void StepForward()
    {
        if (myPath!=null && myPath.Count > 0)
        {
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), new Vector2(myPath[0].x * TheGrid.GridDimension, myPath[0].y * TheGrid.GridDimension), stepSpeed*Time.deltaTime);
        }
    }

}
