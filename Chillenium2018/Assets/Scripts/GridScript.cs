using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScript : MonoBehaviour {

    

    public float GridDimension=1; //Welcome to the Grid Dimension
    public int MapWidth;
    public int MapHeight;
    public GameObject basicTile;
    public GameObject InvisWall; //Barrier to prevent player leaving gamespace
    public bool[,] OccupiedGrid;

    private void Start()
    {
        //CreateTiles();
        OccupiedGrid = new bool[MapWidth, MapHeight];

    }

    public List<point> GeneratePath(point start, point destination)
    {
        Queue<PathStep> unVisited = new Queue<PathStep>();
        List<point> Visited = new List<point>();
        unVisited.Enqueue(new PathStep(start, null));
        while (unVisited.Count > 0)
        {
            PathStep thisStep = unVisited.Dequeue();
            if (thisStep.position.Equals(destination))
            {
                List<point> path = new List<point>();
                PathStep cur = thisStep;
                path.Add(cur.position);
                while (cur.previous != null)
                {
                    cur = cur.previous;
                    path.Add(cur.position);
                }
                path.Reverse();
                return path;
            }
            try
            {
                if (!OccupiedGrid[thisStep.position.x - 1, thisStep.position.y])
                {
                    if (!Visited.Contains(new point(thisStep.position.x - 1, thisStep.position.y)))
                    {
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
        return null;
    }

    public void CreateOrDestroy(Vector3 position, bool set)
    {
        int xCount = Mathf.RoundToInt(position.x / GridDimension);
        int yCount = Mathf.RoundToInt(position.y / GridDimension);
        OccupiedGrid[xCount, yCount] = set;
    }

    public Vector3 NearestPointOnGrid(Vector3 position)
    {
        position -= transform.position;
        int xCount = Mathf.RoundToInt(position.x / GridDimension);
        int yCount = Mathf.RoundToInt(position.y / GridDimension);
        //int zCount = Mathf.RoundToInt(position.z / GridDimension);
        Vector3 result = new Vector3(
            (float)xCount * GridDimension,
            (float)yCount * GridDimension,
            0
            );
        result += transform.position;
        return result;
    }
    private void OnDrawGizmosSelected()
    {
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
