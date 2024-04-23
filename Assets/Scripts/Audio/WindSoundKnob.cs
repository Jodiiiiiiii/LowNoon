using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class exists to adjust the volume of the desert ambiance based on how close the worm is to the saloon.

public class WindSoundKnob : MonoBehaviour
{
    [SerializeField, Tooltip("threshold distance where if player is this distance or closer to the saloon, volume = 0")] private float _tooCloseDistance = 10f;
    [SerializeField, Tooltip("distance from saloon where volume = 1 (max)")] private float _maxVolumeDistance = 50f;

    [SerializeField] private Transform _saloonSoundSource;

    private Transform _player;

    private AudioSource _soundSource;
    private MusicController _musicController;
    void Start()
    {
        _player = GameObject.Find("WormFinal2").GetComponent<Transform>();
        _soundSource = GetComponent<AudioSource>();
        _musicController = GetComponent<MusicController>();
    }

    void Update()
    {
        if (_musicController.IsOn)
        {
            if (Vector3.Distance(_saloonSoundSource.position, _player.position) < _tooCloseDistance)
            {
                _soundSource.volume = 0;
            }
            else if (Vector3.Distance(_saloonSoundSource.position, _player.position) > _maxVolumeDistance)
            {
                _soundSource.volume = 1 * GameManager.Instance.GetEnvironmentVolume();
            }
            else
            {
                _soundSource.volume = ((Vector3.Distance(_saloonSoundSource.position, _player.position) - _tooCloseDistance) / _maxVolumeDistance) * GameManager.Instance.GetEnvironmentVolume();
            }
        }
        
    }
}
