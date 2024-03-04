using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualCameraController : MonoBehaviour
{
    private CameraController _cameraController;
    void Start()
    {
        _cameraController = GetComponent<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // The camera movement coroutine that all of the bespoke camera movements use
    IEnumerator DoCamPosition(Vector3 targetPos, float travelTime, Vector3 targetRotation)
    {
        //activeCoroutine = true;

        float xAngle;
        float yAngle;
        float zAngle;

        Vector3 velocity = Vector3.zero;    // Initial velocity values for the damping functions
        float xVelocity = 0f;
        float yVelocity = 0f;
        float zVelocity = 0f;

        while (Vector3.Distance(transform.position, targetPos) >= .05f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, travelTime); // Move camera position

            xAngle = Mathf.SmoothDampAngle(transform.eulerAngles.x, targetRotation.x, ref xVelocity, travelTime);
            yAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation.y, ref yVelocity, travelTime);
            zAngle = Mathf.SmoothDampAngle(transform.eulerAngles.z, targetRotation.z, ref zVelocity, travelTime);

            transform.eulerAngles = new Vector3(xAngle, yAngle, zAngle);    // Change camera rotation
            yield return null;
        }
        //activeCoroutine = false;
        yield return null;
    }
}
