using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class controls the behavior of the Lantern prefab
public class LanternController : MonoBehaviour
{
    public bool IsLit;    // Whether the lantern is lit or not
    [SerializeField] private Light _candleLight;    // The light creating the candle effect
    [SerializeField] private Animator _candleAnim;  // Animator for the candle sprite

    [SerializeField] private float _minIntensity = .3f;   // Intensity range of light
    [SerializeField] private float _maxIntensity = 1.5f;
    [SerializeField] private float _flickerSpeed = .002f;    // How fast the candle flickers
    private bool _isLightRising = true;  // Internal bool to keep track of whether the candle is brightening or dimming

    void Start()
    {
        if (IsLit) // The candle sprite changes depending on whether or not it's lit, as do its emissive properties
        {
            _candleLight.gameObject.SetActive(true);
            _candleAnim.Play("CandleFlicker", 0, 0);
        }
        else
        {
            _candleLight.gameObject.SetActive(false);
            _candleAnim.Play("CandleDoused", 0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Logic for flickering the candlelight
        if (_isLightRising)
            _candleLight.intensity += _flickerSpeed;
        else
            _candleLight.intensity -= _flickerSpeed;

        if(_candleLight.intensity > _maxIntensity)
            _isLightRising = false;
        if (_candleLight.intensity < _minIntensity)
            _isLightRising = true;

    }
}
