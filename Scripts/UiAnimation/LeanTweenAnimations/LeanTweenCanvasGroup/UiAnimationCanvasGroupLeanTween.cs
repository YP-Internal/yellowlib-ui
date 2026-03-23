using UnityEngine;
using UnityEngine.UI;
using YellowPanda.UI;

namespace YellowPanda.UI.LeanTweenUI
{
    public class UiAnimationCanvasGroupLeanTween : UiAnimationCustomTween<TweenAnimationCanvasGroupDataSO, TweenAnimationCanvasGroupData>
    {
        [SerializeField] CanvasGroup canvasGroup;
        public override void Init(UIElement target)
        {
            base.Init(target);

            if (target.TryGetComponent(out CanvasGroup canvasGroup))
            {
                this.canvasGroup = canvasGroup;
            }
        }
        protected override void Evaluate(float t, TweenAnimationCanvasGroupData data)
        {
            canvasGroup.alpha = Mathf.Lerp(data.startAlpha, data.finalAlpha, t);
        }

    }
}
