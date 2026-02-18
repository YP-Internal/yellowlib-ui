using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace YellowPanda.UI
{
    public enum EaseType { LeanTweenType, AnimationCurve }

    [Serializable]
    public class TweenAnimationData
    {
        public float animationTime;
        public EaseType easeType;

        [ShowIf("@easeType == EaseType.AnimationCurve")]
        public AnimationCurve animationCurve;
        [ShowIf("@easeType == EaseType.LeanTweenType")]
        public LeanTweenType leanTweenType;

        public bool loop;
        public bool useLoopCounts;
        [ShowIf("@loop")]
        public LeanTweenType loopType;
        [ShowIf("@loop && useLoopCounts")]
        public int loopCount;
        public bool returnToOriginalValueOnAnimationEnd;

    }
}