using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReticleController : MonoBehaviour
{
    [Header("Main Reticle")]
    [SerializeField, Tooltip("Amount of horizontal offset when worm is not aligned with camera")] private float _maxHorizontalOffset = 10f;
    [SerializeField, Tooltip("Distance from centered at which the reticle becomes completely transparent")] private float _maxFadeDistance = 750f;
    [SerializeField, Tooltip("Alpha value of reticle when perfectly centered (in range 0 to 1)")] private float _maxAlpha = 0.75f;
    [SerializeField, Tooltip("'snappiness' of color fading in and out")] private float _transparencySharpness = 10f;

    [Header("Reticle Outline")]
    [SerializeField, Tooltip("Needed to toggle display in/out of stationary mode")] private Image _reticleOutline;
    [SerializeField, Tooltip("Alpha value of outline when in shooting mode")] private float _maxOutlineAlpha = 0.4f;

    private RectTransform _rect;
    private Image _image;
    private PlayerShooting _shootingInfo;
    private PlayerController _playerController;

    // Start is called before the first frame update
    void Start()
    {
        _rect = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
        _shootingInfo = GameObject.Find("Player").GetComponent<PlayerShooting>();
        _playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        // update horizontal offset position
        Vector2 newPos = _rect.anchoredPosition;
        newPos.x = Map(_shootingInfo.PlayerAngleOffset, -180, 180, -_maxHorizontalOffset, _maxHorizontalOffset);
        _rect.anchoredPosition = newPos;

        // update transparency
        float goalAlpha;
        if (_playerController.State == CharacterState.STATIONARY)
        {
            goalAlpha = Map(Mathf.Abs(newPos.x), 0, _maxFadeDistance, _maxAlpha, 0);

            // smoothly lerp outline
            Color outlineColor = _image.color;
            outlineColor.a = _maxOutlineAlpha;
            _reticleOutline.color = Color.Lerp(_reticleOutline.color, outlineColor, 1f - Mathf.Exp(-_transparencySharpness * Time.deltaTime));
        }
        else // not stationary
        {
            goalAlpha = 0; // no reticle unless in shooting mode

            // smoothly lerp outline
            _reticleOutline.color = Color.Lerp(_reticleOutline.color, Color.clear, 1f - Mathf.Exp(-_transparencySharpness * Time.deltaTime));
        }

        // smoothly lerp to goal transparency
        Color newColor = _image.color;
        newColor.a = goalAlpha;
        _image.color = Color.Lerp(_image.color, newColor, 1f - Mathf.Exp(-_transparencySharpness * Time.deltaTime));
    }

    // map s from range [a1, a2] to [b1, b2]
    private float Map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }
}
