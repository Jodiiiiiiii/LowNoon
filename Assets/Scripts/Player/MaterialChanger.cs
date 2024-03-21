using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChanger : MonoBehaviour
{
    // Start is called before the first frame update
    private Material _wormMaterial;
    [SerializeField] private Texture _normalTexture;
    [SerializeField] private Texture _gummyTexture;

    private bool _materialChanged = false;

    private void Start()
    {
        _wormMaterial = GameObject.Find("4_Worm").GetComponent<SkinnedMeshRenderer>().material;
        if(_wormMaterial.mainTexture.name == "WormGummy")
        {
            _materialChanged = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_materialChanged)
                _wormMaterial.mainTexture = _normalTexture;
            else
                _wormMaterial.mainTexture = _gummyTexture;

            _materialChanged = !_materialChanged;
        } 
    }
}
