using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoEnable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.IsMainMenuLoaded)
        {
            this.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
