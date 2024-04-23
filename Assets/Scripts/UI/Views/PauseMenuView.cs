using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuView : View
{
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _settings;

    [SerializeField] private Animator _sceneTransitionAnimator; // Animator for our scene transition element
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
        StartCoroutine(DoLeaveScene());
    }

    public void Settings()
    {
        _pauseMenu.SetActive(false);
        _settings.SetActive(true);
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

    private IEnumerator DoLeaveScene()
    {
        _sceneTransitionAnimator.Play("StandardExit", 0, 0);
        GameObject.Find("Player Camera").GetComponent<CameraController>().enabled = false;
        Time.timeScale = 1f;
        yield return new WaitForSeconds(2f);
        GameManager.IsPaused = false;
        SceneManager.LoadScene("0_Hub");
        //Debug.Log("Loading to hub");
        yield return null;
    }
}
