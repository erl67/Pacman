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
    private float timer = 1f;

    public GameObject player;
    public Maze mazeInstance;

    private int score = 0, lives = 3;
    public Text txtScore, txtLives, txtCenter;

    public AudioSource[] sounds;
    public AudioSource eatPill, loseLife, endSound;

    IEnumerator Start () {
        agent = gameObject.GetComponent<NavMeshAgent>();
        rb = gameObject.GetComponent<Rigidbody>();
        player = GameObject.Find("Pacman").gameObject;
        agent.Warp(player.transform.position);

        txtLives.text = "Lives: " + lives;
        txtScore.text = "Score: " + score;

        sounds = GetComponents<AudioSource>();
        eatPill = sounds[0];
        loseLife = sounds[1];
        endSound = GameObject.Find("GameOver").gameObject.GetComponent<AudioSource>();


        yield return new WaitForSecondsRealtime(3);
        mazeInstance = GameObject.Find("mazeInstance").GetComponent<Maze>();
    }

    void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ////transform.position = transform.position + new Vector3(1f, 0f, 2f);
            rb.AddForce(new Vector3(1f, 50f, 0f), ForceMode.Impulse);
            rb.AddForce(new Vector3(1f, 50f, 0f));

            Debug.Log("Pacman " + gameObject.transform.position.ToString());
        }

        //agent.enabled = true;
        if (Input.GetMouseButtonDown(0))
        {
            
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {
                agent.destination = hit.point;
                Debug.Log("Pacman moving to " + agent.destination);
            }
        }
        else if (timer < Time.time)
        {
            //agent.destination = gameObject.transform.position;
            agent.ResetPath();
            rb.velocity = Vector3.zero;
            timer = Time.time + 1f;
            Debug.Log("Pacman reset");
            //agent.enabled = false;
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
            rb.velocity = Vector3.zero;
            rb.AddForce(new Vector3(0f, 0f, 500f));
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            //agent.destination = Vector3.zero;
            rb.velocity = Vector3.zero;
            rb.AddForce(new Vector3(0f, 0f, -500f));
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            //agent.destination = Vector3.zero;
            rb.velocity = Vector3.zero;
            rb.AddForce(new Vector3(-500f, 0f, 0f));
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            //agent.destination = Vector3.zero;
            rb.velocity = Vector3.zero;
            rb.AddForce(new Vector3(500f, 0f, 0f));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("PC " + other.tag + " " + other.transform.position + " @ " + gameObject.transform.position);

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
        lives--;
        txtLives.text = "Lives: " + lives;

        if (lives < 1) { endSound.Play(); }
        else { loseLife.Play(); };

        Time.timeScale = 0;

        float intWall = 7.5f - lives;
        float extWall = 14.5f + lives;

        var walls = GameObject.FindGameObjectsWithTag("mazewallObs");
        foreach (GameObject wall in walls)
        {
            if ((System.Math.Abs(wall.transform.position.x) < intWall && System.Math.Abs(wall.transform.position.z) < intWall) ||
                (System.Math.Abs(wall.transform.position.x) > extWall && System.Math.Abs(wall.transform.position.z) > extWall))
            {
                Destroy(wall);
            }
        }

        mazeInstance.ToggleMaze();

        if (lives < 1)
        {
            txtCenter.text = "Game Over\nYour Final Score is: " + score.ToString();
            txtCenter.text += "\n\nPress (r) to try again";
            GameController.instance.PlayerDead();
        }
        else
        {
            txtCenter.text = "\nYou got eaten by a ghost.\nPress (r or space) to continue";
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
        player.transform.position = new Vector3(0f, -.75f, 0f);
        agent.Warp(player.transform.position);
    }

}
 