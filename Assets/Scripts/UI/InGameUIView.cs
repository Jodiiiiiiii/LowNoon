using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUIView : View
{
    // Variables needed

    [SerializeField] GameObject _hpUnit;    // Prefab for HP Unit
    [SerializeField] GameObject _leftBookend, _rightBookend;    // The silly worm bookends
    [SerializeField] Transform _hpParent;
    List<GameObject> _hpUnits;                          // A list of HP Units
    [SerializeField] List<UpgradeTracker> _upgrades;    // A list of the upgrade stuff
    GameManager.Stats _stats;
    PlayerStats _playerStats;


    public override void Initialize()
    {
        _hpUnits = new List<GameObject>();
        _stats = GameManager.Instance.PlayerData;
        _playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        int fullCount = _stats.CurrHealth;
        int arCount = _stats.Armor;
        for (int i = 0; i < _stats.MaxHealth; i++)
        {
            GameObject unit = Instantiate(_hpUnit, _hpParent);
            if(fullCount > 0)
            {
                unit.GetComponent<HPUnit>().toggleEmptyFull();
                fullCount--;
            }
            if (arCount > 0)
            {
                unit.GetComponent<HPUnit>().toggleFullArmor();
                arCount--;
            }
            _hpUnits.Add(unit);
        }
        
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _leftBookend.transform.position = new Vector2(_hpUnits[0].GetComponent<RectTransform>().position.x - 70, _hpUnits[0].GetComponent<RectTransform>().position.y - 45);
        _rightBookend.transform.position = new Vector2(_hpUnits[_hpUnits.Count - 1].GetComponent<RectTransform>().position.x + 90, _hpUnits[_hpUnits.Count - 1].GetComponent<RectTransform>().position.y - 45);
        _upgrades[0].setUpgradeCount(_playerStats.getDamageUpgradeCount());
        _upgrades[1].setUpgradeCount(_stats.FireRateUpgradeCount);
        _upgrades[2].setUpgradeCount(_playerStats.getMoveSpdUpgradeCount());
        _upgrades[3].setUpgradeCount(_stats.LightUpgradeCount);
    }

    public void MaxHPUp()
    {
        //if (prevStats.MaxHealth < stats.MaxHealth)
        //{
            //for (int i = 0; i < stats.MaxHealth - prevStats.MaxHealth; i++)
           // {
                GameObject unit = Instantiate(_hpUnit, _hpParent);
                unit.GetComponent<HPUnit>().toggleEmptyFull();
                _hpUnits.Add(unit);
           // }
        //}
    }

    public void CurrHPUp()
    {
        // If we gained HP, find the FIRST empty container and make it full
        for (int i = 0; i < _hpUnits.Count; i++)
        {
            if (_hpUnits[i].GetComponent<HPUnit>().getEmpty())
            {
                _hpUnits[i].GetComponent<HPUnit>().toggleEmptyFull();
                break;
            }
        }
    }

    public void CurrHPDown()
    {
            // If we lost HP, find the LAST full container and make it empty
            for (int i = _hpUnits.Count - 1; i >= 0; i--)
            {
                if (_hpUnits[i].GetComponent<HPUnit>().getFull())
                {
                    _hpUnits[i].GetComponent<HPUnit>().toggleEmptyFull();
                    break;
                }
            }
    }

    public void ArmorUp()
    {
        // If we gained armor, find the FIRST full container and make it armored
        for (int i = 0; i < _hpUnits.Count; i++)
        {
            if (_hpUnits[i].GetComponent<HPUnit>().getFull() && !_hpUnits[i].GetComponent<HPUnit>().getArmor())
            {
                _hpUnits[i].GetComponent<HPUnit>().toggleFullArmor();
                break;
            }
        }
    }

    public void ArmorDown()
    {
        // If we lost armor, find the LAST armored container and de-armor it
        for (int i = _hpUnits.Count - 1; i >= 0; i--)
        {
            if (_hpUnits[i].GetComponent<HPUnit>().getArmor())
            {
                _hpUnits[i].GetComponent<HPUnit>().toggleFullArmor();
                break;
            }
        }
    }

}
