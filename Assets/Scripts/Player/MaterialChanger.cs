using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChanger : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Material _wormMaterial;
    [SerializeField] private Texture _normalTexture;
    [SerializeField] private Texture _gummyTexture;

    private bool _materialChanged = false;  

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(_materialChanged)
                _wormMaterial.mainTexture = _normalTexture;
            else
                _wormMaterial.mainTexture = _gummyTexture;

            _materialChanged = !_materialChanged;
        } 
    }
}
