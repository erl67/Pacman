using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    private Rigidbody rb;
    private Transform target;
    private NavMeshAgent agent;

    RaycastHit hit;

    private int score = 0, lives = 3;
    public Text txtScore;
    public Text txtLives;
    public AudioSource eatPill;

    void Start () {
        agent = gameObject.GetComponent<NavMeshAgent>();
        rb = gameObject.GetComponent<Rigidbody>();
        txtLives.text = "Lives: " + lives;
        txtScore.text = "Score: " + score;
        eatPill = GetComponents<AudioSource>()[0];
    }

    void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = transform.position + new Vector3(1f, 0f, 2f);
            Debug.Log("Pacman " + gameObject.transform.position.ToString());
        }

 

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {
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
        lives--;
        txtLives.text = "Lives: " + lives;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player collision " + other.tag);

        if (other.tag.Equals("mazewall"))
        {
            agent.destination = Vector3.zero;
        }

        if (other.tag.Equals("dot"))
        {
            eatPill.Play();
            Destroy(other.gameObject);
            score++;
            txtScore.text = "Score: " + score;        
        }

        if (other.tag.Equals("ghost"))
        {
            //GameController.instance.MuteBG();
            //txtCenter.text = "Game Over\nYour Final Score is: " + score.ToString();
            //txtCenter.text += "\nPress (r) to continue";
            lives--;
            Time.timeScale = 0;
            GameController.instance.PlayerDead();
        }
    }



    //private void LateUpdate()
    //{
    //    Debug.Log("x: " + rb.transform.position.x + "z: " + rb.transform.position.z + "y: " + rb.transform.position.y);
    //}
}
 