using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLight : MonoBehaviour
{
    public Light PlayerLightSource;
    [Tooltip("Whether the light is disabled in this scene only")] public bool IsDisabled = false; // on by default in all scenes but hub

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsDisabled)
        {
            if (PlayerLightSource.spotAngle != GameManager.Instance.PlayerData.LightFOV)
                PlayerLightSource.spotAngle = GameManager.Instance.PlayerData.LightFOV;

            if (PlayerLightSource.intensity != GameManager.Instance.PlayerData.LightIntensity)
                PlayerLightSource.intensity = GameManager.Instance.PlayerData.LightIntensity;

            if (PlayerLightSource.range != GameManager.Instance.PlayerData.LightRange)
                PlayerLightSource.range = GameManager.Instance.PlayerData.LightRange;
        }
        else
        {
            PlayerLightSource.intensity = 0; // turn off light
        }
    }

}
