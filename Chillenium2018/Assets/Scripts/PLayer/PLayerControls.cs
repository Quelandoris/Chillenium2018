﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLayerControls : MonoBehaviour {

    //Controls Variables
    public float speed = 3;
    public float topSpeed = 5;
    public float slowAmount = 5;
    public float GrabDistance = 5;

    //Private variables
    Transform myTransform;
    Rigidbody2D myRB;
    Liftable carrying;
    
    // Use this for initialization
	void Start () {
        myTransform = GetComponent<Transform>();
        myRB = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        PlayerMovement();
        Pickup();
	}

    void CheckGrabbable()
    {
        Collider2D[] circleHit = Physics2D.OverlapCircleAll(transform.position, GrabDistance);
        Liftable LiftableHit = null;
        foreach (Collider2D coll in circleHit)
        {
            if (coll.GetComponent<Liftable>() != null)
            {
                Debug.Log("LiftableFound");
                LiftableHit.near = true;
            }
        }
    }

    //X to pickup object when nearby
    void Pickup()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            
            //Object that isnt carried, pick up first object on the stack
            if (carrying == null)
            {
                Collider2D[] circleHit = Physics2D.OverlapCircleAll(transform.position, GrabDistance);
                Liftable LiftableHit=null;
                foreach (Collider2D coll in circleHit)
                {
                    
                    if (coll.GetComponent<Liftable>() != null)
                    {
                        
                        LiftableHit = coll.GetComponent<Liftable>();
                        //goto label1;
                    }
                }
                //label1:
                if (LiftableHit != null)
                {
                   
                    if (LiftableHit.Carrier == null)
                    {
                       
                        LiftableHit.Carrier = gameObject;
                        carrying = LiftableHit;
                        carrying.carried = true;
                    }
                }
            }
            //Drop object
            else
            {
                carrying.Carrier = null;
                carrying.carried = false;
                carrying = null;
            }
        }
    }

    void PlayerMovement()
    {
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
        {
            //Move up: UpArrow
            if (Input.GetKey(KeyCode.UpArrow))
            {
                myRB.AddRelativeForce(Vector2.up * speed);
            }
            //Move down: DownArrow
            if (Input.GetKey(KeyCode.DownArrow))
            {
                myRB.AddRelativeForce(Vector2.down * speed);
            }
            //Move left: LeftArrow
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                myRB.AddRelativeForce(Vector2.left * speed);
            }
            //Move Right: RightArrow
            if (Input.GetKey(KeyCode.RightArrow))
            {
                myRB.AddRelativeForce(Vector2.right * speed);
            }

            if (myRB.velocity.magnitude > topSpeed)
            {
                myRB.velocity = Vector2.ClampMagnitude(myRB.velocity, topSpeed);
            }
        }
        else
        {
            myRB.velocity = myRB.velocity * slowAmount;
            if (myRB.velocity.magnitude <= 0.1f) myRB.velocity = Vector2.zero;
        }
    }

    //Debug shit
    void OnDrawGizmosSelected()
    {
         // Draw a yellow sphere at the transform's position
         Gizmos.color = Color.yellow;
         Gizmos.DrawWireSphere(transform.position, GrabDistance);
    }
}