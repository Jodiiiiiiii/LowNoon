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

    private CanvasGroup _cGroup;
    [SerializeField] private GameObject _logo;
    public override void Initialize()
    {
        _mainMenu.SetActive(true);
        _creditsMenu.SetActive(false);
        _optionsMenu.SetActive(false);
    }

    private void OnEnable()
    {
        _playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        //_playerController.enabled = false;
        _cameraController = GameObject.Find("Player Camera").GetComponent<CameraController>();
        //_cameraController.enabled = false;
        _manualCameraController = GameObject.Find("Player Camera").GetComponent<ManualCameraController>();
        _cGroup = GetComponent<CanvasGroup>();
        if (!GameManager.Instance.IsMainMenuLoaded)
        {
            _playerController.enabled = false;
            _cameraController.enabled = false;
        }

        StopAllCoroutines();
        StartCoroutine(DoMenuLoad());

        

            //GameManager.onHubRevive += Deactivate;

    }

    private void OnDisable()
    {
        // necessary so that the camera is properly in normal player tracking mode when entering a new scene without main menu
        if (!GameManager.Instance.IsMainMenuLoaded)
        {
            if(_playerController != null)
                _playerController.enabled = true;
            if(_cameraController != null)
                _cameraController.enabled = true;
        }

        //GameManager.onHubRevive -= Deactivate;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (GameManager.Instance.IsMainMenuLoaded)
        {
            Deactivate();
        }
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

    private void Deactivate()
    {
        //_playerController.enabled = true;
        //_cameraController.enabled = true;
        ViewManager.Show<InGameUIView>(false);
    }

    IEnumerator DoStartGame()
    {
        Destroy(_logo);
        GameObject.Find("Ambient Audio").GetComponent<MusicController>().FadeIn();
        GameObject.Find("Title Music Audio").GetComponent<MusicController>().FadeOut();
        _manualCameraController.moveToGameStart();
        _mainMenu.SetActive(false);
        yield return new WaitUntil(() => !_manualCameraController.activeCoroutine);

        
        _playerController.enabled = true;
        _cameraController.enabled = true;
        GameManager.Instance.IsMainMenuLoaded = true;
        ViewManager.Show<InGameUIView>(false);
        yield return null;
    }

    IEnumerator DoMenuLoad()
    {
        yield return new WaitForSeconds(4f);
        _manualCameraController.moveToMainMenu();
        yield return new WaitForSeconds(4f);
        while (_cGroup.alpha < 1)
        {
            _cGroup.alpha += Time.deltaTime;
            yield return null;
        }
        
        
    }
}
