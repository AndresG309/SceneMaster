using  UnityEngine;

public abstract class LoadingScreen : MonoBehaviour
{
    public abstract void Activate(AsyncOperation operation);
    public abstract void Deactivate();
}