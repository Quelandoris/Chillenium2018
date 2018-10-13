using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform player;

    private Vector3 offset;

	// Use this for initialization
	void Start () {
        offset = transform.position - player.position;
	}
	
	// LateUpdate is called once per frame, after Update
	void LateUpdate () {
        transform.position = player.position + offset;
	}
}
