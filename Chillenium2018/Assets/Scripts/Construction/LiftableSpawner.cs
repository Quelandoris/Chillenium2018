using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftableSpawner : MonoBehaviour {
    //Spawns new objects when none is found within its trigger

    public float checkRange; //how far away to check for an object; if the object were spawning is in this area, don't spawn a new one.
    public GameObject spawn; //Object we want to spawn.
		
	// Update is called once per frame
	void Update () {
        //circlecheck using checkrange.
        Collider2D[] circleHit = Physics2D.OverlapCircleAll(transform.position, checkRange);
        //temp bool to track if anything is there
        bool isThereAnyLiftable=false;
        //Check everything that was hit
        foreach (Collider2D coll in circleHit)
        {
            //If something has the component we're looking for...
            if (coll.GetComponent<Liftable>())
            {
                //Set that bool to true
                isThereAnyLiftable = true;
                //Set the Liftable's onSpawner to true, see Liftable.cs 
                coll.GetComponent<Liftable>().onSpawner = true;
            }
        }
        //If we didn't find anything, Initialize our spawn
        if (!isThereAnyLiftable)
        {
            Instantiate(spawn, transform.position, transform.rotation);
        }
    }
    //Use a gizmo to see the area the spawner is checking.
    //See the end of GridScript.cs for info about Gizmos
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, checkRange);
    }
}
