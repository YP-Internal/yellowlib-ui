using UnityEngine;

namespace YellowPanda.UI
{
    public abstract class UiState : MonoBehaviour
    {
        public abstract void UpdateState(object parameters = null);
    }
}
