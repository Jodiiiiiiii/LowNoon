using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private Slider _masterVolumeSlider;
    [SerializeField] private Slider _playerVolumeSlider;
    [SerializeField] private Slider _enemyVolumeSlider;
    [SerializeField] private Slider _enviromentVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private GameObject _mainView;

    // Start is called before the first frame update
    void Start()
    {
        
        _masterVolumeSlider.value = GameManager.Instance.SaveData.MasterVolume * 100;
        _playerVolumeSlider.value = GameManager.Instance.SaveData.PlayerVolume * 100;
        _enemyVolumeSlider.value = GameManager.Instance.SaveData.EnemyVolume * 100;
        _enviromentVolumeSlider.value = GameManager.Instance.SaveData.EnvironmentVolume * 100;
        _musicVolumeSlider.value = GameManager.Instance.SaveData.MusicVolume * 100;
    }

    // Update is called once per frame
    void Update()
    {
        GameManager.Instance.SaveData.MasterVolume = _masterVolumeSlider.value / 100;
        GameManager.Instance.SaveData.PlayerVolume = _playerVolumeSlider.value / 100;
        GameManager.Instance.SaveData.EnemyVolume = _enemyVolumeSlider.value / 100;
        GameManager.Instance.SaveData.EnvironmentVolume = _enviromentVolumeSlider.value / 100;
        GameManager.Instance.SaveData.MusicVolume = _musicVolumeSlider.value / 100;
    }


    public void BackButton()
    {
        _mainView.SetActive(true);
        gameObject.SetActive(false);
    }

}
