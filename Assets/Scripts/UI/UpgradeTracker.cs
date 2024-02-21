using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeTracker : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _mainNum; // The text saying how many of an upgrade that we have
    [SerializeField] TextMeshProUGUI _backNum;
    [SerializeField] Image _img;
    private int _upgradeCount;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_upgradeCount > 0)
        {
            _img.gameObject.SetActive(true);
            _mainNum.text = "x" + _upgradeCount.ToString();
            _backNum.text = _mainNum.text;
        }
        else
        {
            _img.gameObject.SetActive(false);
            _mainNum.text = "";
            _backNum.text = _mainNum.text;
        }
       
    }

    public void setUpgradeCount(int count)
    {
        _upgradeCount = count;
    }
}
