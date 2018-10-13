using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftableCanvas : MonoBehaviour {


    public Liftable myBox;
    public GameObject ButtonY;
    // Use this for initialization

    void LateUpdate () {
        if (myBox.isBuildable && !myBox.onSpawner && !ButtonY.activeSelf)
        {

            ButtonY.SetActive(true);
        }
        else if (!myBox.isBuildable || (myBox.onSpawner && ButtonY.activeSelf))
        {
            ButtonY.SetActive(false);
        }
        
        
	}
}
