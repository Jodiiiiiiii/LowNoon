using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RunTracker : MonoBehaviour
{
    private TextMeshProUGUI _text;

    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();

        // only needs to be updated on loading the scene
        _text.text = "Attempts\n" + GameManager.Instance.SaveData.NumOfRuns;
    }
}
