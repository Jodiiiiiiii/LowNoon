using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GummyWorm : MonoBehaviour
{
    [SerializeField] private Material _wormMaterial;

    [SerializeField] private Texture _normalTexture;
    [SerializeField] private Texture _gummyTexture;

    void Start()
    {

       // _wormMaterial = GameObject.Find("4_Worm").GetComponent<SkinnedMeshRenderer>().material;
        _wormMaterial.mainTexture = _gummyTexture;
    }

    // Update is called once per frame
    void Update()
    {
        //_wormMaterial = GameObject.Find("4_Worm").GetComponent<SkinnedMeshRenderer>().material;
        if (!GameManager.Instance.IsGummy)
        {
            _wormMaterial.mainTexture = _normalTexture;
        }
        else
        {
            _wormMaterial.mainTexture = _gummyTexture;
        }
       // GameObject.Find("4_Worm").GetComponent<SkinnedMeshRenderer>().material = _wormMaterial;
    }
}
