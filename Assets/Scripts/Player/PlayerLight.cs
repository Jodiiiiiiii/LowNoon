using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLight : MonoBehaviour
{
    public Light PlayerLightSource;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerLightSource.spotAngle != GameManager.Instance.PlayerData.LightFOV)
        {
           PlayerLightSource.spotAngle = GameManager.Instance.PlayerData.LightFOV;
        }

        if (PlayerLightSource.intensity != GameManager.Instance.PlayerData.LightIntensity)
        {
            PlayerLightSource.intensity = GameManager.Instance.PlayerData.LightIntensity;
        }


    }

}
