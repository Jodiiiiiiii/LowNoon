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

    // Methods for toggling whether an HP unit is empty/full
    public void toggleEmptyFull()
    {
        _fullHPImg.SetActive(!_fullHPImg.activeSelf);
    }

    public void toggleFullArmor()
    {
        _armorHPImg.SetActive(!_armorHPImg.activeSelf);
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
