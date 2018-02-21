using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ghost : MonoBehaviour {

    public GameObject body;
    public GameObject mouth;
    public GameObject head;
    private Rigidbody rb;

    private float speed, lifespan;

    void Start () {
        rb = gameObject.GetComponent<Rigidbody>();
        speed = Random.Range(1f, 2f);

        rb.transform.localScale *= Random.Range(.1f, 2f);

    }

    void Update () {
        //Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        Vector3 movement = new Vector3(10f, 0.0f, 10f);

        rb.AddForce(movement * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Ghost collision " + other.tag);

        if (other.tag.Equals("player"))
        {
            GhostDies();
        }
    }

    private void GhostDies()
    {
        Destroy(gameObject);
    }

    void OnBecameInvisible()
    {
        Debug.Log("Ghost Invisibile");
        //Destroy(gameObject);
    }
}
