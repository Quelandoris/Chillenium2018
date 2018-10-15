using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftableCanvas : MonoBehaviour {

    //Liftable canvas updates the UI used by the Liftable object. For the most part, it just toggles the ButtonY object

    //public variables can be edited and assigned in the inspector.
    public Liftable myBox; //The box this UI is controlled by.
    public GameObject ButtonY; //Despite the variable name, this actually corresponds to the Z button on the UI.
    

    //LateUpdate happens after every single objects' Updates but before rendering, so we can be sure all the applicable variables have been changes.
    void LateUpdate () {
        //If this UI's box is buildable, not on its spawner, and the Ybutton isn't enabled, enable it
        if (myBox.isBuildable && !myBox.onSpawner && !ButtonY.activeSelf)
        {
            ButtonY.SetActive(true);
        }
        //If this UI's box isn't buildable or is on the spawner and is active, disable.
        else if (!myBox.isBuildable || (myBox.onSpawner && ButtonY.activeSelf))
        {
            ButtonY.SetActive(false);
        }
        
        
	}
}
