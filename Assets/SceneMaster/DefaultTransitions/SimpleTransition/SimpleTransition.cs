using System.Collections;
using UnityEngine;
[RequireComponent(typeof(Animator))]
public class SimpleTransition : TransitionEffect
{
    Animator animator;
    bool isPlaying;

    void Awake()
    {
        isPlaying = false;
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        gameObject.SetActive(false);
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