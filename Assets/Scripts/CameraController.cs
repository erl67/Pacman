using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public GameObject pacman;
    private Vector3 offset;

    private float x, y, moveH, moveV;
    private Vector3 rotateValue;

    void Start () {
        offset = transform.position - pacman.transform.position;
	}
	
	void LateUpdate () {
        transform.position = pacman.transform.position + offset;
    }

    //https://answers.unity.com/questions/1179680/how-to-rotate-my-camera.html
    void Update()
    {
        moveH = Input.GetAxis("Horizontal");
        moveV = Input.GetAxis("Vertical");

        rotateValue = new Vector3(moveV, moveH * -1, 0);
        transform.eulerAngles = transform.eulerAngles - rotateValue;

        //Debug.Log(x + ":" + y + "   " + moveH + ":" + moveV);
    }
}
