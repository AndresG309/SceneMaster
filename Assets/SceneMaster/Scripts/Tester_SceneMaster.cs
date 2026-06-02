using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tester_SceneMaster : MonoBehaviour
{
    public static Tester_SceneMaster Instance;
    public TransitionEffect effect;
    public bool destroyOnLoad = true;
    public bool changeSceneAsync = false;
    [Header("Use name for changing scene")]
    public bool useName = false;
    public string sceneName = "1";
    [Header("Use index for changing scene\n(Wont work if 'Use Name' is active)")]
    public bool changeToNextSceneOnBuildSettings = false;
    public int sceneIndex = 0;
    int index;
    void Awake()
    {
        if (destroyOnLoad) return;
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
            if (useName)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    var builder = SceneMaster.Instance.TransitionTo(sceneName)
                        .WithTransitionEffect(effect)
                        .WithCallback(callbackFunction());
                    if (changeSceneAsync) builder.LoadAsync();
                    builder.Execute();
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    var builder = SceneMaster.Instance.TransitionTo(sceneName)
                        .WithTransitionEffect(effect);
                    if (changeSceneAsync) builder.LoadAsync();
                    builder.Execute();
                }
                else
                {
                    var builder = SceneMaster.Instance.TransitionTo(sceneName);
                    if (changeSceneAsync) builder.LoadAsync();
                    builder.Execute();
                }
            }
            else
            {
                if (changeToNextSceneOnBuildSettings)
                {
                    index = SceneManager.GetActiveScene().buildIndex + 1;
                    if (index >= SceneManager.sceneCountInBuildSettings) index = 0;
                }
                else
                {
                    index = sceneIndex;
                }
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    var builder = SceneMaster.Instance.TransitionTo(index)
                        .WithTransitionEffect(effect)
                        .WithCallback(callbackFunction());
                    if (changeSceneAsync) builder.LoadAsync();
                    builder.Execute();
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    var builder = SceneMaster.Instance.TransitionTo(index)
                        .WithTransitionEffect(effect);
                    if (changeSceneAsync) builder.LoadAsync();
                    builder.Execute();
                }
                else
                {
                    var builder = SceneMaster.Instance.TransitionTo(index);
                    if (changeSceneAsync) builder.LoadAsync();
                    builder.Execute();
                }
            }
        }
    }
    IEnumerator callbackFunction()
    {
        Debug.Log("Iniciando callback desde Tester_SceneMaster");
        yield return new WaitForSeconds(3f);
        Debug.Log("Callback desde Tester_SceneMaster terminado");
    }
    public void setMastersEffect()
    {

    }
}

