using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraShakeEffect : MonoBehaviour
{
    [Header("Hub")]
    [SerializeField, Tooltip("minimum duration between screen shakes in hub")] private float _minHubInterval = 7f;
    [SerializeField, Tooltip("maximum duration between screen shakes in hub")] private float _maxHubInterval = 15f;

    [Header("Caves")]
    [SerializeField, Tooltip("minimum duration between screen shakes in caves")] private float _minCaveInterval = 20f;
    [SerializeField, Tooltip("maximum duration between screen shakes in caves")] private float _maxCaveInterval = 30f;

    [Header("Other Components")]
    [SerializeField] private CameraController _camController;
    private Animator _camAnimator;

    [Header("Audio")]
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClip;

    private float _timer;

    // Start is called before the first frame update
    void Start()
    {
        _camAnimator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        // start timer initially at max (to prevent really sudden screen shake
        if (SceneManager.GetActiveScene().name == "0_Hub")
            _timer = _maxHubInterval;
        else
            _timer = _maxCaveInterval;
    }

    // Update is called once per frame
    void Update()
    {
        // only handle screen shake timing if not paused or in main menu
        if(_camController.enabled)
        {
            // activate effect and restart timer
            if(_timer < 0)
            {
                // visual
                _camAnimator.SetTrigger("Shake");
                // sfx
                _audioSource.PlayOneShot(_audioClip, GameManager.Instance.GetEnvironmentVolume());

                // restart timer
                if (SceneManager.GetActiveScene().name == "0_Hub")
                    _timer = Random.Range(_minHubInterval, _maxHubInterval);
                else
                    _timer = Random.Range(_minCaveInterval, _maxCaveInterval);
            }

            _timer -= Time.deltaTime;
        }
    }
}
