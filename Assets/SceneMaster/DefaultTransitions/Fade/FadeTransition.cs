using System.Collections;
using UnityEngine;
[RequireComponent(typeof(Animator))]
public class FadeTransition : TransitionEffect
{
    Animator animator;
    bool isPlaying = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public override IEnumerator StartTransition()
    {
        isPlaying = true;
        animator.SetTrigger("in");
        while (isPlaying) yield return null;
    }

    public override IEnumerator EndTransition()
    {
        isPlaying = true;
        animator.SetTrigger("out");
        while (isPlaying) yield return null;
    }
    public void animationFinished() => isPlaying = false;
}