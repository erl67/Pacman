using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public GameObject pacman;
    private Vector3 offset;


	void Start () {
        offset = transform.position - pacman.transform.position;
	}
	
	void LateUpdate () {
        transform.position = pacman.transform.position + offset;

    }
}
