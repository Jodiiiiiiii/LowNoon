using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChanger : MonoBehaviour
{
    
    [SerializeField] private Texture _normalTexture;
    [SerializeField] private Texture _gummyTexture;

    // Start is called before the first frame update
    private void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            if (GameManager.Instance.IsGummy)
            {
                GameManager.Instance.IsGummy = false;
            }
            else
            {
                GameManager.Instance.IsGummy = true;
            }
        } 
    }
}
