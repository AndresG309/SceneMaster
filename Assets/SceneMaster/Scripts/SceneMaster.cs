using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMaster : MonoBehaviour
{
    public static SceneMaster Instance { get; private set; }
    [Tooltip("When 'TransitionToScene' is called without giving a TransitionEffect component, this effect will be used. It is needed for this effect to be a child of the SceneMaster object")]
    public TransitionEffect defaultTransition;

    TransitionEffect transitionCanvas;
    List<StringEffectPair> registeredTransitionsNames = new();

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
        if (defaultTransition == null)
        {
            Debug.Log("Default Transition missing on SceneMaster");
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
    public void TransitionToScene(int index, TransitionEffect transition = null, IEnumerator callback = null)
    {
        if (transition != null)
        {
            transitionCanvas = transition;
        }
        else
        {
            transitionCanvas = defaultTransition;
        }
        StartCoroutine(performTransition(index, callback));
    }

    IEnumerator performTransition(int index, IEnumerator callback)
    {
        registerEffect();
        transitionCanvas.gameObject.SetActive(true);
        yield return null;
        yield return transitionCanvas.StartTransition();
        SceneManager.LoadScene(index);
        if (callback != null) yield return callback;
        yield return transitionCanvas.EndTransition();
        yield return null;
        transitionCanvas.gameObject.SetActive(false);
    }
    void registerEffect()
    {
        GameObject transitionObject = transitionCanvas.gameObject;
        foreach (StringEffectPair pair in registeredTransitionsNames)
        {
            if (pair.name.Equals(transitionObject.name))
            {
                // If the name of the transition object that is trying to be assigned is already in the registered effects list, ignore the new assignment
                transitionCanvas = pair.effect;
                Debug.Log("[SceneMaster] The effect received is already registered. Using existing register.");
                return;
            }
        }
        transitionObject.transform.SetParent(this.transform, false);
        registeredTransitionsNames.Add(new StringEffectPair(transitionObject.name, transitionCanvas));

        Debug.Log("[SceneMaster] New effect received and registered.");
    }

    [Serializable]
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
