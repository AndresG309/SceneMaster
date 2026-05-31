using System.Collections;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMaster : MonoBehaviour
{
    public static SceneMaster Instance { get; private set; }
    public TransitionEffect transitionCanvas;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void TransitionToScene(int index)
    {
        StartCoroutine(performTransition(index));
    }

    IEnumerator performTransition(int index)
    {
        setEffectAsChild();
        yield return transitionCanvas.StartTransition();
        yield return loadScene(index);
        yield return transitionCanvas.EndTransition();
    }
    IEnumerator loadScene(int index)
    {
        SceneManager.LoadScene(index);
        yield return null;
    }
    void setEffectAsChild()
    {
        GameObject transitionObject = transitionCanvas.gameObject;
        if (transitionObject.transform.parent != this.gameObject)
        {
            transitionObject.transform.SetParent(this.gameObject.transform, false);
        }
    }
}