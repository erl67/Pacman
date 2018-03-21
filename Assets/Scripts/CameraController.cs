using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public GameObject pacman;
    private Vector3 offset;

    private float x, y, moveH, moveV;
    private Vector3 rotateValue;

    public float minFov = 15f;
    public float maxFov = 90f;
    public float sensitivity = 10f;
    public float fov;

    void Start() {
        offset = transform.position - pacman.transform.position;
        Debug.Log(transform.position + " " + pacman.transform.position);
        fov = Camera.main.fieldOfView;
    }

    void LateUpdate() {
        transform.position = pacman.transform.position + offset;

        //https://answers.unity.com/questions/218347/how-do-i-make-the-camera-zoom-in-and-out-with-the.html
        fov += Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        Camera.main.fieldOfView = fov;
    }

    void Update()
    {
        //https://answers.unity.com/questions/1179680/how-to-rotate-my-camera.html
        moveH = Input.GetAxis("Horizontal");
        moveV = Input.GetAxis("Vertical");
        rotateValue = new Vector3(moveV, moveH * -1, 0);
        transform.eulerAngles = transform.eulerAngles - rotateValue;
        //Debug.Log(x + ":" + y + "   " + moveH + ":" + moveV);
    }
}
