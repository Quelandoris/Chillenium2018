using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftableSpawner : MonoBehaviour {


    public float checkRange;
    public GameObject spawn;
		
	// Update is called once per frame
	void Update () {
        Collider2D[] circleHit = Physics2D.OverlapCircleAll(transform.position, checkRange);
        bool isThereAnyLiftable=false;
        foreach (Collider2D coll in circleHit)
        {
            if (coll.GetComponent<Liftable>())
            {
                isThereAnyLiftable = true;
                coll.GetComponent<Liftable>().onSpawner = true;
            }
        }
        if (!isThereAnyLiftable)
        {
            Instantiate(spawn, transform.position, transform.rotation);
        }
    }
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, checkRange);
    }
}
