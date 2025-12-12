using UnityEngine;
using UnityEngine.Events;

namespace YellowPanda.UI
{
    public abstract class UiAnimation : MonoBehaviour
    {
        public UnityEvent onPlayAnimation;
        public UnityEvent onStopAnimation;

        public abstract void Play();
        public abstract void Stop();
    }
}
