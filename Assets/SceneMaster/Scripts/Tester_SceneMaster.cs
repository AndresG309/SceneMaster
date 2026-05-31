using UnityEngine;

public class Tester_SceneMaster : MonoBehaviour
{
    public int index;
    void Awake()
    {
        DontDestroyOnLoad(this);
    }
    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneMaster.Instance.TransitionToScene(index);
        }
    }
}

