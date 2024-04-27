using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator _playerAnimator;
    // Start is called before the first frame update

    
    public void PlayRunAnimation()
    {
        AnimatorStateInfo currentState = _playerAnimator.GetCurrentAnimatorStateInfo(0);

        string currentAnimName = currentState.IsName("Run") ? "Run" : currentState.fullPathHash.ToString();
        if (currentAnimName != "Run")
        {
            Debug.Log("Current Animation: " + currentAnimName);
            _playerAnimator.Play("Run", -1, normalizedTime: 0.0f);
        }
    }
    public void PlayIdleAnimation()
    {
        AnimatorStateInfo currentState = _playerAnimator.GetCurrentAnimatorStateInfo(0);

        string currentAnimName = currentState.IsName("Idle") ? "Idle" : currentState.fullPathHash.ToString();
        if (currentAnimName != "Idle")
        {
            Debug.Log("Current Animation: " + currentAnimName);
            _playerAnimator.Play("Idle", -1, normalizedTime: 0.0f);
        }
    }
}
