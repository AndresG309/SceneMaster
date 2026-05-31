using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tester_SceneMaster : MonoBehaviour
{
    public static Tester_SceneMaster Instance;
    int index;
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
    void Update()
    {

        if (Input.anyKeyDown)
        {
            index = SceneManager.GetActiveScene().buildIndex + 1;
            if (index >= SceneManager.sceneCountInBuildSettings) index = 0;
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SceneMaster.Instance.TransitionToScene(index, callbackFunction());
            }
            else
            {
                SceneMaster.Instance.TransitionToScene(index);
            }
        }
    }
    IEnumerator callbackFunction()
    {
        Debug.Log("Iniciando callback desde Tester_SceneMaster");
        yield return new WaitForSeconds(3f);
        Debug.Log("Callback desde Tester_SceneMaster terminado");
    }
}

