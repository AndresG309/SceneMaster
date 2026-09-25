using UnityEngine;

namespace AndresG09.SceneMaster
{
    public abstract class LoadingScreen : MonoBehaviour
    {
        public abstract void Configure();
        public abstract void Activate(AsyncOperation operation);
    }
}