using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRagdollController : MonoBehaviour
{

    public Rigidbody[] ragdollRigidbodies;
    public Collider[] ragdollColliders;
    public Animator characterAnimator;
    public GameObject AIChildObject;
    public Vector3 AIChildObjectPos;
    public Vector3 AIChildObjectRot;
    public List<Vector3> aAIChildObjectPos;
    public List<Vector3> aAIChildObjectRot;
    // Start is called before the first frame update


    private void Start()
    {
        characterAnimator = GetComponent<Animator>();
        SetRagdollState(false);
        AIChildObjectPos = AIChildObject.transform.localPosition;
        AIChildObjectRot = AIChildObject.transform.localRotation.eulerAngles;
        for (int i = 0; i < ragdollRigidbodies.Length; i++)
        {
            aAIChildObjectPos.Add(ragdollRigidbodies[i].transform.localPosition);
            aAIChildObjectRot.Add(ragdollRigidbodies[i].transform.localRotation.eulerAngles);
        }
    }

    public void SetRagdollState(bool state)
    {
        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            rb.isKinematic = !state;
        }

        foreach (Collider col in ragdollColliders)
        {
            col.enabled = state;
        }

        if (state)
        {
            GetComponent<AIController>()._agent.speed = 0;
        }
        else
        {
            GetComponent<AIController>()._agent.speed = 1;

        }
        characterAnimator.enabled = !state;
    }

    public void AddForceToAI(Vector3 direction)
    {
        var elapsedTime = 0f;
        while (elapsedTime < 1)
        {
            // Kuvvet uygulama işlemi
            transform.position += direction * 10f * Time.deltaTime;
            elapsedTime += Time.deltaTime;
        }
    }
    public IEnumerator AddForceToAICor(Vector3 direction)
    {
        var elapsedTime = 0f;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + Vector3.one*5f);

        while (elapsedTime < 0.1)
        {
            // Kuvvet uygulama işlemi
            transform.position += direction * 25f * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(ReturnWalk());

    }
    public IEnumerator ReturnWalk()
    {
        yield return new WaitForSeconds(2f);
        AIChildObject.transform.localPosition = AIChildObjectPos;
        AIChildObject.transform.localRotation = Quaternion.Euler(AIChildObjectRot);
        for (int i = 0; i < ragdollRigidbodies.Length; i++)
        {
            ragdollRigidbodies[i].transform.localPosition = aAIChildObjectPos[i];
            ragdollRigidbodies[i].transform.localRotation = Quaternion.Euler(aAIChildObjectRot[i]);
        }
        SetRagdollState(false);
        
    }
}
