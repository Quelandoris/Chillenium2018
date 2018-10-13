using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liftable : MonoBehaviour {

    public bool isBuildable;
    public GameObject toBuild;
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
            myUI.SetActive(false);
        }
        if (near && !carried && !myUI.activeSelf)
        {
            myUI.SetActive(true);
        }
        else if(!near && myUI.activeSelf || carried)
        {
            myUI.SetActive(false);
        }
        near = false;
	}

    //Called by player if this bad boy is buildable and turns him into a corresponding wall segment.
    public void Build()
    {
        if (isBuildable)
        {
            Instantiate(toBuild, transform.position, transform.rotation);
        }
        else Debug.Log("Build called without being buildable. Why did this happen?");
    }

    private void LateUpdate()
    {
        //if (near && !myUI.activeSelf) myUI.SetActive(true);
        //else if (!near) myUI.SetActive(false);
    }
    private void OnCollisionEnter(Collision coll)
    {
        
    }
}
