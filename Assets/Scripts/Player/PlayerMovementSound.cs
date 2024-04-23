using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementSound : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _movementSound;
    private float _repeatDuration = 1.25f;
    CharacterState _prevState;
    float _startVolume;

    private float _timer;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        _timer = 0f;
        _prevState = _playerController.State;
        _startVolume = _audioSource.volume;
    }

    // Update is called once per frame
    void Update()
    {
        // make it so sound starts instantly upon starting moving
        if (_prevState != CharacterState.MOVING && _playerController.State == CharacterState.MOVING)
        {
            _timer = 0;

            // ensure it plays at proper max volume regardless of previous fade out
            StopAllCoroutines();
            _audioSource.volume = _startVolume;
        }

        if (_playerController.State == CharacterState.MOVING)
        {
            if (_timer <= 0f)
            {
                _audioSource.PlayOneShot(_movementSound, 0.1f * GameManager.Instance.GetPlayerVolume());
                _timer = _repeatDuration;
            }
            _timer -= Time.deltaTime;
        }
        else
            StartCoroutine(FadeOut(_audioSource, 0.5f)); // cuts off sound when no longer moving

        // update prev state
        _prevState = _playerController.State;
    }

    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        // return to initial volume
        audioSource.volume = startVolume;
    }
}
