using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Structure to hold x and y. While similar to vector 2, because its a struct, we can compare it and pathstep
public struct point
{
    public int x, y;
    //Declarator
    public point(int x, int y)
    {
        //this.x is the public int x we declared just above
        this.x = x;
        this.y = y;
    }
}

public class PathStep {
    //Pathstep stores path data for the individual paths of a path. More info in GridScript.cs
    public point position; //Location this step is trying to go to
    public PathStep previous; //The PathStep that was used before this
	// Use this for initialization
	
    public PathStep(point position, PathStep previous)
    {
        this.position = position;
        this.previous = previous;
    }
}
