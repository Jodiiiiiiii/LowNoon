using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsView : View
{
    // Start is called before the first frame update
    [Header("The picture cards")]
    [SerializeField] private GameObject _fb1;
    [SerializeField] private GameObject _fb2;
    [SerializeField] private GameObject _fb3;
    [SerializeField] private GameObject _fb4;
    [Header("When each card is enabled")]
    [SerializeField] private float _fb1Time;
    [SerializeField] private float _fb2Time;
    [SerializeField] private float _fb3Time;
    [SerializeField] private float _fb4Time;
    [SerializeField] private float _closingTime;

    private float _timer;

    public override void Initialize()
    {
        throw new System.NotImplementedException();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > _fb1Time)
            _fb1.SetActive(true);
        if (_timer > _fb2Time)
            _fb2.SetActive(true);
        if (_timer > _fb3Time)
            _fb3.SetActive(true);
        if (_timer > _fb4Time)
            _fb4.SetActive(true);
        if (_timer > _closingTime)
        {
            // Go back to the main menu
        }
    }
}
