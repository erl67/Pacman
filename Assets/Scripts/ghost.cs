using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ghost : MonoBehaviour {

    //public GameObject body;
    //public GameObject mouth;
    //public GameObject head;
    private Rigidbody rb;

    private float speed, lifespan;

    public Transform target;
    private NavMeshAgent agent;

    void Start () {
        agent = gameObject.GetComponent<NavMeshAgent>();

        rb = gameObject.GetComponent<Rigidbody>();
        speed = Random.Range(1f, 3f);

        rb.transform.localScale *= Random.Range(.9f, 1.1f);
        //gameObject.GetComponent<MeshRenderer>().material.color = new Color(0f, 0f, Random.Range(0f,1f), 1);

    }

    void FixedUpdate () {
        agent.destination = target.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Ghost collision " + other.tag);

        if (other.tag.Equals("player"))
        {
            //GhostDies();
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
