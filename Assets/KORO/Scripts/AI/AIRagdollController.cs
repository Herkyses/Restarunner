using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRagdollController : MonoBehaviour
{

    public Rigidbody[] ragdollRigidbodies;
    public Collider[] ragdollColliders;
    public Animator characterAnimator;
    // Start is called before the first frame update


    private void Start()
    {
        characterAnimator = GetComponent<Animator>();
        SetRagdollState(false);
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

        characterAnimator.enabled = !state;
    }
}
