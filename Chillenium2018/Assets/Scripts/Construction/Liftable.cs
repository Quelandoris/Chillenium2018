using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Liftable : MonoBehaviour {

    //Liftable is the class that makes an object able to be carried by the player and built into a different kind of object
    //Needs to b attached to an object with a trigger box collider

    //public variables are editable in the inspector, making them easily modular
    public bool isBuildable; //Is this particular object able to be built? Allows you to use this script for objects that can be carried but not built, like corpses
    public float BuildTime; //How long it takes to build this item. If isBuildable is false, this isn't used at all.
    public GameObject toBuild; //GameObject that's created when this finished building. If isBuildable is false, this isn't used at all.
    public GameObject Carrier; //The gameobject carrying this object. Almost always the player, but potentially could be expanded to include the worker AIs
    public bool carried; //is this object currently being carried?
    public Vector3 offset; //Where is this object position relative to Carrier when carried==true
    public bool near; //is the player nearby? if not, hide the UI
    public GameObject myUI; //Objects personal UI, a miniature canvas that appears above the object's head
    public Image buildProgBar; //progress bar on the Press Z button. Fills up while player is building. If isBuildable is false, this isn't used at all.
    public bool onSpawner; //Checks if the object is currently on top of the object that spawned it. Isn't possible to build while this is true. This makes sure you don't accidentally build a wall and block the spawner.
    Sprite baseSprite; //The sprite the bricks normally use
    GridScript TheGrid; //Welcome to the Grid.
    public Sprite inProgBuild; //sprite the object switches to while its being built. If isBuildable is false, this isn't used at all.
    float buildProg; //timer to track  If isBuildable is false, this isn't used at all.

    private void Start()
    {
        TheGrid = FindObjectOfType<GridScript>(); //Get a reference to the GridScript that's in the scene.
        baseSprite = GetComponent<SpriteRenderer>().sprite; //Assign the baseSprite by finding the sprite that is assigned to its SpriteRenderer at creation
    }

    // Update is called once per frame
    void Update () {
        onSpawner = false; //Default onSpawner to false to avoid edge cases
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
        near = false; //near is checked in LiftableCanvas.cs during LateUpdate, so we set it to false now. PLayerControls.cs will change it.
	}

    //Called by player if this bad boy is buildable and turns him into a corresponding wall segment.
    public void Build()
    {
        //Switch the spritr while building.
        GetComponent<SpriteRenderer>().sprite = inProgBuild;
        //Update the build timer. Build is done when buildprog==1, so if we divide 1 by BuildTime, we get the frames needed to complete the build
        buildProg += 1 / BuildTime;
        if(buildProg<1) buildProgBar.fillAmount = buildProg; //When less than one, update buildProgBar to match buildProg
        else buildProgBar.fillAmount = 1; //Errors occur when thr progress bar goes past 1, so we cap it
        //When fnished building, destroy the object, instantiate toBuild, and then mark the point on TheGrid as being occupied.
        if (buildProg >= 1)
        {
            Destroy(gameObject);
            Instantiate(toBuild, TheGrid.NearestPointOnGrid(transform.position), transform.rotation);
            TheGrid.CreateOrDestroy(TheGrid.NearestPointOnGrid(transform.position), true);
            
        }
    }
    //If you release z before you're done building, it resets build progress
    public void CancelBuild()
    {
        buildProg = 0;
        buildProgBar.fillAmount = 0;
        GetComponent<SpriteRenderer>().sprite = baseSprite;
    }
}
