using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Liftable : MonoBehaviour {

    public bool isBuildable;
    public float BuildTime;
    public GameObject toBuild;
    public GameObject Carrier;
    public bool carried;
    public Vector3 offset;
    public bool near;
    public GameObject myUI;
    public Image buildProgBar;
    public bool onSpawner;
    Sprite baseSprite;
    GridScript TheGrid;
    public Sprite inProgBuild;
    float buildProg;

    private void Start()
    {
        TheGrid = FindObjectOfType<GridScript>();
        baseSprite = GetComponent<SpriteRenderer>().sprite; 
    }

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
        GetComponent<SpriteRenderer>().sprite = inProgBuild;
        buildProg += 1 / BuildTime;
        if(buildProg<1) buildProgBar.fillAmount = buildProg;
        else buildProgBar.fillAmount = 1;
        if (buildProg >= 1)
        {
            Instantiate(toBuild, TheGrid.NearestPointOnGrid(transform.position), transform.rotation);
            Destroy(gameObject);
        }
    }
    public void CancelBuild()
    {
        buildProg = 0;
        buildProgBar.fillAmount = 0;
        GetComponent<SpriteRenderer>().sprite = baseSprite;
    }

    private void LateUpdate()
    {
       
    }
    private void OnCollisionEnter(Collision coll)
    {
        
    }
}
