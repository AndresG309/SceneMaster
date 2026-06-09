using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenWithProgressBar : LoadingScreen
{
    [SerializeField] Slider progressBar;

    AsyncOperation asyncOp;

    void Start()
    {
        gameObject.SetActive(false);
    }
    public override void Activate(AsyncOperation op)
    {
        progressBar.value = 0;
        gameObject.SetActive(true);
        asyncOp = op;
        StartCoroutine(StartLoading());
    }
    public override void Deactivate()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }
    IEnumerator StartLoading()
    {
        while (!asyncOp.isDone)
        {
            progressBar.value = Mathf.Clamp01(asyncOp.progress / 0.9f);
            yield return null;
        }
    }
}
