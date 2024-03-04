using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class exists to adjust the volume of the desert ambiance based on how close the worm is to the saloon.

public class WindSoundKnob : MonoBehaviour
{
    [SerializeField] private Transform _saloonSoundSource;
    private Transform _player;

    private AudioSource _soundSource;
    void Start()
    {
        _player = GameObject.Find("WormFinal1").GetComponent<Transform>();
        _soundSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(Vector3.Distance(_saloonSoundSource.position, _player.position) < 10f)
        {
            _soundSource.volume = 0;
        }
        else if(Vector3.Distance(_saloonSoundSource.position, _player.position) > 50f)
        {
            _soundSource.volume = 1;
        }
        else
        {
            _soundSource.volume = ((Vector3.Distance(_saloonSoundSource.position, _player.position) - 10) / 40f);
        }
    }
}
