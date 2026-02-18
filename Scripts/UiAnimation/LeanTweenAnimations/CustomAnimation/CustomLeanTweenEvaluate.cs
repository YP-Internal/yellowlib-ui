using UnityEngine;


namespace YellowPanda.UI
{
    public abstract class CustomLeanTweenEvaluate : MonoBehaviour
    {
        public abstract void Evaluate(float t);
        public abstract float Time { get; protected set; }
    }
}