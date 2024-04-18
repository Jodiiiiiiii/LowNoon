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
            // TODO: Do a game over thing
        }

        _image.fillAmount = _time / _timer;
        _image.color = Color.Lerp(_startColor, _endColor, _time / _timer);
    }
}
