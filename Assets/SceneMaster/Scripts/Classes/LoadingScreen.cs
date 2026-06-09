using  UnityEngine;

public abstract class LoadingScreen : MonoBehaviour
{
    public abstract void Configure();
    public abstract void Activate(AsyncOperation operation);
}