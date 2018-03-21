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

    public GameObject player;
    public Maze mazeInstance;

    private int score = 0, lives = 3;
    public Text txtScore, txtLives, txtCenter;
    public AudioSource eatPill;

    

    IEnumerator Start () {
        agent = gameObject.GetComponent<NavMeshAgent>();
        rb = gameObject.GetComponent<Rigidbody>();
        player = GameObject.Find("Pacman").gameObject;

        txtLives.text = "Lives: " + lives;
        txtScore.text = "Score: " + score;
        eatPill = GetComponents<AudioSource>()[0];

        yield return new WaitForSecondsRealtime(3);
        mazeInstance = GameObject.Find("mazeInstance").GetComponent<Maze>();
    }

    void StartPlay()
    {

    }


    void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ////transform.position = transform.position + new Vector3(1f, 0f, 2f);
            rb.AddForce(new Vector3(1f, 50f, 0f), ForceMode.Impulse);
            rb.AddForce(new Vector3(1f, 50f, 0f));

            Debug.Log("Pacman " + gameObject.transform.position.ToString());
        }


        if (Input.GetMouseButtonDown(0) && agent.isActiveAndEnabled)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {
                agent.destination = hit.point;
                Debug.Log("Pacman moving to " + agent.destination);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            LoseLife();
        }
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            //agent.destination = Vector3.zero;
            //rb.velocity = Vector3.zero;
            rb.AddForce(new Vector3(0f, 0f, 50f));
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            agent.destination = Vector3.zero;
            rb.velocity = Vector3.zero;
            rb.AddForce(new Vector3(0f, 0f, -50f));
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            agent.destination = Vector3.zero;
            rb.velocity = Vector3.zero;
            rb.AddForce(new Vector3(-50f, 0f, 0f));
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            agent.destination = Vector3.zero;
            rb.velocity = Vector3.zero;
            rb.AddForce(new Vector3(50f, 0f, 0f));
        }
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

            LoseLife();
        }
    }

    private void LoseLife()
    {
        //agent.destination = Vector3.zero;
        //agent.enabled = false;

        lives--;
        txtLives.text = "Lives: " + lives;

        Time.timeScale = 0;
        mazeInstance.ToggleMaze();

        if (lives == 0)
        {
            txtCenter.text = "Game Over\nYour Final Score is: " + score.ToString();
            GameController.instance.PlayerDead();
        }
        else
        {
            txtCenter.text = "\nYou got eaten by a ghost.\nPress (r) to continue";
            NewLife();
        }
    }

    public void NewLife()
    {
        var ghosts = GameObject.FindGameObjectsWithTag("ghost");
        foreach (GameObject ghost in ghosts) Destroy(ghost);
        StartCoroutine(StartBox());
    }

    public IEnumerator StartBox()
    {
        yield return new WaitForSecondsRealtime(1);

        while (!(Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Space)))
        {
            yield return null;
        }
        txtCenter.text = "";
        GameController.instance.MakeGhosts();
        mazeInstance.ToggleMaze();

        Time.timeScale = 1;
        player.transform.position = new Vector3(0f, 0f, 0f);
        //agent.enabled = true;
    }

}
 