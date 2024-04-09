using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMove : MonoBehaviour
{
    public GameObject Player;
    public float Direction = 1.0f;
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        StartCoroutine(DoUpgradeMove());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator DoUpgradeMove(){//}, Vector3 direction){
        float goalPosition = 4.5f;
        float increment = 0.05f;
        float counter = 0.0f;

        while(counter <= goalPosition){
            transform.position += ((increment*Direction)*Player.transform.right);
            counter += increment;
            yield return null;
        }

        yield return null;
        
    }
}