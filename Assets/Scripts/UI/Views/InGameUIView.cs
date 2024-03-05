using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUIView : View
{
    // Variables needed

    [SerializeField] private GameObject _hpUnit;    // Prefab for HP Unit
    [SerializeField] private GameObject _leftBookend, _rightBookend;    // The silly worm bookends
    [SerializeField] private Transform _hpParent;               // The transform with the Horizontal Layout Group
    private List<GameObject> _hpUnits;                          // A list of HP Units
    [SerializeField] private List<UpgradeTracker> _upgrades;    // A list of the upgrade stuff
    private GameManager.Stats _stats;   // The player's actual stats
    private PlayerStats _playerStats;   // PlayerStats (for accessing upgrade count methods)
    [SerializeField] private ObtainedItemCard card; // The info card that pops up when we get an item
    [SerializeField, Tooltip("How long the card stays active for (includes startup time")] private float cardTime = 3f;
    private int _currUIHealth;          // player's health level displayed on UI


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
        _leftBookend.transform.position = new Vector2(_hpUnits[0].GetComponent<RectTransform>().position.x - 70, _hpUnits[0].GetComponent<RectTransform>().position.y - 45);
        _rightBookend.transform.position = new Vector2(_hpUnits[_hpUnits.Count - 1].GetComponent<RectTransform>().position.x + 90, _hpUnits[_hpUnits.Count - 1].GetComponent<RectTransform>().position.y - 45);

        _currUIHealth = _stats.CurrHealth; // start at max
    }


    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // re-gather new player stats every frame
        _stats = GameManager.Instance.PlayerData;

        _leftBookend.transform.position = new Vector2(_hpUnits[0].GetComponent<RectTransform>().position.x - 70, _hpUnits[0].GetComponent<RectTransform>().position.y - 45);
        _rightBookend.transform.position = new Vector2(_hpUnits[_hpUnits.Count - 1].GetComponent<RectTransform>().position.x + 90, _hpUnits[_hpUnits.Count - 1].GetComponent<RectTransform>().position.y - 45);
        _upgrades[0].setUpgradeCount(_playerStats.getDamageUpgradeCount());
        _upgrades[1].setUpgradeCount(_stats.FireRateUpgradeCount);
        _upgrades[2].setUpgradeCount(_playerStats.getMoveSpdUpgradeCount());
        _upgrades[3].setUpgradeCount(_stats.LightUpgradeCount);
        
        // check for health decrement
        while(_stats.CurrHealth < _currUIHealth)
        {
            CurrHPDown();
            _currUIHealth--;
        }

        // check for health increment
       /* while (_stats.CurrHealth > _currUIHealth)
        {
            CurrHPUp();
            _currUIHealth++;
        }*/
    }

    public void MaxHPUp()
    {
        GameObject unit = Instantiate(_hpUnit, _hpParent);
        unit.GetComponent<HPUnit>().toggleEmptyFull();
        _hpUnits.Add(unit);
        //_hpUnits.Insert(_hpUnits.Count, unit);
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

    public void CallItemCard(int type) // For activating the card popup sequence
    {
        StartCoroutine(DoItemCard(type));
    }

    private IEnumerator DoItemCard(int type)
    {
        if (type == 0)
            card.SetTextAndShow(0, "+ MAX HP", "Worm fact: Worms have 5 hearts. Now you have more.");
        if (type == 1)
            card.SetTextAndShow(1, "+ ARMOR", "Keep that badge on your breast, sheriff.");
        if (type == 2)
            card.SetTextAndShow(2, "+ DAMAGE", "Stuffs your bullets with a little extra lead.");
        if (type == 3)
            card.SetTextAndShow(3, "+ FIRE RATE", "Gives your trigger finger a fierce itch.");
        if (type == 4)
            card.SetTextAndShow(4, "+ MOVE SPEED", "...and his squirming was eccentric.");
        if (type == 5)
            card.SetTextAndShow(5, "+ LIGHT", "Puts some elbow grease in your kerosene.");

        yield return new WaitForSeconds(cardTime);

        card.Hide();
    }

}
