using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLight : MonoBehaviour
{
    public Light light;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (light.range != GameManager.Instance.PlayerData.LightFOV)
        {
            light.range = GameManager.Instance.PlayerData.LightFOV;
        }

        if (light.intensity != GameManager.Instance.PlayerData.LightIntensity)
        {
            light.intensity = GameManager.Instance.PlayerData.LightIntensity;
        }


    }

}
