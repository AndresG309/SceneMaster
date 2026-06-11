using System.Collections;

public class SceneTransitionRequest
{
    public int sceneIndex = -1;
    public TransitionEffect transition = null;
    public IEnumerator callback = null;
    public bool loadAsync = false;
    public bool useLoadingScreen = false;
    
    public SceneTransitionRequest(int index)
    {
        this.sceneIndex = index;
    }
}