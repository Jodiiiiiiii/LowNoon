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
        //_wormMaterial.mainTexture = _gummyTexture;
        if (_wormMaterial.mainTexture.name == "WormGummy")
        {
            GameManager.Instance.IsGummy = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            if (GameManager.Instance.IsGummy)
            {
                //_wormMaterial.mainTexture = (Texture)Resources.Load("Worm2");
                GameManager.Instance.IsGummy = false;
            }
            else
            {
                //_wormMaterial.mainTexture = (Texture)Resources.Load("WormGummy");
                GameManager.Instance.IsGummy = true;
            }
            //GameObject.Find("4_Worm").GetComponent<SkinnedMeshRenderer>().material = _wormMaterial;
        } 
    }
}
