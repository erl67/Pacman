using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    public static GameController instance;
    public bool gameOver;
    public bool spawn = true, spawnGhost = false;

    public GameObject ghostPrefab;

    private AudioSource background;

    private float volume, timer, timer2;
    private float xOffset = 0f, yOffset = 0f, zOffset = 0f;


    public Maze mazePrefab;

    private Maze mazeInstance;


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
        BeginGame();
    }

    private void BeginGame()
    {
        Debug.Log("Beginning Game");
        mazeInstance = Instantiate(mazePrefab) as Maze;
        StartCoroutine(mazeInstance.Generate());

    }

    void Update () {

        if (GameController.instance.gameOver) {
            PlayerDead();
        }

        if (gameOver && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (!spawn && Input.GetKeyDown(KeyCode.N))
        {
            PlayerDead();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayerDead();
            //UnityEditor.EditorApplication.isPlaying = false;  //hide for build
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Pause))
        {
            volume = Time.timeScale == 1 ? AudioListener.volume : volume;
            Time.timeScale = Time.timeScale == 1 ? 0 : 1;
            spawn = spawn == true ? false : true;
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
            //Time.timeScale = 1;
        }

        if (Input.GetKeyDown(KeyCode.Home))
        {
            SceneManager.LoadScene(0);
        }

        if (timer < Time.time)
        {
            timer = Time.time + Random.Range(1f, 3f);
            spawnGhost = true;
        }

        if (timer2 < Time.time)
        {
            timer2 = Time.time + Random.Range(2f, 4f);
        }
        
        if (spawn && spawnGhost)
        {
            xOffset = Random.Range(5f, -5f);
            zOffset = Random.Range(5f, -5f);
            //yOffset = Random.Range(5f, -5f);

            Vector3 spawnOffset = new Vector3(xOffset, 0f, zOffset);
            //var enemy = (GameObject)Instantiate(ghostPrefab, transform.position + spawnOffset, transform.rotation);
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
        Destroy(mazeInstance.gameObject);

        MuteBG();
        spawn = false;
        gameOver = true;
    }

}
