using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using YellowPanda.UI;

public class UiAnimationTransformLeanTween : UiAnimation
{
    public override float AnimationTime => overrideAnimationData.time;
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
    public struct AnimationData
    {
        public TransformTweenType tweenType;
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
    [SerializeField]
    public RectTransform target;
    [FoldoutGroup(ANIMATION_SETTINGS)]
    [SerializeField]
    UiAnimationLeanTweenData animationData;

    [FoldoutGroup(ANIMATION_SETTINGS)]
    [ToggleLeft]
    [SerializeField]
    bool overrideData;

    [FoldoutGroup(ANIMATION_SETTINGS)]
    [ShowIf(nameof(overrideData))]
    [SerializeField]
    AnimationData overrideAnimationData;

    AnimationData CurrentAnimationData
    {
        get
        {
            if (overrideData)
                return overrideAnimationData;

            return animationData.data;
        }
    }

    [FoldoutGroup(ANIMATION_SETTINGS)]
    [Button]
    void CreateAnimationData()
    {
        string directoryPath = "Assets/Resources/UiAnimation/Lean Tweem";

        FolderUtils.EnsureDirectoryExists(directoryPath);

        string path = EditorUtility.SaveFilePanelInProject(
            "Create Animation Data",
            "NewAnimationData",
            "asset",
            "Enter asset name",
            directoryPath
        );

        animationData = UiAnimationScriptableObjectFactory.Create<UiAnimationLeanTweenData>(path);

        overrideData = false;
    }


    public override void PlayAnimation()
    {
        isPlaying = true;

        LTDescr tween = null;

        var animationData = CurrentAnimationData;

        switch (animationData.tweenType)
        {
            case TransformTweenType.Position:
                tween = LeanTween.move(target, animationData.to, animationData.time);
                break;
            case TransformTweenType.LocalPosition:
                tween = LeanTween.moveLocal(target.gameObject, animationData.to, animationData.time);
                break;
            case TransformTweenType.AnchoredPosition:
                var initialAnchoredPosition = target.anchoredPosition;
                tween = LeanTween.value(target.gameObject, 0, 1, animationData.time)
                    .setOnUpdate(t => target.anchoredPosition = Vector3.Lerp(initialAnchoredPosition, animationData.to, t));
                break;
            case TransformTweenType.Scale:
                tween = LeanTween.scale(target, animationData.to, animationData.time);
                break;
            case TransformTweenType.Rotate:
                tween = LeanTween.rotate(target, animationData.to, animationData.time);
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
        LeanTween.cancel(target);
    }

    public override void Init(UIElement target)
    {
        this.target = target.transform as RectTransform;
    }
}
