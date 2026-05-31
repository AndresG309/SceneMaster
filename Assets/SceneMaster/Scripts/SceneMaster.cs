using System.Collections;
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
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void TransitionToScene(int index, IEnumerator callback = null)
    {
        StartCoroutine(performTransition(index, callback));
    }

    IEnumerator performTransition(int index, IEnumerator callback)
    {
        setEffectAsChild();
        transitionCanvas.gameObject.SetActive(true);
        yield return null;
        yield return transitionCanvas.StartTransition();
        SceneManager.LoadScene(index);
        if (callback != null) yield return callback;
        yield return transitionCanvas.EndTransition();
        yield return null;
        transitionCanvas.gameObject.SetActive(false);
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