using UnityEngine;
using UnityEngine.SceneManagement;

public class Tester_SceneMaster : MonoBehaviour
{
    int index;
    void Awake()
    {
        DontDestroyOnLoad(this);
    }
    void Update()
    {
        if (Input.anyKeyDown)
        {
            index = SceneManager.GetActiveScene().buildIndex + 1;
            if (index >= SceneManager.sceneCountInBuildSettings) index = 0;
            Debug.Log(index);
            SceneMaster.Instance.TransitionToScene(index);
        }
    }
}

