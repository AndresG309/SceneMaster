using System.Collections;
using UnityEngine;
public class FadeTransition : TransitionEffect
{
    Animation animationPlayer;
    public AnimationClip fadeIn;
    public AnimationClip fadeOut;

    void Start()
    {
        animationPlayer = GetComponent<Animation>();
    }
    public override IEnumerator StartTransition()
    {
        animationPlayer.clip = fadeIn;
        animationPlayer.Play();
        yield return new WaitForSeconds(fadeIn.length);
    }

    public override IEnumerator EndTransition()
    {
        animationPlayer.clip = fadeOut;
        animationPlayer.Play();
        yield return new WaitForSeconds(fadeOut.length);
    }
}