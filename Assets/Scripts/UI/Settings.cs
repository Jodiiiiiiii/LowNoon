using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private Slider _masterVolumeSlider;
    [SerializeField] private Slider _playerVolumeSlider;
    [SerializeField] private Slider _enemyVolumeSlider;
    [SerializeField] private Slider _environmentVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _horizontalSensSlider;
    [SerializeField] private Slider _verticalSensSlider;
    [SerializeField] private GameObject _mainView;

    // Start is called before the first frame update
    void Start()
    {
        _masterVolumeSlider.value = GameManager.Instance.SaveData.MasterVolumeSlider * 100;
        _playerVolumeSlider.value = GameManager.Instance.SaveData.PlayerVolumeSlider * 100;
        _enemyVolumeSlider.value = GameManager.Instance.SaveData.EnemyVolumeSlider * 100;
        _environmentVolumeSlider.value = GameManager.Instance.SaveData.EnvironmentVolumeSlider * 100;
        _musicVolumeSlider.value = GameManager.Instance.SaveData.MusicVolumeSlider * 100;
        _horizontalSensSlider.value = GameManager.Instance.SaveData.HorizontalSensitivity;
        _verticalSensSlider.value = GameManager.Instance.SaveData.VerticalSensitivity;
    }

    // Update is called once per frame
    void Update()
    {
        GameManager.Instance.SetMasterVolumeSlider(_masterVolumeSlider.value / 100);
        GameManager.Instance.SetPlayerVolumeSlider(_playerVolumeSlider.value / 100);
        GameManager.Instance.SetEnemyVolumeSlider(_enemyVolumeSlider.value / 100);
        GameManager.Instance.SetEnvironmentVolumeSlider(_environmentVolumeSlider.value / 100);
        GameManager.Instance.SetMusicVolumeSlider(_musicVolumeSlider.value / 100);
        GameManager.Instance.SaveData.HorizontalSensitivity = _horizontalSensSlider.value;
        GameManager.Instance.SaveData.VerticalSensitivity = _verticalSensSlider.value;
    }

    public void BackButton()
    {
        _mainView.SetActive(true);
        gameObject.SetActive(false);
    }
}
