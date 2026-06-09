using System.Collections;

public class SceneTransitionBuilder
{
    int sceneIndex = -1;
    TransitionEffect transition = null;
    IEnumerator callback = null;
    bool loadAsync = false;
    bool useLoadingScreen = false;
    SceneMaster sceneMaster = null;

    public SceneTransitionBuilder(SceneMaster sceneMaster, int sceneIndex)
    {
        this.sceneMaster = sceneMaster;
        this.sceneIndex = sceneIndex;
    }

    public SceneTransitionBuilder WithTransitionEffect(TransitionEffect effect)
    {
        this.transition = effect;
        return this;
    }

    public SceneTransitionBuilder WithLoadingScreen()
    {
        this.useLoadingScreen = true;
        this.loadAsync = true;
        return this;
    }

    public SceneTransitionBuilder WithCallback(IEnumerator callback)
    {
        this.callback = callback;
        return this;
    }

    public SceneTransitionBuilder LoadAsync()
    {
        this.loadAsync = true;
        return this;
    }

    public void Execute()
    {
        sceneMaster.TransitionToScene(sceneIndex, transition, callback, loadAsync, useLoadingScreen);
    }

}