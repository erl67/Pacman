using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    private Rigidbody rb;
    private Transform target;
    private NavMeshAgent agent;

    private int score = 0, lives = 3;
    public Text txtScore;
    public Text txtLives;
    public Text txtLevelEnd;

    void Start () {
        agent = gameObject.GetComponent<NavMeshAgent>();
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update () {
        float moveH = Input.GetAxis("Horizontal2");
        float moveV = Input.GetAxis("Vertical2");
        if (moveH != 0f)
        {
            Vector3 motion = new Vector3(moveH, 0f, 0f);
            rb.AddForce(motion * 10f);
        }
        else if (moveV != 0f)
        {
            Vector3 motion = new Vector3(0f, 0f, moveV);
            rb.AddForce(motion * 10f);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = transform.position + new Vector3(2f, 0f, 0f);

        }

        //float h = Input.GetAxis("Horizontal");
        //float v = Input.GetAxis("Vertical");
        //rb.AddForce(new Vector3(h, 0f, v));

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                agent.destination = hit.point;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player collision " + other.tag);

        if (other.tag.Equals("dot"))
        {
            Destroy(other.gameObject);
            score++;
            //txtScore.text = "Score: " + score;
            //txtLives.text = "Lives: " + lives;
        }

        if (other.tag.Equals("ghost"))
        {
            //GameController.instance.MuteBG();
            //txtLevelEnd.text = "Game Over\nYour Final Score is: " + score.ToString();
            //txtLevelEnd.text += "\nPress (r) to continue";
            //Time.timeScale = 0;
            //GameController.instance.PlayerDead();
        }
    }



    //private void LateUpdate()
    //{
    //    Debug.Log("x: " + rb.transform.position.x + "z: " + rb.transform.position.z + "y: " + rb.transform.position.y);
    //}
}
 