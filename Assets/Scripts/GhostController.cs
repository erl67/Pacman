﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostController : MonoBehaviour
{
    private Rigidbody rb;

    private float speed, timer;

    public Transform target;
    private NavMeshAgent agent;
    private MeshRenderer mr;
    private Tree tree;

    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        rb = gameObject.GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent.destination = target.position;

        timer = Time.time + 5f;

        //mr = gameObject.GetComponent<MeshRenderer>();
        //mr = gameObject.GetComponent<Tree>().GetComponent<MeshRenderer>();

        //tree = gameObject.GetComponent<Tree>();


        rb.transform.localScale *= Random.Range(.9f, 1.1f);
        agent.speed = Random.Range(3f, 5f);
        //mr.material.color = Color.red;
        //gameObject.GetComponent<MeshRenderer>().material.color = new Color(0f, 0f, Random.Range(0f,1f), 1);

    }

    void FixedUpdate()
    {

        if (timer < Time.time)
        {
            timer = Time.time + 5f;
            Debug.Log(this.name + " " + agent.transform.position);
            agent.destination = target.position;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Ghost collision " + other.tag);

        if (other.tag.Equals("player"))
        {
            //Destroy(gameObject);
        }
    }


}
