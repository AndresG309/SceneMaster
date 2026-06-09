using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tester_SceneMaster : MonoBehaviour
{
    public static Tester_SceneMaster Instance;
    public TransitionEffect effect;
    public bool useSingleton = false;
    public bool changeSceneAsync = false;

    [Header("Use name for changing scene")]
    public bool useName = false;
    public string sceneName = "1";

    [Header("Use index for changing scene\n(Wont work if 'Use Name' is active)")]
    public bool changeToNextSceneOnBuildSettings = false;
    public int sceneIndex = 0;

    void Awake()
    {
        if (!useSingleton) return;
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

    void Update()
    {
        if (Input.anyKeyDown)
        {
            HandleSceneTransition();
        }
    }

    void HandleSceneTransition()
    {
        var builder = useName
            ? SceneMaster.Instance.TransitionTo(sceneName)
            : SceneMaster.Instance.TransitionTo(GetSceneIndex());

        // Transition effect
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            builder.WithTransitionEffect(effect);
        }

        // Use callback
        if (Input.GetKeyDown(KeyCode.Return))
        {
            builder.WithCallback(callbackFunction());
        }

        // Use Load Screen
        if (Input.GetKeyDown(KeyCode.L))
        {
            builder.WithLoadingScreen();
        }

        if (changeSceneAsync)
            builder.LoadAsync();

        builder.Execute();
    }

    int GetSceneIndex()
    {
        if (changeToNextSceneOnBuildSettings)
        {
            int index = SceneManager.GetActiveScene().buildIndex + 1;
            return index >= SceneManager.sceneCountInBuildSettings ? 0 : index;
        }
        return sceneIndex;
    }

    IEnumerator callbackFunction()
    {
        Debug.Log("[Tester_SceneMaster] Starting callback function");
        yield return new WaitForSeconds(3f);
        Debug.Log("[Tester_SceneMaster] Ending callback function");
    }
}

