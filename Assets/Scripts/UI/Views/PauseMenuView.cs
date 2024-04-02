using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuView : View
{
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _settings;
    public override void Initialize()
    {
        _pauseMenu.SetActive(true);
        _settings.SetActive(false);
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackButton();
        }
    }

    public void BackButton()
    {
        _pauseMenu.SetActive(true);
        _settings.SetActive(false);
        ViewManager.ShowLast();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("0_Hub");
    }

    public void WormFacts()
    {
        _pauseMenu.SetActive(false);
        _settings.SetActive(true);
    }

    public void ExitWormFacts()
    {
        _pauseMenu.SetActive(true);
        _settings.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    void OnEnable()
    {
        Time.timeScale = 0;
        GameManager.IsPaused = true;
    }

    void OnDisable()
    {
        Time.timeScale = 1f;
        GameManager.IsPaused = false;
    }
}
