using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneCharacterController : MonoBehaviour
{
    private Animator _animator;
    
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetToPosition(Vector3 pos)
    {
        this.transform.position = pos;  
    }


    public void SetToRotation(Quaternion rot)
    {
        this.transform.rotation = rot;
    }

    public void TravelToPositionAndRotation(Vector3 pos, Vector3 rot, float speed)
    {

    }

    public void SetCurrentAnimation(string name)
    {
        _animator = GetComponent<Animator>();
        _animator.Play(name, 0, 0);
    }
}
