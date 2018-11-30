//Bryan Leister
//Nov. 2018
//
//This scripts should be a Singleton, place only on one game object in your scene. It checks for mouse input and then sends
//messages to activate animations on other objects.
//
//Instructions:
//Drag the gameobject with an Animator onto the m_animatorTransform. The Animator needs to be set up with Layers
//that correspond to the names of the objects the user clicks on. If a user clicks on an object named "Left", the
//script looks for a layer in the Animator named 'Left'. It then transitions that layer from 0 to 1. 
//All of the layers should be at 0 in the Animator on Start.
//All colliders the user hits are sent to the Animator, if a layer does not exist with that name, an error is generated.
//Don't include colliders on things that aren't meant to trigger an animation!
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CubeControls : MonoBehaviour
{
    public Animator m_animatorTransform;
    public float m_animationSpeed = 1f;
    public float m_transitionSpeed = 10f;
    //When we hit a collider, store the collider's name in a list. This is public for viewing/debugging in the Editor
    public List<string> m_playedAnimations;

    //If a hit object has a label, we need to activate it and store a reference to it for de-activating it later...
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

                    //First idea is to simply play the animation whose name matches the collider transform's name. Does not work well..
                    //PlayAnimation(hit.transform.name);


                    //Better idea, use the name of the object to lookup a layer in the Animator. We need to know the ID of that layer
                    //We send the layer ID to the LerpAnimation routine. If you get errors here, it's because your Layers in the Animator
                    //are not named the same as the collider that was hit. Fix is to rename the layer to match the collider's name
                    //and, to get rid of colliders that are not being used for interactions.
                    if (m_playedAnimations.Contains(hit.transform.name))
                    {
                        //This layer must already be active and set to 1
                        //Based on the colliders name, run the function to gradually change the layer from 1 to 0
                        StartCoroutine(LerpAnimation(m_animatorTransform.GetLayerIndex(hit.transform.name), 0f));
                        m_playedAnimations.Remove(hit.transform.name);
                    }
                    else
                    {
                        //Based on the colliders name, run the function to gradually change the layer from 0 to 1
                        StartCoroutine(LerpAnimation(m_animatorTransform.GetLayerIndex(hit.transform.name), 1f));
                        m_playedAnimations.Add(hit.transform.name);
                    }

                    m_animatorTransform.speed = m_animationSpeed;

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
            m_animatorTransform.SetLayerWeight(m_animatorTransform.GetLayerIndex(layerName), 0);
        }
        else
        {
            m_playedAnimations.Add(layerName);
            m_animatorTransform.SetLayerWeight(m_animatorTransform.GetLayerIndex(layerName), 1);
        }

    }

    //This function gradually transitions the layer to the animation, so it moves smoothly based on the transition speed set in the Inspector
    IEnumerator LerpAnimation(int layerIndex, float target)
    {
        float currentWeight = m_animatorTransform.GetLayerWeight(layerIndex);

        while (Mathf.Abs(currentWeight - target) >= .001f)
        {
            currentWeight = Mathf.Lerp(currentWeight, target, Time.deltaTime * m_transitionSpeed);
            m_animatorTransform.SetLayerWeight(layerIndex, currentWeight);
            yield return null;

        }
        Debug.Log("All Done with layer " + layerIndex.ToString());
    }

    public void ResetAnimations()
    {
        foreach (string s in m_playedAnimations)
        {
            StartCoroutine(LerpAnimation(m_animatorTransform.GetLayerIndex(s), 0f));
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
        m_animatorTransform.speed = speed;
    }
}
