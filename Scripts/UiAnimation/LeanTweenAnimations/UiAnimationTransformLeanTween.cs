using System;
using Sirenix.OdinInspector;
using UnityEngine;
using YellowPanda.Core.AssetCreation;

namespace YellowPanda.UI
{
    public class UiAnimationTransformLeanTween : UiAnimation
    {
        public override float AnimationTime => 1;// overrideAnimationData.animationTime;
        public override bool IsPlaying => isPlaying;
        protected override bool CanInspectorPlay => Application.isPlaying;
        protected override bool CanInspectorStop => base.CanInspectorStop && Application.isPlaying;

        [ReadOnly]
        [FoldoutGroup(ANIMATION_SETTINGS)]
        public bool isPlaying;

        public enum TransformTweenType
        {
            Scale,
            Position,
            LocalPosition,
            AnchoredPosition,
            Rotate,
        }
        [Serializable]
        public class TransformTweenAnimationData : TweenAnimationData
        {
            [BoxGroup("TransformTween", order: 0)]
            public TransformTweenType tweenType;

            [BoxGroup("TransformTween", order: 0)]
            public Vector3 to;
        }

        public class TransformTweenAnimationDataSO : OverridableVariableSO<TransformTweenAnimationData> { }

        [FoldoutGroup(ANIMATION_SETTINGS)]
        [SerializeField]
        public RectTransform target;
        [FoldoutGroup(ANIMATION_SETTINGS)]
        [SerializeField]
        OverridableVariable<TransformTweenAnimationData, TransformTweenAnimationDataSO> animationData;


        
        Vector3 originalValue;
        void Start()
        {
            originalValue = animationData.Value.tweenType switch
            {
                TransformTweenType.Scale => target.localScale,
                TransformTweenType.Position => target.position,
                TransformTweenType.LocalPosition => target.localPosition,
                TransformTweenType.AnchoredPosition => target.anchoredPosition,
                TransformTweenType.Rotate => target.eulerAngles,
                _ => throw new NotImplementedException(),
            };
        }


        public override void PlayAnimation()
        {
            isPlaying = true;

            LTDescr tween = null;

            var animationData = this.animationData.Value;

            LeanTween.cancel(target);

            switch (animationData.tweenType)
            {
                case TransformTweenType.Position:
                    tween = LeanTween.move(target, animationData.to, animationData.animationTime);
                    break;
                case TransformTweenType.LocalPosition:
                    tween = LeanTween.moveLocal(target.gameObject, animationData.to, animationData.animationTime);
                    break;
                case TransformTweenType.AnchoredPosition:
                    var initialAnchoredPosition = target.anchoredPosition;
                    tween = LeanTween.value(target.gameObject, 0, 1, animationData.animationTime)
                        .setOnUpdate(t => target.anchoredPosition = Vector3.Lerp(initialAnchoredPosition, animationData.to, t));
                    break;
                case TransformTweenType.Scale:
                    tween = LeanTween.scale(target, animationData.to, animationData.animationTime);
                    break;
                case TransformTweenType.Rotate:
                    tween = LeanTween.rotate(target, animationData.to, animationData.animationTime);
                    break;
            }


            if (tween == null) return;

            tween
                .setDelay(delay)
                .setOnComplete(() =>
                {
                    Stop();
                    if (animationData.returnToOriginalValueOnAnimationEnd)
                        GoBackToOriginalValue(animationData.tweenType);
                });

            if (animationData.loop)
                tween
                    .setLoopCount(animationData.useLoopCounts ? animationData.loopCount : 0)
                    .setLoopType(animationData.loopType);


            switch (animationData.easeType)
            {
                case EaseType.LeanTweenType:
                    tween.setEase(animationData.leanTweenType);
                    break;
                case EaseType.AnimationCurve:
                    tween.setEase(animationData.animationCurve);
                    break;
            }
        }

        void GoBackToOriginalValue(TransformTweenType type)
        {
            switch (type)
            {
                case TransformTweenType.Scale:
                    target.localScale = originalValue;
                    break;
                case TransformTweenType.Position:
                    target.position = originalValue;
                    break;
                case TransformTweenType.LocalPosition:
                    target.localPosition = originalValue;
                    break;
                case TransformTweenType.AnchoredPosition:
                    target.anchoredPosition = originalValue;
                    break;
                case TransformTweenType.Rotate:
                    target.eulerAngles = originalValue;
                    break;
            }
        }

        public override void StopAnimation()
        {
            isPlaying = false;
            LeanTween.cancel(target);
        }

        public override void Init(UIElement target)
        {
            this.target = target.transform as RectTransform;
        }

        public override void CreateAsset(string path)
        {
            throw new NotImplementedException();
        }
    }
}