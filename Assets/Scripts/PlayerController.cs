using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    private Rigidbody rb;
    private Transform target;
    private NavMeshAgent agent;
    private RaycastHit hit;

    //private GameController gc;
    //public PlayerLifespan pl;
    //public GameObject player;
    public Maze mazeInstance;

    private int score = 0, lives = 3;
    public Text txtScore, txtLives, txtCenter;
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
            ////transform.position = transform.position + new Vector3(1f, 0f, 2f);
            rb.AddForce(new Vector3(1f, 50f, 0f), ForceMode.Impulse);
            rb.AddForce(new Vector3(1f, 50f, 0f));

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
            agent.destination = Vector3.zero;
            rb.velocity = Vector3.zero;
            rb.AddForce(new Vector3(0f, 0f, 50f));
        }
        if (Input.GetKeyDown(KeyCode.End))
        {
            agent.destination = Vector3.zero;
            rb.velocity = Vector3.zero;
            rb.AddForce(new Vector3(0f, 0f, -50f));
        }
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            agent.destination = Vector3.zero;
            rb.velocity = Vector3.zero;
            rb.AddForce(new Vector3(-50f, 0f, 0f));
        }
        if (Input.GetKeyDown(KeyCode.PageDown))
        {
            agent.destination = Vector3.zero;
            rb.velocity = Vector3.zero;
            rb.AddForce(new Vector3(50f, 0f, 0f));
        }
    }


    public void NewLife()
    {
        Time.timeScale = 0;
        var ghosts = GameObject.FindGameObjectsWithTag("ghost");
        foreach (GameObject ghost in ghosts) Destroy(ghost);
        StartCoroutine(StartBox());
    }

    public IEnumerator StartBox()
    {
        while (!Input.anyKey)
        {
            yield return null;
        }
        txtCenter.text = "";
        GameController.instance.MakeGhosts();
        mazeInstance.ToggleMaze();
        GameObject.Find("Pacman").transform.position = new Vector3(0f, 0f, 0f);
        Time.timeScale = 1;
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
            lives--;
            txtLives.text = "Lives: " + lives;

            Time.timeScale = 0;
            mazeInstance = GameObject.Find("mazeInstance").GetComponent<Maze>();

            if (lives == 0)
            {
                txtCenter.text = "Game Over\nYour Final Score is: " + score.ToString();
                GameController.instance.PlayerDead();
                mazeInstance.ToggleMaze();
            }
            else
            {
                Time.timeScale = 0;
                mazeInstance.ToggleMaze();
                txtCenter.text = "\nYou got eaten by a ghost.\nPress (r) to continue";
                NewLife();
            }
        }
    }

}
 