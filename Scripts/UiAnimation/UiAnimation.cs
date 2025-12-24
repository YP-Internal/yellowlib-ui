using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace YellowPanda.UI
{
    public abstract class UiAnimation : MonoBehaviour
    {
        protected const string GENERAL_SETTINGS = "General Settings";
        protected const string EVENTS = "Events";
        protected const string ANIMATION_SETTINGS = "Animation Settings";



        [FoldoutGroup(GENERAL_SETTINGS)]
        [SerializeField] protected float delay = 0;
        public abstract float AnimationTime { get; }
        public abstract bool IsPlaying { get; }

        [FoldoutGroup(EVENTS,3)]
        public UnityEvent onPlayAnimation;
        [FoldoutGroup(EVENTS,3)]
        public UnityEvent onStopAnimation;

        IEnumerator animationDelayCoroutine;
        public void Play()
        {
            animationDelayCoroutine = AnimationDelayCoroutine();
            StartCoroutine(animationDelayCoroutine);
        }
        public void Stop()
        {
            if (!IsPlaying) return;

            if (animationDelayCoroutine != null)
            {
                StopCoroutine(animationDelayCoroutine);
                animationDelayCoroutine = null;
            }
            onStopAnimation?.Invoke();
            StopAnimation();
        }

        IEnumerator AnimationDelayCoroutine()
        {
            yield return new WaitForSeconds(delay);
            PlayAnimation();
            onPlayAnimation?.Invoke();
            animationDelayCoroutine = null;
        }

        protected virtual bool CanInspectorPlay => true;
        protected virtual bool CanInspectorStop => IsPlaying;

        [FoldoutGroup(ANIMATION_SETTINGS)]
        [Button]
        [ShowIf("CanInspectorPlay")]
        public abstract void PlayAnimation();

        [FoldoutGroup(ANIMATION_SETTINGS)]
        [Button]
        [ShowIf("CanInspectorStop")]
        public abstract void StopAnimation();
        public abstract void Init(UIElement target);
        public abstract void CreateAnimationData();
    }
}
