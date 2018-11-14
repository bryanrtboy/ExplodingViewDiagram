using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineToLabelMaker : MonoBehaviour
{

    public GameObject m_originAnchor;
    public GameObject m_destinationAnchor;
    LineRenderer m_line;



    void Awake()
    {
        m_line = this.GetComponent<LineRenderer>() as LineRenderer;
    }


    // Update is called once per frame
    void Update()
    {
        if (m_originAnchor.activeSelf)
        {
            if (!m_line.enabled)
                m_line.enabled = true;

            m_line.SetPosition(0, m_originAnchor.transform.position);
            m_line.SetPosition(1, m_destinationAnchor.transform.position);
        }
        else
        {
            m_line.enabled = false;
        }

    }
}
