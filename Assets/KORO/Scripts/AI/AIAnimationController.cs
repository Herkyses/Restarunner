using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAnimationController : MonoBehaviour
{
    public Animator AiAnimator;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.TryGetComponent(out AiAnimator);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayClapAnimation()
    {
        
    }
    public void PlayMoveAnimation()
    {
        float randomTime = Random.Range(0f, 1f);
        AiAnimator.Play("Walk",0,randomTime);

    }
    public void PlayIdleAnimation()
    {
        float randomTime = Random.Range(0f, 1f);
        AiAnimator.Play("Idle",0);
    }
    public void PlaySitAnimation()
    {
        
    }
}
