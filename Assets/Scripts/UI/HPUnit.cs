using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPUnit : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject _emptyHPImg;
    [SerializeField] GameObject _fullHPImg;
    [SerializeField] GameObject _armorHPImg;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    /// set health to full (has health) or empty (no health)
    /// </summary>
    public void setHPState(bool state)
    {
        _fullHPImg.SetActive(state);
    }

    /// <summary>
    /// set armor to present or not-present
    /// </summary>
    public void setArmorState(bool state)
    {
        _armorHPImg.SetActive(state);
    }

    // Getters for determining status of a unit
    public bool getEmpty()
    {
        return _emptyHPImg.activeSelf;
    }

    public bool getFull()
    {
        return _fullHPImg.activeSelf;
    }

    public bool getArmor()
    {
        return _armorHPImg.activeSelf;
    }
}
