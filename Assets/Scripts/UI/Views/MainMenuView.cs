using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuView : View
{
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _optionsMenu;
    [SerializeField] private GameObject _creditsMenu;

    private CameraController _cameraController;
    private ManualCameraController _manualCameraController;
    private PlayerController _playerController;
    public override void Initialize()
    {
        _mainMenu.SetActive(true);
        _creditsMenu.SetActive(false);
        _optionsMenu.SetActive(false);

        
        _manualCameraController = GameObject.Find("Player Camera").GetComponent<ManualCameraController>();
    }

    private void OnEnable()
    {
        _playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        _playerController.enabled = false;
        _cameraController = GameObject.Find("Player Camera").GetComponent<CameraController>();
        _cameraController.enabled = false;
    }
    private void OnDisable()
    {
        _playerController.enabled = true;
        _cameraController.enabled = true;
        
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // Public methods for all of the buttons ---
    public void StartButton()
    {
        StartCoroutine(DoStartGame());

    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void OptionsButton()
    {
        _mainMenu.SetActive(false);
        _creditsMenu.SetActive(false);
        _optionsMenu.SetActive(true);
    }

    public void CreditsButton()
    {
        _mainMenu.SetActive(false);
        _creditsMenu.SetActive(true);
        _optionsMenu.SetActive(false);
    }

    public void BackButton()
    {
        _mainMenu.SetActive(true);
        _creditsMenu.SetActive(false);
        _optionsMenu.SetActive(false);
    }

    IEnumerator DoStartGame()
    {
        GameObject.Find("Ambient Audio").GetComponent<MusicController>().FadeIn();
        GameObject.Find("Title Music Audio").GetComponent<MusicController>().FadeOut();
        _playerController.enabled = true;
        _cameraController.enabled = true;
        ViewManager.Show<InGameUIView>(false);
        yield return null;
    }
}
