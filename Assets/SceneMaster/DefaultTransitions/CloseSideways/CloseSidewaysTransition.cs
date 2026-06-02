using System.Collections;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class CloseSidewaysTransition : TransitionEffect
{
    [SerializeField] float transitionTime = 1f;
    [SerializeField] AnimationCurve curve;
    [SerializeField] RectTransform leftPanel;
    [SerializeField] RectTransform rightPanel;
    Vector2 leftPanelOriginPos;
    Vector2 leftPanelFinalPos;
    Vector2 rightPanelOriginPos;
    Vector2 rightPanelFinalPos;


    void Start()
    {
        RectTransform canvas = GetComponent<RectTransform>();

        leftPanelOriginPos = new Vector2(-canvas.rect.width * 0.5f, 0);
        leftPanelFinalPos = Vector2.zero;
        leftPanel.anchoredPosition = leftPanelOriginPos;

        rightPanelOriginPos = new Vector2(canvas.rect.width * 0.5f, 0);
        rightPanelFinalPos = Vector2.zero;
        rightPanel.anchoredPosition = rightPanelOriginPos;

        gameObject.SetActive(false);
    }
    public override IEnumerator StartTransition()
    {
        float timer = 0;
        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / transitionTime);
            t = curve.Evaluate(t);

            leftPanel.anchoredPosition = Vector2.Lerp(leftPanelOriginPos, leftPanelFinalPos, t);
            rightPanel.anchoredPosition = Vector2.Lerp(rightPanelOriginPos, rightPanelFinalPos, t);

            yield return null;
        }
        leftPanel.anchoredPosition = leftPanelFinalPos;
        rightPanel.anchoredPosition = rightPanelFinalPos;
    }
    public override IEnumerator EndTransition()
    {
        float timer = 0;
        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / transitionTime);
            t = curve.Evaluate(t);

            leftPanel.anchoredPosition = Vector2.Lerp(leftPanelFinalPos, leftPanelOriginPos, t);
            rightPanel.anchoredPosition = Vector2.Lerp(rightPanelFinalPos, rightPanelOriginPos, t);

            yield return null;
        }
        leftPanel.anchoredPosition = leftPanelOriginPos;
        rightPanel.anchoredPosition = rightPanelOriginPos;
    }
}