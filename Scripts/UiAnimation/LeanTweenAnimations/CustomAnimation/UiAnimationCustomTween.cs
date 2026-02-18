using UnityEngine;

namespace YellowPanda.UI
{
    public class UiAnimationCustomTween : UiAnimation
    {
        public override float AnimationTime => customEvaluate.Time;
        public override bool IsPlaying => isPlaying;
        bool isPlaying;
        [SerializeField] CustomLeanTweenEvaluate customEvaluate;

        public override void CreateAsset(string path)
        {
            throw new System.NotImplementedException();
        }

        public override void Init(UIElement target) { }
        public override void PlayAnimation()
        {
            isPlaying = true;

            LeanTween.value(gameObject, 0f, 1f, AnimationTime)
                .setOnUpdate((float val) =>
                {
                    customEvaluate.Evaluate(val);
                })
                .setOnComplete(() =>
                {
                    Stop();
                });
        }

        public override void StopAnimation()
        {
            isPlaying = false;
            LeanTween.cancel(gameObject);
        }
    }
}