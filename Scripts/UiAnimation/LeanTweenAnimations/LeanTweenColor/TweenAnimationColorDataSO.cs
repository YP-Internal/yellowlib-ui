using UnityEngine;

namespace YellowPanda.UI.LeanTweenUI
{
    public class TweenAnimationColorDataSO : OverridableVariableSO<TweenAnimationColorData> { }

    [System.Serializable]
    public class TweenAnimationColorData : TweenAnimationData
    {
        public bool useGradient;
        public Gradient gradient;
        public Color from;
        public Color to;
    }
}