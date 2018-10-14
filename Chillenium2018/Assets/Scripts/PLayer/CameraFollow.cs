using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    //Makes camera follow the player. Needs to be attached to camera


    public Transform player; //Get ref to the player's transform
    

    private Vector3 offset; //Internal value, keeps the camera in a consistent place as the game starts.

	// Use this for initialization
	void Start () {
        offset = transform.position - player.position; //Gets diff between player and camera position and saves it
	}
	
	// LateUpdate is called once per frame, after Update
	void LateUpdate () {
        transform.position = player.position + offset; //Applies diff to the camera's position to force the camera to stay in the same place above the player
       
	}
}
