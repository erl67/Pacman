using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    public static GameController instance;
    public bool gameOver;
    public bool spawnGhost = false;

    public GameObject ghostPrefab;
    public int ghostCount;

    public GameObject[] dots;
    public GameObject[] walls;

    private AudioSource background;

    private float volume, timer, timer2;
    private float xOffset = 0f, yOffset = 0f, zOffset = 0f;

    public Maze mazePrefab;
    private Maze mazeInstance;

    public Text txtCenter, txtHelp;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        gameOver = false;
    }

    void Start () {
        Time.timeScale = 0;
        txtCenter.text = "Press any Key to Begin";
        StartCoroutine(StartBox());
        BeginGame();
    }

    private void BeginGame()
    {
        mazeInstance = Instantiate(mazePrefab) as Maze;
        mazeInstance.Generate();
        background = GetComponents<AudioSource>()[0];

        Debug.Log("Beginning Game");

        //StartCoroutine(mazeInstance.Generate());

        dots = GameObject.FindGameObjectsWithTag("dot");

        bool deletedot = false; //reduce number of dots
        foreach (GameObject dot in dots)
        {
            if (deletedot == true)
            {
                Destroy(dot);
            }
            deletedot = deletedot == true ? false : true;
        }

        walls = GameObject.FindGameObjectsWithTag("mazewall");
        foreach (GameObject wall in walls)
        {
            Debug.Log(wall.transform.position);
        }



        var ghost1 = (GameObject)Instantiate(ghostPrefab, new Vector3(18f, 0, 18f), transform.rotation);
        var ghost2 = (GameObject)Instantiate(ghostPrefab, new Vector3(18f, 0, -18f), transform.rotation);
        var ghost3 = (GameObject)Instantiate(ghostPrefab, new Vector3(-18f, 0, 18f), transform.rotation);
        var ghost4 = (GameObject)Instantiate(ghostPrefab, new Vector3(-18f, 0, -18f), transform.rotation);

    }

    private IEnumerator StartBox()
    {
        txtCenter.text = "Press any Key to Begin";
        while (!Input.anyKey)
        {
            yield return null;
        }
        txtHelp.text = "";
        txtCenter.text = "";
        background.Play();
        Time.timeScale = 1;
    }

    void Update () {

        if (GameController.instance.gameOver) {
            PlayerDead();
        }

        if (gameOver && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }


        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayerDead();
            UnityEditor.EditorApplication.isPlaying = false;  //hide for build
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Pause))
        {
            volume = Time.timeScale == 1 ? AudioListener.volume : volume;
            Time.timeScale = Time.timeScale == 1 ? 0 : 1;
            spawnGhost = spawnGhost == true ? false : true;
            AudioListener.volume = Time.timeScale == 0 ? 0f : volume; 
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            AudioListener.volume = AudioListener.volume * .9f;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            AudioListener.volume = AudioListener.volume * 1.1f;
        }

        if (Input.GetKeyDown(KeyCode.F1) || Input.GetKeyDown(KeyCode.H))
        {
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }


        if (timer < Time.time)
        {
            timer = Time.time + Random.Range(3f, 5f);
            spawnGhost = true;
        }

        //if (timer2 < Time.time)
        //{
        //    timer2 = Time.time + Random.Range(2f, 4f);
        //}

        if (spawnGhost)
        {
            ghostCount = GameObject.FindGameObjectsWithTag("ghost").Length;
            if (ghostCount < 10) {
                xOffset = Random.Range(-20f, 20f);
                zOffset = Random.Range(-20f, 20f);
                var ghostX = (GameObject)Instantiate(ghostPrefab, new Vector3(xOffset, 0, zOffset), transform.rotation);
            }
            spawnGhost = false;
        }

    }

    void OnBecameInvisible()
    {
        Debug.Log("GC Invisibile" + this.tag);
    }

    public void MuteBG()
    {
        background.mute = true;     //doesn't work on it's own in PlayerDead()
    }

    public void PlayerDead()
    {
        StopAllCoroutines();
        //Destroy(mazeInstance.gameObject);

        MuteBG();
        spawnGhost = false;
        gameOver = true;
    }

}
