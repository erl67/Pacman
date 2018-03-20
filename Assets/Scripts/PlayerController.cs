using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    private Rigidbody rb;
    private Transform target;
    private NavMeshAgent agent;

    Ray ray;
    RaycastHit hit;

    private int score = 0, lives = 3;
    public Text txtScore;
    public Text txtLives;
    public Text txtCenter;

    void Start () {
        agent = gameObject.GetComponent<NavMeshAgent>();
        rb = gameObject.GetComponent<Rigidbody>();
        txtLives.text = "Lives: " + lives;
        txtScore.text += score;
    }

    void Update () {


        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = transform.position + new Vector3(2f, 2f, 0f);
            Debug.Log("Pacman " + gameObject.transform.position.ToString());
        }

 

        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("Mouse Down");
            //Debug.Log(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100));
            //Debug.Log(Physics.Raycast(transform.localPosition, out hit, Mathf.Infinity));

            //Debug.DrawRay(transform.position, Input.mousePosition - transform.position);


            //ray.origin = transform.position;
            //ray.direction = transform.position + Input.mousePosition;

            //RaycastHit hit;
            //Ray.

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {
                txtCenter.text = "";
                agent.destination = hit.point;
                Debug.Log("Pacman moving to " + agent.destination);

            }
        }

    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Home))
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(new Vector3(0f, 0f, 50f));
        }
        if (Input.GetKeyDown(KeyCode.End))
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(new Vector3(0f, 0f, -50f));
        }
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(new Vector3(-50f, 0f, 0f));
        }
        if (Input.GetKeyDown(KeyCode.PageDown))
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(new Vector3(50f, 0f, 0f));
        }
        score--;
        txtScore.text = "Score: " + score;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player collision " + other.tag);

        if (other.tag.Equals("dot"))
        {
            Destroy(other.gameObject);
            score++;
            txtScore.text = "Score: " + score;        
        }

        if (other.tag.Equals("ghost"))
        {
            //GameController.instance.MuteBG();
            txtCenter.text = "Game Over\nYour Final Score is: " + score.ToString();
            txtCenter.text += "\nPress (r) to continue";
            Time.timeScale = 0;
            GameController.instance.PlayerDead();
        }
    }



    //private void LateUpdate()
    //{
    //    Debug.Log("x: " + rb.transform.position.x + "z: " + rb.transform.position.z + "y: " + rb.transform.position.y);
    //}
}
 