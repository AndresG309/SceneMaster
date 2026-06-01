using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMaster : MonoBehaviour
{
    public static SceneMaster Instance { get; private set; }
    [Tooltip("When 'TransitionToScene' is called without giving a TransitionEffect component, this effect will be used. It is needed for this effect to be a child of the SceneMaster object")]
    public TransitionEffect defaultTransition;

    TransitionEffect transitionCanvas;
    List<StringEffectPair> registeredTransitionsNames = new();
    bool isChangingScene;

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
    void Start()
    {
        isChangingScene = false;
        CurrentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (defaultTransition == null)
        {
            Debug.LogWarning("[SceneMaster] Default Transition missing.");
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform currentChild = transform.GetChild(i);
            TransitionEffect effect;
            currentChild.TryGetComponent(out effect);
            if (effect != null)
            {
                StringEffectPair newEffect = new StringEffectPair(currentChild.name, effect);
                registeredTransitionsNames.Add(newEffect);
            }
        }
    }
    #region API PÚBLICA

    public int CurrentSceneIndex { get; private set; }
    public string CurrentSceneName => SceneManager.GetActiveScene().name;

    /// <summary>
    /// Transitions to a scene by its name
    /// </summary>
    public void TransitionToScene(string sceneName, TransitionEffect transition = null, IEnumerator callback = null)
    {
        int sceneIndex = GetSceneIndex(sceneName);
        if (sceneIndex == -1)
        {
            Debug.LogError($"[SceneMaster] Scene '{sceneName}' not found in Build Settings.");
            return;
        }
        TransitionToScene(sceneIndex, transition, callback);
    }
    /// <summary>
    /// Transitions to a scene by its index in build settings
    /// </summary>
    public void TransitionToScene(int index, TransitionEffect transition = null, IEnumerator callback = null)
    {
        if (isChangingScene) return;
        if (transition != null)
        {
            transitionCanvas = transition;
            registerEffect();
        }
        else
        {
            transitionCanvas = defaultTransition;
        }
        if (transitionCanvas == null)
        {
            Debug.LogWarning("[SceneMaster] There is no transition assigned.");
            return;
        }
        StopAllCoroutines();
        StartCoroutine(performTransition(index, callback));
    }
    #endregion
    #region Métodos privados
    int GetSceneIndex(string sceneName)
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < sceneCount; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string nameFromPath = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            if (nameFromPath == sceneName)
            {
                return i;
            }
        }
        return -1;
    }

    IEnumerator performTransition(int index, IEnumerator callback)
    {
        isChangingScene = true;
        transitionCanvas.gameObject.SetActive(true);
        yield return null;
        yield return transitionCanvas.StartTransition();
        SceneManager.LoadScene(index);
        CurrentSceneIndex = index;
        if (callback != null) yield return callback;
        yield return transitionCanvas.EndTransition();
        yield return null;
        transitionCanvas.gameObject.SetActive(false);
        isChangingScene = false;
    }
    void registerEffect()
    {
        GameObject transitionObject = transitionCanvas.gameObject;
        foreach (StringEffectPair pair in registeredTransitionsNames)
        {
            if (pair.name.Equals(transitionObject.name))
            {
                // If the name of the transition object that is trying to be assigned is already in the registered effects list, ignore the new assignment
                Debug.Log("[SceneMaster] The effect received is already registered. Using existing registry.");
                transitionCanvas = pair.effect;
            }
        }
        transitionObject.transform.SetParent(transform, false);
        registeredTransitionsNames.Add(new StringEffectPair(transitionObject.name, transitionCanvas));

        Debug.Log("[SceneMaster] New effect received and registered.");
    }
    #endregion
    class StringEffectPair
    {
        public string name;
        public TransitionEffect effect;

        public StringEffectPair(string _name, TransitionEffect _effect)
        {
            name = _name;
            effect = _effect;
        }
    }
}
