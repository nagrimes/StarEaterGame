using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controller : MonoBehaviour {
	public GameObject player;
	private Vector3 offset, camAdjust;
	private float oldSize;
	public Camera thisCamera;
	public int startingCameraSize;

	// Use this for initialization
	void Start () {
		offset = transform.position - player.transform.position;
		//oldSize = player.GetComponent<Entity>().size;
		//camAdjust = new Vector3 (0.0f, 0.0f, 0.0f);

	}

	// Update is called once per frame
	void Update () {

		thisCamera.orthographicSize = startingCameraSize + player.transform.localScale.x;
		transform.position = new Vector3 (player.transform.position.x + offset.x, player.transform.position.y + offset.y, player.transform.position.z + offset.z - (player.transform.localScale.x + 1));
		thisCamera.nearClipPlane = -14 - player.transform.localScale.x;
		thisCamera.farClipPlane = 14 + player.transform.localScale.x;

		/*
		if (player) { //prevents errors if player is destroyed
			if (oldSize < player.GetComponent<Entity>().size) {
					camAdjust += new Vector3 (0.0f, 0.0f, -5.0f);
					oldSize = player.GetComponent<Entity>().size;
			}
			transform.position = Vector3.Lerp(transform.position, player.transform.position + offset + camAdjust, .01f);
		}
		*/
	}
}
