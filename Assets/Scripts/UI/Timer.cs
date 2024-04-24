using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [Header("Timer")]
    [SerializeField] private float _timer;
    [SerializeField] private GameObject _timerObj;

    [Header("Color")]
    [SerializeField, Tooltip("color at start of room")] private Color _startColor;
    [SerializeField, Tooltip("color at end of timer")] private Color _endColor;
    private UnityEngine.UI.Image _image;

    [Header("Audio")]
    [SerializeField, Tooltip("sound played when timer runs out - same as tremors")] private AudioClip _tremorSound;

    private float _time;
    // Start is called before the first frame update
    void Start()
    {
        _time = 0;
        _image = _timerObj.GetComponent<UnityEngine.UI.Image>();
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;

        if ( _time >= _timer)
        {
            // player dies
            ViewManager.Show<GameOverView>(false);
            GameManager.Instance.PlayerData.CrumblingDeath = true; // ensure fade to black in game over screen
            GameObject.Find("Player").GetComponent<PlayerController>().enabled = false; // ensure player loses control

            GameObject cam = GameObject.Find("ActualCamera");
            // visual
            cam.GetComponent<Animator>().SetTrigger("Shake");
            // sfx
            cam.GetComponent<AudioSource>().PlayOneShot(_tremorSound, GameManager.Instance.GetEnvironmentVolume());
        }

        _image.fillAmount = _time / _timer;
        _image.color = Color.Lerp(_startColor, _endColor, _time / _timer);
    }
}
