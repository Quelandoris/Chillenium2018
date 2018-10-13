using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bombscript : MonoBehaviour {

    public int timer;
	// Use this for initialization
	void Start () {
		Invoke("Explode", timer)
	}
	// Update is called once per frame
	void Explode () {
        //circle check for any built objects
        //if a built object is found, if(coll.tag=="Wall")
        //Destroy(thatobject);
        Destroy(gameObject);
	}
}
