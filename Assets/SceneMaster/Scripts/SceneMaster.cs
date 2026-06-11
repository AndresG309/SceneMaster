using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMaster : MonoBehaviour
{
    public static SceneMaster Instance { get; private set; }
    [Tooltip("When a transition is made without giving a TransitionEffect, this effect will be used.\nWhen a transition is made giving a TransitionEffect, that effect will be set as the new default transition.\nIt is needed for this effect to be a child of the SceneMaster object.")]
    public TransitionEffect defaultTransition;
    public string LoadingScreenSceneName = "";

    TransitionEffect transitionCanvas;
    List<StringEffectPair> registeredTransitionsNames = new();
    bool isChangingScene;
    LoadingScreen loadingScreen;
    string lastSavedLoadingScreenSceneName;
    int loadingScreenSceneIndex;

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
    #region Public API

    public int CurrentSceneIndex { get; private set; }
    public string CurrentSceneName => SceneManager.GetActiveScene().name;

    // =================================================================================
    // FLUENT BUILDER
    // =================================================================================

    public SceneTransitionBuilder TransitionTo(int index)
    {
        ValidateIndex(index, "Please check the name or buildIndex of the scene you are trying to load.");
        return new SceneTransitionBuilder(this, index);
    }
    public SceneTransitionBuilder TransitionTo(string name)
    {
        int index = GetSceneIndex(name);
        return TransitionTo(index);
    }

    // =================================================================================
    // PERFORM THE TRANSITION
    // =================================================================================
    public void PerformTransition(SceneTransitionRequest request)
    {
        if (isChangingScene)
        {
            Debug.LogWarning("[SceneMaster] Already changing scene. Ignoring new request.");
            return;
        }
        if (request.transition != null)
        {
            transitionCanvas = request.transition;
            RegisterEffect();
            defaultTransition = transitionCanvas;
        }
        else if (defaultTransition != null)
        {
            transitionCanvas = defaultTransition;
        }
        else
        {
            Debug.LogWarning("[SceneMaster] There is no transition neither default transition assigned. Ignoring request.");
            return;
        }
        StopAllCoroutines();
        StartCoroutine(TransitionSequence(request));
    }
    #endregion

    #region Internal methods
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
    void ValidateIndex(int index, string customErrorMessage = null)
    {
        if (index < 0 || index >= SceneManager.sceneCountInBuildSettings)
        {
            isChangingScene = false;
            throw new ArgumentOutOfRangeException(
                nameof(index),
                $"Scene index {index} is not configured in Build Settings. {customErrorMessage}"
            );
        }
    }

    IEnumerator TransitionSequence(SceneTransitionRequest request)
    {
        isChangingScene = true;
        transitionCanvas.gameObject.SetActive(true);
        yield return null;

        // Execute the appropriate loading strategy
        if (request.loadAsync && request.useLoadingScreen)
        {
            yield return PerformAsyncTransitionWithLoadingScreen(request.sceneIndex);
        }
        else if (request.loadAsync)
        {
            yield return PerformAsyncTransition(request.sceneIndex);
        }
        else
        {
            yield return PerformSyncTransition(request.sceneIndex);
        }

        // Finish the transition
        yield return null;
        CurrentSceneIndex = request.sceneIndex;
        if (request.callback != null) yield return request.callback;
        yield return transitionCanvas.EndTransition();
        yield return null;
        transitionCanvas.gameObject.SetActive(false);
        isChangingScene = false;
    }
    IEnumerator PerformAsyncTransitionWithLoadingScreen(int index)
    {
        if (LoadingScreenSceneName != lastSavedLoadingScreenSceneName)
        {
            loadingScreenSceneIndex = GetSceneIndex(LoadingScreenSceneName);
            ValidateIndex(loadingScreenSceneIndex, "Please check the LoadingScreen scene name you are trying to use.");
            lastSavedLoadingScreenSceneName = LoadingScreenSceneName;
        }
        // Transition In - Show loading screen
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(loadingScreenSceneIndex);
        asyncOp.allowSceneActivation = false;
        yield return new WaitUntil(() => asyncOp.progress >= 0.9f);
        yield return transitionCanvas.StartTransition();
        asyncOp.allowSceneActivation = true;
        yield return new WaitUntil(() => asyncOp.isDone);
        yield return null;
        loadingScreen = FindObjectOfType<LoadingScreen>();
        yield return null;
        loadingScreen.Configure();
        yield return transitionCanvas.EndTransition();
        // Start laoding the new game scene
        asyncOp = SceneManager.LoadSceneAsync(index);
        asyncOp.allowSceneActivation = false;
        loadingScreen.Activate(asyncOp);

        // Wait for loading to complete
        yield return new WaitUntil(() => asyncOp.progress >= 0.9f);

        // Transition Out - Hide loading screen
        yield return transitionCanvas.StartTransition();
        asyncOp.allowSceneActivation = true;
        yield return new WaitUntil(() => asyncOp.isDone);
    }

    IEnumerator PerformAsyncTransition(int index)
    {
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(index);
        asyncOp.allowSceneActivation = false;

        // Wait for scene to load before starting transition
        yield return new WaitUntil(() => asyncOp.progress >= 0.9f);
        yield return transitionCanvas.StartTransition();
        asyncOp.allowSceneActivation = true;
        yield return new WaitUntil(() => asyncOp.isDone);
    }

    IEnumerator PerformSyncTransition(int index)
    {
        yield return transitionCanvas.StartTransition();
        SceneManager.LoadScene(index);
    }
    void RegisterEffect()
    {
        GameObject transitionObject = transitionCanvas.gameObject;
        foreach (StringEffectPair pair in registeredTransitionsNames)
        {
            if (pair.name.Equals(transitionObject.name))
            {
                // If the name of the transition object that is trying to be assigned is already in the registered effects list, ignore the new assignment
                Debug.Log("[SceneMaster] The effect received is already registered. Using existing registry.");
                transitionCanvas = pair.effect;
                return;
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
