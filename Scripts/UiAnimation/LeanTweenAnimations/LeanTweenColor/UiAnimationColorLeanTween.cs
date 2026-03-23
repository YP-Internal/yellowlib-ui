using UnityEngine;
using UnityEngine.UI;

namespace YellowPanda.UI.LeanTweenUI
{
    public class UiAnimationColorLeanTween : UiAnimationCustomTween<TweenAnimationColorDataSO, TweenAnimationColorData>
    {
        [SerializeField] Graphic graphic;
        public override void Init(UIElement target)
        {
            base.Init(target);

            if (target.TryGetComponent(out Graphic graphic))
            {
                this.graphic = graphic;
            }
        }
        protected override void Evaluate(float t, TweenAnimationColorData data)
        {
            graphic.color = data.color.Evaluate(t);
        }
    }

}