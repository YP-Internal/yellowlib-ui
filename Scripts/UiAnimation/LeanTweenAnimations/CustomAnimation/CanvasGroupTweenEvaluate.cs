using UnityEngine;

namespace YellowPanda.UI
{
    public class CanvasGroupEvaluate : CustomLeanTweenEvaluate
    {
        public override float Time { get => time; protected set => time = value; }
        [SerializeField] float time;
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] AnimationCurve curve;

        public override void Evaluate(float t)
        {
            canvasGroup.alpha = curve.Evaluate(t);
        }
    }
}