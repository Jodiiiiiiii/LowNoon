using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private int _prevUIHealth;          // player's health level displayed on UI in previous frame
    private int _prevUIArmor;           // amount of current armor displayed on UI in previous frame

    [SerializeField] private Animator _sceneTransitionAnimator; // Animator for our scene transition element


    public override void Initialize()
    {
        _hpUnits = new List<GameObject>();
        _stats = GameManager.Instance.PlayerData;
        _playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();

        // create starting health HP units
        for (int i = 0; i < _stats.MaxHealth; i++)
            _hpUnits.Add(Instantiate(_hpUnit, _hpParent));
        UpdateHealthUI();
        _prevUIHealth = _stats.CurrHealth; // start at max for prev value
        _prevUIArmor = _stats.Armor;

        _leftBookend.transform.position = new Vector2(_hpUnits[0].GetComponent<RectTransform>().position.x - 70, _hpUnits[0].GetComponent<RectTransform>().position.y - 45);
        _rightBookend.transform.position = new Vector2(_hpUnits[_hpUnits.Count - 1].GetComponent<RectTransform>().position.x + 90, _hpUnits[_hpUnits.Count - 1].GetComponent<RectTransform>().position.y - 45);       
    }

    private void OnEnable()
    {
        SceneTransitionObject.onSceneTransition += LeavingSceneTransition;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        SceneTransitionObject.onSceneTransition -= LeavingSceneTransition;
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

        // update HPUnits (if necessary)
        if (_prevUIHealth != _stats.CurrHealth || _prevUIArmor != _stats.Armor)
            UpdateHealthUI();
        _prevUIHealth = _stats.CurrHealth; // update HP amount for next frame
        _prevUIArmor = _stats.Armor; // update Armor amount for next frame

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ViewManager.Show<PauseMenuView>(true);
        }
    }

    public void MaxHPUp()
    {
        GameObject unit = Instantiate(_hpUnit, _hpParent);
        unit.GetComponent<HPUnit>().setHPState(true);
        unit.GetComponent<HPUnit>().setArmorState(false);
        _hpUnits.Add(unit);
    }

    public void CallItemCard(int type) // For activating the card popup sequence
    {
        StartCoroutine(DoItemCard(type));
    }

    /// <summary>
    /// sets HP units in UI to active or inactive based on current health
    /// </summary>
    private void UpdateHealthUI()
    {
        int hpCount = _stats.CurrHealth;
        int arCount = _stats.Armor;
        for (int i = 0; i < _stats.MaxHealth; i++)
        {
            HPUnit currUnit = _hpUnits[i].GetComponent<HPUnit>();
            // set HP state
            if (hpCount > 0)
            {
                currUnit.setHPState(true);
                hpCount--;
            }
            else
                currUnit.setHPState(false);

            // set Armor state
            if (arCount > 0)
            {
                currUnit.setArmorState(true);
                arCount--;
            }
            else
                currUnit.setArmorState(false);
        }
    }

    private void LeavingSceneTransition()
    {
        _sceneTransitionAnimator.Play("StandardExit", 0, 0);
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
