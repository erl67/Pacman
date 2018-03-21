using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostController : MonoBehaviour
{
    private Rigidbody rb;

    private float speed, timer, updateTimer = 1f;

    public Transform target;
    private NavMeshAgent agent;
    private MeshRenderer mr;
    private Tree tree;

    public Material[] mats = new Material[10];

    public AudioSource ghostDie, ghostSpawn;
    public bool playOnce = true;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();

        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.Warp(gameObject.transform.position);  //make sure it's on the mesh
        agent.speed = Random.Range(4f, 10f);

        target = GameObject.Find("Pacman").transform;

        timer = Time.time + 8f;

        ghostDie = GameObject.Find("GhostSound").gameObject.GetComponent<AudioSource>();
        ghostSpawn = GameObject.Find("GhostSpawn").gameObject.GetComponent<AudioSource>();    //clones won't play attached audio
        ghostSpawn.Play();

        var tree = gameObject.GetComponentInChildren<Tree>();
        var tr = tree.GetComponent<Renderer>();
        var mr = tree.GetComponent<MeshRenderer>();

        Debug.Log(gameObject.name + "mainTexture: " + tr.material.mainTexture);
        Debug.Log("mats: " + mats.Length);

        tr.material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));


        //tr.material = mats[random.Next(0, mats.Length)];
        //tr.material = mats[(int)System.Math.Floor(Random.Range(0f, mats.Length))];
        //tr.material = mats[2];


        tr.material = mats[1];






        //Debug.Log("Mats " + mats.Length);
    }

    void FixedUpdate()
    {
        if (timer < Time.time)
        {
            rb.transform.localScale *= Random.Range(.9f, .97f);
            agent.speed *= rb.transform.localScale.y;
            agent.ResetPath();

            if (agent.speed < 2f && playOnce)
            {
                playOnce = agent.enabled = false;
                ghostDie.Play();
                rb.angularVelocity = new Vector3(Random.Range(30f, 120f), Random.Range(30f, 120f), Random.Range(30f, 120f));
                Destroy(gameObject, 4f);
            }
            else if (playOnce)
            {
                timer = Time.time + Random.Range(5f, 8f);
                //Debug.Log(this.name + " " + transform.position + " " + transform.localScale + " " + transform);
            }
        }

        if (updateTimer < Time.time)
        {
            agent.destination = target.position;
            updateTimer = Time.time + 1f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Ghost collision " + other.tag);

        if (other.tag.Equals("player"))
        {
            Debug.Log("Ghost died : " + gameObject.transform.position);
            Destroy(gameObject);
        }
    }



}
