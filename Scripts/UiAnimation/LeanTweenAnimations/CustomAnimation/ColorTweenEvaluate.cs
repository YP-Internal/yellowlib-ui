using UnityEngine;
namespace YellowPanda.UI
{
    public class ColorEvaluate : CustomLeanTweenEvaluate
    {
        public override float Time { get => throw new System.NotImplementedException(); protected set => throw new System.NotImplementedException(); }
        [SerializeField] float time;
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] AnimationCurve curve;

        public override void Evaluate(float t)
        {
            canvasGroup.alpha = curve.Evaluate(t);
        }
    }
}