using UnityEngine;
using YellowPanda.Core.AssetCreation;

namespace YellowPanda.UI.LeanTweenUI
{
    public abstract class UiAnimationCustomTween<DataSO, Data> : UiAnimation where Data : TweenAnimationData where DataSO : OverridableVariableSO<Data>
    {
        [SerializeField] protected OverridableVariable<Data, DataSO> data;
        public override float AnimationTime => data.Value.animationTime;
        public override bool IsPlaying => isPlaying;
        bool isPlaying;

        public override void Init(UIElement target) { }
        public override void PlayAnimation()
        {
            isPlaying = true;

            Data _data = data.Value;

            LeanTween.value(gameObject, 0f, 1f, AnimationTime)
                .setOnUpdate((float t) =>
                {
                    Evaluate(t, _data);
                })
                .setOnComplete(Stop);
        }

        public override void StopAnimation()
        {
            isPlaying = false;
            LeanTween.cancel(gameObject);
        }

        protected abstract void Evaluate(float t, Data data);
    }
}