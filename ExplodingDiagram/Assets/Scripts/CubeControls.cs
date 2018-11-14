using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CubeControls : MonoBehaviour
{
    public Animator m_animator;
    public float m_animationSpeed = 1f;
    public float m_transitionSpeed = 10f;
    public List<string> m_playedAnimations;


    List<ToggleLabel> m_activeLabels;

    // Use this for initialization
    void Start()
    {
        m_playedAnimations = new List<string>();
        m_activeLabels = new List<ToggleLabel>();

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {
                    Debug.Log("I hit " + hit.transform.name + "!");

                    //PlayAnimation(hit.transform.name);

                    if (m_playedAnimations.Contains(hit.transform.name))
                    {
                        StartCoroutine(LerpAnimation(m_animator.GetLayerIndex(hit.transform.name), 0f));
                        m_playedAnimations.Remove(hit.transform.name);
                    }
                    else
                    {
                        StartCoroutine(LerpAnimation(m_animator.GetLayerIndex(hit.transform.name), 1f));
                        m_playedAnimations.Add(hit.transform.name);
                    }

                    m_animator.speed = m_animationSpeed;

                    //Toggle the label if it exists on the object
                    ToggleLabel label = hit.transform.GetComponent<ToggleLabel>() as ToggleLabel;
                    if (label != null)
                    {
                        label.ActivateLabel();
                        m_activeLabels.Add(label);
                    }
                }
            }
        }
    }

    //This function will immediately change the layer value, so the animation has no transition time
    void PlayAnimation(string layerName)
    {
        if (m_playedAnimations.Contains(layerName))
        {
            m_playedAnimations.Remove(layerName);
            m_animator.SetLayerWeight(m_animator.GetLayerIndex(layerName), 0);
        }
        else
        {
            m_playedAnimations.Add(layerName);
            m_animator.SetLayerWeight(m_animator.GetLayerIndex(layerName), 1);
        }

    }

    //This function gradually transitions the layer to the animation, so it moves smoothly based on the transition speed set in the Inspector
    IEnumerator LerpAnimation(int layerIndex, float target)
    {
        float currentWeight = m_animator.GetLayerWeight(layerIndex);

        while (Mathf.Abs(currentWeight - target) >= .001f)
        {
            currentWeight = Mathf.Lerp(currentWeight, target, Time.deltaTime * m_transitionSpeed);
            m_animator.SetLayerWeight(layerIndex, currentWeight);
            yield return null;

        }
        Debug.Log("All Done with layer " + layerIndex.ToString());
    }

    public void ResetAnimations()
    {
        foreach (string s in m_playedAnimations)
        {
            StartCoroutine(LerpAnimation(m_animator.GetLayerIndex(s), 0f));
        }

        foreach (ToggleLabel l in m_activeLabels)
        {
            l.ActivateLabel();
        }

        m_playedAnimations.Clear();
        m_activeLabels.Clear();
    }

    public void SetAnimationSpeed(float speed)
    {
        m_animator.speed = speed;
    }
}
