using System.Collections;

namespace AndresG09.SceneMaster
{
    public class SceneTransitionRequestData
    {
        public int sceneIndex = -1;
        public TransitionEffect transition = null;
        public IEnumerator callback = null;
        public bool loadAsync = false;
        public bool useLoadingScreen = false;

        public SceneTransitionRequestData(int index)
        {
            this.sceneIndex = index;
        }
    }
}