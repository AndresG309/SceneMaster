using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tester_SceneMaster : MonoBehaviour
{
    public static Tester_SceneMaster Instance;
    public TransitionEffect effect;
    public bool destroyOnLoad = true;
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
                    SceneMaster.Instance.TransitionToScene(sceneName, effect, callbackFunction());
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    SceneMaster.Instance.TransitionToScene(sceneName, effect);
                }
                else
                {
                    SceneMaster.Instance.TransitionToScene(sceneName);
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
                    SceneMaster.Instance.TransitionToScene(index, effect, callbackFunction());
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    SceneMaster.Instance.TransitionToScene(index, effect);
                }
                else
                {
                    SceneMaster.Instance.TransitionToScene(index);
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

