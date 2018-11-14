using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleLabel : MonoBehaviour
{
    public Transform m_label;

    public void ActivateLabel()
    {
        if (m_label != null)
        {
            m_label.gameObject.SetActive(!m_label.gameObject.activeSelf)
;
        }
    }
}
