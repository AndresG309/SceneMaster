using System.Collections;

namespace AndresG09.SceneMaster
{
    public class SceneTransitionBuilder
    {
        SceneMaster sceneMaster;
        SceneTransitionRequestData request;

        public SceneTransitionBuilder(SceneMaster sceneMaster, int sceneIndex)
        {
            this.sceneMaster = sceneMaster;
            this.request = new(sceneIndex);
        }

        public SceneTransitionBuilder WithTransitionEffect(TransitionEffect effect, bool setAsDefault = false)
        {
            this.request.transition = effect;
            this.request.setEffectAsDefault = setAsDefault;
            return this;
        }

        public SceneTransitionBuilder WithLoadingScreen()
        {
            this.request.useLoadingScreen = true;
            this.request.loadAsync = true;
            return this;
        }

        public SceneTransitionBuilder WithCallback(IEnumerator callback)
        {
            this.request.callback = callback;
            return this;
        }

        public SceneTransitionBuilder LoadAsync()
        {
            this.request.loadAsync = true;
            return this;
        }

        public void Execute()
        {
            sceneMaster.PerformTransition(this.request);
        }
    }
}