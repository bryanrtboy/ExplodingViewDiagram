using UnityEngine;
using System.Collections;

public class CamOrbiter : MonoBehaviour
{
    /* 
	This Orbiting camera works with
	polor coordinates.
	
*/



    public Transform cameraTargetTransform;
    [Tooltip("How far from the target should the camera be at the start of the level")]
    public float startingRadius = 20;
    [Tooltip("After start, how far away should the camera be from the target")]
    public float desiredRadiusFromTarget = 10;
    public float dragRotateSpeed = 6;
    [Tooltip("When the user lifts the mouse, how fast until the camera before the camera is resting again")]
    public float restRotateSpeed = 2;
    [Tooltip("Do you want to copy the orientation of the camera at startup")]
    public bool useCameraRotationAtStart = true;


    float desiredBeta = 15;
    float desiredAlpha = 15;
    bool isDraggingView = false;
    float alpha = 15;
    float beta = 15;

    // Use this for initialization
    void Awake()
    {
        if (useCameraRotationAtStart)
        {
            desiredAlpha = transform.localEulerAngles.y;
            desiredBeta = transform.localEulerAngles.x;
            alpha = transform.localEulerAngles.y;
            beta = transform.localEulerAngles.x;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            isDraggingView = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isDraggingView = false;
        }


        // ORBIT CONTROL
        float h;
        float v;

        if (isDraggingView) // not iOS device
        {
            h = (5000 / Screen.width) * Input.GetAxis("Mouse X");
            v = (5000 / Screen.width) * Input.GetAxis("Mouse Y");

            desiredAlpha += h;
            desiredBeta -= v;

            desiredBeta = Mathf.Clamp(desiredBeta, -89, 89);
        }

    }

    void FixedUpdate()
    {
        // LERP BETWEEN ACTUAL AND DESIRED VALUES

        // 1. Go to target, then back away
        transform.position = cameraTargetTransform.position;

        float lerpFactorRotate = restRotateSpeed;
        // 3. Reorient the camera to the axis of the orbit
        if (isDraggingView)
        {
            lerpFactorRotate = dragRotateSpeed;
        }
        transform.rotation = Quaternion.identity;
        alpha = Mathf.Lerp(alpha, desiredAlpha, lerpFactorRotate * Time.deltaTime);
        beta = Mathf.Lerp(beta, desiredBeta, lerpFactorRotate * Time.deltaTime);
        transform.Rotate(new Vector3(beta, alpha, 0));


        startingRadius = Mathf.Lerp(startingRadius, desiredRadiusFromTarget, Time.deltaTime);
        var distance = startingRadius;
        transform.position -= transform.rotation * Vector3.forward * distance;


    }

    public void Zoom(float radius)
    {
        desiredRadiusFromTarget = radius;
    }
}
