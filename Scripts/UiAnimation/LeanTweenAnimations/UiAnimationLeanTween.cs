using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using YellowPanda.UI;

public class UiAnimationTransformLeanTween : UiAnimation
{
    public override float AnimationTime => animationData.time;
    public override bool IsPlaying => isPlaying;
    protected override bool CanInspectorPlay => Application.isPlaying;
    protected override bool CanInspectorStop => base.CanInspectorStop && Application.isPlaying;

    [ReadOnly]
    [FoldoutGroup(ANIMATION_SETTINGS)]
    public bool isPlaying;

    public enum TransformTweenType
    {
        Position,
        LocalPosition,
        AnchoredPosition,
        Scale,
        Rotate,
    }
    public enum EaseType { LeanTweenType, AnimationCurve }
    [Serializable]
    struct AnimationData
    {
        public TransformTweenType tweenType;
        public RectTransform target;
        public float time;

        public Vector3 to;

        public EaseType easeType;

        [ShowIf("@easeType == EaseType.AnimationCurve")]
        public AnimationCurve animationCurve;
        [ShowIf("@easeType == EaseType.LeanTweenType")]
        public LeanTweenType leanTweenType;

        public bool loop;
        public bool useLoopCounts;
        [ShowIf("@loop && useLoopCounts")]
        public int loopCount;

    }

    [FoldoutGroup(ANIMATION_SETTINGS)]
    [SerializeField] AnimationData animationData;


    public override void PlayAnimation()
    {
        isPlaying = true;

        LTDescr tween = null;

        switch (animationData.tweenType)
        {
            case TransformTweenType.Position:
                tween = LeanTween.move(animationData.target, animationData.to, animationData.time);
                break;
            case TransformTweenType.LocalPosition:
                tween = LeanTween.moveLocal(animationData.target.gameObject, animationData.to, animationData.time);
                break;
            case TransformTweenType.AnchoredPosition:
                var initialAnchoredPosition = animationData.target.anchoredPosition;
                tween = LeanTween.value(animationData.target.gameObject, 0, 1, animationData.time)
                    .setOnUpdate(t => animationData.target.anchoredPosition = Vector3.Lerp(initialAnchoredPosition, animationData.to, t));
                break;
            case TransformTweenType.Scale:
                tween = LeanTween.scale(animationData.target, animationData.to, animationData.time);
                break;
            case TransformTweenType.Rotate:
                tween = LeanTween.rotate(animationData.target, animationData.to, animationData.time);
                break;
        }

        if (tween == null) return;

        tween
            .setDelay(delay)
            .setOnComplete(Stop);

        if (animationData.loop)
            tween
                .setLoopPingPong(animationData.useLoopCounts ? animationData.loopCount : 0);


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

    public override void StopAnimation()
    {
        isPlaying = false;
        LeanTween.cancel(animationData.target);
    }
}
