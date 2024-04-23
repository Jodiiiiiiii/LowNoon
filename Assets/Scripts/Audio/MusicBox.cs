using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicBox : MonoBehaviour
{
    private static MusicBox instance;
    private AudioSource _audioSource;
    private bool silent;
    private bool activeCoroutine = false;

    [SerializeField] private AudioClip _fightingMusic;
    [SerializeField] private AudioClip _creditsMusic;
    private AudioClip _currentMusic;

    private float _localVolume = 1;

    public static MusicBox Instance
    {
        get
        {
            return instance;
        }
    }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);

    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

    }

    private void OnEnable()
    {
        GameManager.onSceneBegin += newSceneCheck;
        GameManager.onHubRevive += newSceneCheck;
    }

    private void OnDisable()
    {
        GameManager.onSceneBegin -= newSceneCheck;
        GameManager.onHubRevive -= newSceneCheck;
    }

    private void Update()
    {
        if (!activeCoroutine)
        {
            if (!silent)
            {
                _audioSource.volume = GameManager.Instance.GetMusicVolume() * _localVolume;
                if (GameManager.Instance.PlayerData.CurrHealth <= 0)
                {
                    StartCoroutine(DoFadeOut());
                }
            }
            else
            {

                _audioSource.volume = 0f;
            }
        }

    }

    public void newSceneCheck()
    {
        _audioSource = GetComponent<AudioSource>();
        activeCoroutine = false;
        if (wrongSceneCheck())
        {
            Debug.Log("Shut up");
            silent = true;
            _localVolume = 0f;
            if (_audioSource.isPlaying)
                _audioSource.Stop();
        }
        else
        {
            if(!(SceneManager.GetActiveScene().name == "xx_Credits"))
            {
                _audioSource.loop = true;
                silent = false;
                _audioSource.clip = _fightingMusic;
                _audioSource.volume = GameManager.Instance.GetMusicVolume() * _localVolume;
                if (!_audioSource.isPlaying)
                    _audioSource.Play();

            }

            if (SceneManager.GetActiveScene().name == "x_FinalBoss")
            {
                FadeOut();
            }


        }
    }

    public void FadeOut()
    {
        StopAllCoroutines();
        StartCoroutine(DoFadeOut());
    }

    public void RollCredits()
    {
        _currentMusic = _creditsMusic;
        _audioSource.clip = _currentMusic;
        _localVolume = 1;
        _audioSource.volume = GameManager.Instance.GetMusicVolume() * _localVolume;
        silent = false;
        _audioSource.loop = false;
        _audioSource.Play();
    }

    public void HardStop()
    {
        silent = true;
    }

    private static bool wrongSceneCheck()
    {
        if (SceneManager.GetActiveScene().name == "0_Hub")
        {
            return true;
        }
        return false;
    }

    private IEnumerator DoFadeOut()
    {
        activeCoroutine = true;
        while (_localVolume > 0f)
        {
            _localVolume -= Time.deltaTime * 2f;
            yield return null;
        }
        _audioSource.Stop();
        silent = true;
    }
}
