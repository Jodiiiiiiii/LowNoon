using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneCharacterController : MonoBehaviour
{
    private Animator _animator;

    private float _threshold = .03f;
    
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
        StopAllCoroutines();
        StartCoroutine(DoGradualPosition(pos, speed, rot));
    }

    public void SetCurrentAnimation(string name)
    {
        _animator = GetComponent<Animator>();
        if(gameObject.tag == "Player" && name == "Move")
        {
            _animator.SetBool("isMoving", !_animator.GetBool("isMoving"));
        }
        else
        {
            _animator.Play(name, 0, 0);
        }
        
    }

    IEnumerator DoGradualPosition(Vector3 targetPos, float travelTime, Vector3 targetRotation)
    {
        float xAngle;
        float yAngle;
        float zAngle;

        Vector3 velocity = Vector3.zero;    // Initial velocity values for the damping functions
        float xVelocity = 0f;
        float yVelocity = 0f;
        float zVelocity = 0f;

        while (Vector3.Distance(transform.position, targetPos) >= _threshold)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, travelTime); // Move position

            xAngle = Mathf.SmoothDampAngle(transform.eulerAngles.x, targetRotation.x, ref xVelocity, travelTime);
            yAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation.y, ref yVelocity, travelTime);
            zAngle = Mathf.SmoothDampAngle(transform.eulerAngles.z, targetRotation.z, ref zVelocity, travelTime);

            transform.eulerAngles = new Vector3(xAngle, yAngle, zAngle);    // Change rotation
            yield return null;
        }
        transform.position = targetPos; // snap to goal value
        yield return null;
    }
}
