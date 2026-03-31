using UnityEngine;

namespace YellowPanda.UI.UiManager
{

    public abstract class UiManager : MonoBehaviour
    {
        public static UiManager CurrentManager;
        [SerializeField] bool setUpOnAwake;


        private void Awake()
        {
            if (setUpOnAwake)
                SetUp();
        }

        public void SetUp()
        {
            if (CurrentManager)
                CurrentManager.Disable();

            CurrentManager = this;
        }

        protected void Disable()
        {
            gameObject.SetActive(false);
        }
        protected abstract void OnSetUp();

        void OpenScreen(UiScreen uiScreen)
        {
            uiScreen.Show();
        }
    }

}