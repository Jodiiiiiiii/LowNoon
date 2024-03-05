using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    private AudioSource _music;
    [SerializeField] private float _fadeRate = 3;
    // Start is called before the first frame update

    public bool IsOn;
    void Start()
    {
        _music = GetComponent<AudioSource>();
        if(_music.volume == 0)
        {
            IsOn = false;
        }
        else
        {
            IsOn = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeIn()
    {
        StartCoroutine(DoFadeIn());
    }

    public void FadeOut()
    {
        StartCoroutine(DoFadeOut());
    }

    IEnumerator DoFadeIn()
    {
        while(_music.volume < 1)
        {
            _music.volume += Time.deltaTime * _fadeRate;
            yield return null;
        }
        IsOn = true;
    }

    IEnumerator DoFadeOut()
    {
        while (_music.volume > 0)
        {
            _music.volume -= Time.deltaTime * _fadeRate;
            yield return null;
        }
        IsOn = false;
    }
}
