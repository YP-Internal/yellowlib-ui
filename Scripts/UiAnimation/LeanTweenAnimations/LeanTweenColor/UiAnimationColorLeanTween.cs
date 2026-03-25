using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using YellowPanda.Core.AssetCreation;

namespace YellowPanda.UI.LeanTweenUI
{
    public class UiAnimationColorLeanTween : UiAnimation
    {
        [SerializeField, FoldoutGroup(ANIMATION_SETTINGS)]
        protected OverridableVariable<TweenAnimationColorData, TweenAnimationColorDataSO> data;
        [SerializeField] Graphic graphic;

        public override float AnimationTime => throw new System.NotImplementedException();
        public override bool IsPlaying => throw new System.NotImplementedException();

        public override void Init(UIElement target)
        {
            if (target.TryGetComponent(out Graphic graphic))
            {
                this.graphic = graphic;
            }
        }

        public override void PlayAnimation()
        {
            var _data = data.Value;

            var tween = LeanTween.color(graphic.gameObject, _data.to, _data.animationTime);

            tween
               .setDelay(delay)
               .setOnComplete(() =>
               {
                   Stop();
               });

            if (_data.loop)
                tween
                    .setLoopCount(_data.useLoopCounts ? _data.loopCount : 0)
                    .setLoopType(_data.loopType);


            switch (_data.easeType)
            {
                case EaseType.LeanTweenType:
                    tween.setEase(_data.leanTweenType);
                    break;
                case EaseType.AnimationCurve:
                    tween.setEase(_data.animationCurve);
                    break;
            }
        }

        public override void StopAnimation()
        {
            LeanTween.cancel(graphic.gameObject);
        }
    }

}