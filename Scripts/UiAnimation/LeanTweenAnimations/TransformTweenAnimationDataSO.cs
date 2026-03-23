using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace YellowPanda.UI.LeanTweenUI
{
    public class TransformTweenAnimationDataSO : OverridableVariableSO<TransformTweenAnimationData> { }

    [Serializable]
    public class TransformTweenAnimationData : TweenAnimationData
    {
        [BoxGroup("TransformTween", order: 0)]
        public TransformTweenType tweenType;

        [BoxGroup("TransformTween", order: 0)]
        public Vector3 to;
    }
    public enum TransformTweenType
    {
        Scale,
        Position,
        LocalPosition,
        AnchoredPosition,
        Rotate,
    }
}