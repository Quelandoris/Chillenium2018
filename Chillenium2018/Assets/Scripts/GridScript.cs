using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScript : MonoBehaviour {

    //GridScrpt creates a pseudo-grid for snapping built objects to look as though they're tiling correctly. It also is able to read
    //if a particular point on the grid has a wall on it (would be impassible), that is also tracked


    //Public variables are editable in the inspector, so its modular and easy to change on the fly
    public float GridDimension=1; //Welcome to the Grid Dimension
    //This determines the spacing between Individual points
    public int MapWidth; // How many points to the rght need to be made
    public int MapHeight; //How many points up need to be made
    public GameObject basicTile; // Game object used as a references for floor tiles
    public GameObject InvisWall; //Barrier to prevent player leaving gamespace
    public bool[,] OccupiedGrid; //Checks if the point on the grid is occupied by an impassible object. 2d Aray to track both up and down
    //In this array, true means the space is occupied, false means it's open

    private void Start()
    {
        //initialize the grid to be the same length as the map
        OccupiedGrid = new bool[MapWidth, MapHeight];

    }

    //AI Algorithm to create a path between and around impassible objects using OccupiedGrid as a reference 
    //and then converting it to world space thanks to GridDimension
    //Takes a point (x and y) for start and destination
    //point is declared in PathStep.cs, along with the constructor for PathStep
    //Called by GuardBrain.cs
    //Realistically, you almost never need to do this and you could just use Unity's Navmesh tools to do this in like 2 lines of code.
    //This is also extremely resource intensive and the game will noticably hitch everytime you call this function.
    //But because Unity2d is basically incomplete, we have no choice
    public List<point> GeneratePath(point start, point destination)
    {
        //Create a queue of points for GuardBrain to test for navigation
        Queue<PathStep> unVisited = new Queue<PathStep>();
        //When points are processed by unVisited, it gets added to visited
        List<point> Visited = new List<point>();
        //Start by Enqueuing (Putting into the end of the queue) the start position
        unVisited.Enqueue(new PathStep(start, null));
        //When every point on the path has been checked, Unvisited will be equal to 0 and and leave the while loop
        while (unVisited.Count > 0)
        {
            //Start by removing the first item from unVisited and putting into a temp PathStep. 
            //This increments the while and lets us work with its value easier
            PathStep thisStep = unVisited.Dequeue();
            //Check if the point we're testing is our destination. If it is, finalize the path and pass it along to GuardBrain
            if (thisStep.position.Equals(destination))
            {
                //Create a list of the points used to reach the destination. 
                //Because we used a queue starting from the start, when things get put into this list, they'll start from the destination instead
                List<point> path = new List<point>();
                //Create a holder for our last step
                PathStep cur = thisStep;
                //add the first item to our path
                path.Add(cur.position);
                //Each PathStep also holds the data for the step it took before that. 
                //This allows us to work our way backwards through where we've been to figure out where we need to go
                //This while loop keeps checking the previous step until it gets to a PathStep that doesn't have a previous step
                //This is the first step, and it means our path is built.
                while (cur.previous != null)
                {
                    //switch which PathStep we're working with
                    cur = cur.previous;
                    //Add it to the path
                    path.Add(cur.position);
                }
                //Once we have all our points, we can reverse the path so the start is now first and the AI can go through them properly.
                path.Reverse();
                //Send the path to the object that caled this function.
                return path;
            }
            //Until we have our destination, we need to try the squares above, below, to the right, and to the left of this
            //try-catch lets you do something that may crash the game, and when it would crash, simply doesn't do it and runs whatever is in catch instead
            //In this case, we're doing it to prevent an ArrayOutOfBounds if we try to check a point that doesn't exist (Like if we're on the top row and try to check the row above us)
            try
            {
                //x-1 would be the step to the left of our current one, for example.
                if (!OccupiedGrid[thisStep.position.x - 1, thisStep.position.y])
                {
                    //If the grid space isn't occupied, it checks to make usre we haven't already checed that space, and if we haven't...
                    if (!Visited.Contains(new point(thisStep.position.x - 1, thisStep.position.y)))
                    {
                        //...it enqueues it for unVisited.
                        unVisited.Enqueue(new PathStep(new point(thisStep.position.x - 1, thisStep.position.y), thisStep));
                    }
                }
            }catch { }
            try
            {
                if (!OccupiedGrid[thisStep.position.x + 1, thisStep.position.y])
                {
                    if (!Visited.Contains(new point(thisStep.position.x + 1, thisStep.position.y)))
                    {
                        unVisited.Enqueue(new PathStep(new point(thisStep.position.x + 1, thisStep.position.y), thisStep));
                    }
                }
            }
            catch { }
            try
            {
                if (!OccupiedGrid[thisStep.position.x, thisStep.position.y - 1])
                {
                    if (!Visited.Contains(new point(thisStep.position.x, thisStep.position.y - 1)))
                    {
                        unVisited.Enqueue(new PathStep(new point(thisStep.position.x, thisStep.position.y - 1), thisStep));
                    }
                }
            }
            catch { }
            try
            {
                if (!OccupiedGrid[thisStep.position.x, thisStep.position.y + 1])
                {
                    if (!Visited.Contains(new point(thisStep.position.x, thisStep.position.y + 1)))
                    {
                        unVisited.Enqueue(new PathStep(new point(thisStep.position.x, thisStep.position.y + 1), thisStep));
                    }
                }
            }
            catch { }
        }
        //If no path could be found, return null to avoid crashing anything
        return null;
    }

    //When you would place an object, you call this, passing along the position of the new object and the bool true
    //Alternatively, if you would destroy an object, call this and pass the bool false
    //This sets the position in our array that corresponds to our object's world position, and changes the value in the array accordingly
    public void CreateOrDestroy(Vector3 position, bool set)
    {
        int xCount = Mathf.RoundToInt(position.x / GridDimension);
        int yCount = Mathf.RoundToInt(position.y / GridDimension);
        OccupiedGrid[xCount, yCount] = set;
    }

    //Function to convert a world position to a grid position
    //This is essentially what our grid is, and its caleld by Liftable to determine where it snaps to when its dropped.
    public Vector3 NearestPointOnGrid(Vector3 position)
    {
        //Adjust the passed along position to be relative to the position of the object this script is attached to (Which could potentially throw calculations off)
        position -= transform.position;
        //Since arrays only have whole-number positions, but world space can have float numbers, we need to round things.
        //We also need to account for the size of our GridDimension, so that these spaces still correspond to world space
        int xCount = Mathf.RoundToInt(position.x / GridDimension);
        int yCount = Mathf.RoundToInt(position.y / GridDimension);
        //Make a new vector 3 to return. Since we're 2d, our Z is just 0
        Vector3 result = new Vector3(
            (float)xCount * GridDimension,
            (float)yCount * GridDimension,
            0
            );
        //Readjust to convert the position back to world space
        result += transform.position;
        return result;
    }
    //OnDrawGizmos and OnDrawGizmosSelected are two built in functions used to draw gizmos, useful debug and reference options.
    //They can be used to draw spheres, lines, boxes, etc that can only be seen in the scene view, so it doesn't effect gameplay at all.
    //Highly useful for trying to figure out distances or from general debugging.
    //In this case I'm using OnDrawGizmosSelected so it only shows while I have that object selected in the inspector.
    private void OnDrawGizmosSelected()
    {
        //Makes a small white dot appear at the points of my grid.
        Gizmos.color = Color.white;
        for(float x = 0; x < MapWidth; x += GridDimension)
        {
            for (float y = 0; y < MapHeight; y += GridDimension)
            {
                var point = NearestPointOnGrid(new Vector3(x, y, 0));
                Gizmos.DrawSphere(point, 0.05f);
            }
        }
    }
}
