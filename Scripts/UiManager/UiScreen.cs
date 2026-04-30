using UnityEngine;

namespace YellowPanda.UI.UiManager
{
    [CreateAssetMenu(menuName = "YellowLib/UiScreen")]
    public class UiScreen : ScriptableObject
    {
        public string screenId;
        public UIElement prefab;
        [Tooltip("Don't disable the current screen when opened")]
        public bool isPopUp;
        [Tooltip("After open, dont close if other screen is oppened. You can close with a custom implementation of UiManager")]
        public bool isConstant;
    }
}
