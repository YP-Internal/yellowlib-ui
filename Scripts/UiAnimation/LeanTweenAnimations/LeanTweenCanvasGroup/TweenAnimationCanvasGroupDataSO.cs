using UnityEngine;
using YellowPanda.UI.LeanTweenUI;

namespace YellowPanda.UI.LeanTweenUI
{
    public class TweenAnimationCanvasGroupDataSO : OverridableVariableSO<TweenAnimationCanvasGroupData>
    {
        protected CanvasGroup target;
    }

    [System.Serializable]
    public class TweenAnimationCanvasGroupData : TweenAnimationData
    {
        [Range(0, 1)] public float finalAlpha;
        [Range(0, 1)] public float startAlpha;
    }

}