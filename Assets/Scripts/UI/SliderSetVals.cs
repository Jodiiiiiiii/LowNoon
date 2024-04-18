using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderSetVals : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMeshPro;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _textMeshPro.text = (((int)(gameObject.GetComponent<Slider>().value * 100)) / 100f).ToString();
    }
}
