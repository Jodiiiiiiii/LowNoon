using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChange : MonoBehaviour
{
    public GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = gameObject.transform.position;
        player.transform.rotation = gameObject.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
