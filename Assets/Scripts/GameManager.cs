using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;
    private GroundPieceController[] allGroundPieces;
    // Start is called before the first frame update
    void Start()
    {
        SetupNewLevel();
    }
    private void SetupNewLevel()
    {
        allGroundPieces = FindObjectsOfType<GroundPieceController>();
    }
    private void Awake()
    {
        if(singleton == null)
        {
            singleton = this;
        }
        else if(singleton != this)
        {
            Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }
    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        SetupNewLevel();
    }

    public void CheckComplete()
    {
        bool isFinished = true;
        for(int i = 0;i < allGroundPieces.Length; i++)
        {
            if (!allGroundPieces[i].isColored) isFinished = false;
        }
        if (isFinished) NextLevel();
    }

    private void NextLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex == 2)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
