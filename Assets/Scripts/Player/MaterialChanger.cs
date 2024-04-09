using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChanger : MonoBehaviour
{
    // Start is called before the first frame update
    private Material _wormMaterial;
    [SerializeField] private Texture _normalTexture;
    [SerializeField] private Texture _gummyTexture;

    private void Start()
    {
        _wormMaterial = GameObject.Find("4_Worm").GetComponent<SkinnedMeshRenderer>().material;
        if(_wormMaterial.mainTexture.name == "WormGummy")
        {
            GameManager.IsGummy = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.IsGummy)
            {
                _wormMaterial.mainTexture = _normalTexture;
                GameManager.IsGummy = false;
            }
            else
            {
                _wormMaterial.mainTexture = _gummyTexture;
                GameManager.IsGummy = true;
            }
        } 
    }
}
