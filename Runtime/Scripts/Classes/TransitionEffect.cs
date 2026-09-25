using System.Collections;
using UnityEngine;

namespace AndresG09.SceneMaster
{
    public abstract class TransitionEffect : MonoBehaviour
    {
        public abstract IEnumerator StartTransition();
        public abstract IEnumerator EndTransition();
    }
}