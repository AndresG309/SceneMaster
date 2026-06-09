using System.Collections;
using UnityEngine;

public abstract class TransitionEffect : MonoBehaviour
{
    public abstract IEnumerator StartTransition();
    public abstract IEnumerator EndTransition();
}