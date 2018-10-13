using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liftable : MonoBehaviour {


    public GameObject Carrier;
    public bool carried;
    public Vector3 offset;
    public bool near;
    public GameObject myUI;
    public bool onSpawner;
		
	// Update is called once per frame
	void Update () {
        onSpawner = false;
        if (carried)
        {
            transform.position = Carrier.transform.position+offset;
        }
        if (near && !myUI.activeSelf)
        {
            myUI.SetActive(true);
        }
        else if(!near && myUI.activeSelf)
        {
            myUI.SetActive(false);
        }
        if (near) near = false;
	}

    private void LateUpdate()
    {
        if (near && !myUI.activeSelf) myUI.SetActive(true);
        else if (!near) myUI.SetActive(false);
    }
    private void OnCollisionEnter(Collision coll)
    {
        
    }
}
