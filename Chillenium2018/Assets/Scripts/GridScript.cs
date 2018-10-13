using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScript : MonoBehaviour {

    

    public float GridDimension=1; //Welcome to the Grid Dimension
    public float MapWidth;
    public float MapHeight;
    public GameObject basicTile;
    public GameObject InvisWall; //Barrier to prevent player leaving gamespace

    private void Start()
    {
        CreateTiles();
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
    public void CreateTiles()
    {
        for (float x = -MapWidth/2; x < MapWidth / 2; x += GridDimension)
        {
            var ech = NearestPointOnGrid(new Vector3(x, (-MapHeight / 2)-GridDimension, 0));
            Instantiate(InvisWall, ech, transform.rotation);
            var echDos = NearestPointOnGrid(new Vector3(x, MapHeight / 2, 0));
            Instantiate(InvisWall, echDos, transform.rotation);
            for (float y = -MapHeight / 2; y < MapHeight / 2; y += GridDimension)
            {
                var point = NearestPointOnGrid(new Vector3(x, y, 0));
                Instantiate(basicTile, point, transform.rotation);
            }
            
        }
        for (float y = -MapHeight / 2; y < MapHeight / 2; y += GridDimension)
        {
            var point = NearestPointOnGrid(new Vector3((-MapWidth / 2)-GridDimension, y, 0));
            Instantiate(InvisWall, point, transform.rotation);
            var point2 = NearestPointOnGrid(new Vector3(MapWidth / 2, y, 0));
            Instantiate(InvisWall, point2, transform.rotation);
        }
    }
    /*private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        for(float x = -40; x < 40; x += GridDimension)
        {
            for (float y = -40; y < 40; y += GridDimension)
            {
                var point = NearestPointOnGrid(new Vector3(x, y, 0));
                Gizmos.DrawSphere(point, 0.1f);
            }
        }
    }*/
}
