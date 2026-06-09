using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenWithProgressBar : LoadingScreen
{
    [SerializeField] Slider progressBar;

    AsyncOperation asyncOp;

    public override void Configure()
    {
        progressBar.value = 0;
    }
    public override void Activate(AsyncOperation op)
    {
        asyncOp = op;
        StartCoroutine(StartLoading());
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
