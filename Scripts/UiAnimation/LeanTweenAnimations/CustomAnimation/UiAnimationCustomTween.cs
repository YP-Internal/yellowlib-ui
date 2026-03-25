using Sirenix.OdinInspector;
using UnityEngine;
using YellowPanda.Core.AssetCreation;

namespace YellowPanda.UI.LeanTweenUI
{
    public abstract class LeanTweenUiAnimation<DataSO, Data> : UiAnimation where Data : TweenAnimationData where DataSO : OverridableVariableSO<Data>
    {
        [SerializeField, FoldoutGroup(ANIMATION_SETTINGS)] protected OverridableVariable<Data, DataSO> data;
        public override float AnimationTime => data.Value.animationTime;
        public override bool IsPlaying => isPlaying;
        bool isPlaying;

        public override void Init(UIElement target) { }
        public override void PlayAnimation()
        {
            isPlaying = true;

            Data _data = data.Value;

            var tween = LeanTween.value(gameObject, 0f, 1f, AnimationTime)
                .setOnUpdate((float t) =>
                {
                    Evaluate(t, _data);
                });

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
            isPlaying = false;
            LeanTween.cancel(gameObject);
        }

        protected abstract void Evaluate(float t, Data data);
    }
}