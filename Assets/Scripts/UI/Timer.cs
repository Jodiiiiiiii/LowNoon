using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Microsoft.Unity.VisualStudio.Editor;

public class Timer : MonoBehaviour
{
    [SerializeField] private float _timer;
    [SerializeField] private GameObject _timerObj;
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
            // Do a game over thing
        }

        _image.fillAmount = _time / _timer;

    }
}
