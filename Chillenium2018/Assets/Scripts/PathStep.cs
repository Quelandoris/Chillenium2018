using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct point
{
    public int x, y;
    public point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}

public class PathStep {

    public point position;
    public PathStep previous;
	// Use this for initialization
	
    public PathStep(point position, PathStep previous)
    {
        this.position = position;
        this.previous = previous;
    }
}
