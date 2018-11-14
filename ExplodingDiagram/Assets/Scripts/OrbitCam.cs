//Bryan Leister
//Orbit with mouse if left or right alt key is pressed
//Place on your camera, and set your target object in the inspector

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCam : MonoBehaviour
{

    public float m_speed = 8f;
    public float m_distance = 10f;
    public Transform m_target;

    private Vector2 m_input = Vector2.zero;

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.RightAlt) || Input.GetKey(KeyCode.LeftAlt))
        {
            m_input += new Vector2(Input.GetAxis("Mouse X") * m_speed, Input.GetAxis("Mouse Y") * m_speed);

            Quaternion rotation = Quaternion.Euler(m_input.y, m_input.x, 0);
            Vector3 position = m_target.position - (rotation * Vector3.forward * m_distance);

            transform.localRotation = rotation;
            transform.localPosition = position;
        }


    }
}
