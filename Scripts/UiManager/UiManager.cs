using System.Collections.Generic;
using UnityEngine;
using YellowPanda.Core;

namespace YellowPanda.UI.UiManager
{

    public abstract class UiManager<UiScreenType> : MonoBehaviour where UiScreenType : UiScreen
    {
        public static UiManager<UiScreenType> CurrentManager;
        [SerializeField] bool setUpOnAwake;
        [SerializeField] bool dontDestroyOnLoad;
        [SerializeField] Transform content;
        protected abstract UiScreenType[] GetUiScreenList();

        Dictionary<string, UiScreenType> uiScreensData = new();
        Dictionary<UiScreenType, UIElement> uiScreens = new();

        UiScreenType currenScreen;
        public UiScreenType CurrenScreen;

        private void Awake()
        {
            if (setUpOnAwake)
                SetUp();
        }

        public void SetUp()
        {
            if (dontDestroyOnLoad)
            {
                if (!CurrentManager)
                    gameObject.AddComponent<DontDestroyOnLoad>();
                else
                    DestroyImmediate(gameObject);

                return;
            }

            OnSetUp();

            CurrentManager = this;

            GenerateUiScreenDataDictionary();
        }
        protected abstract void OnSetUp();
        void GenerateUiScreenDataDictionary()
        {
            var list = GetUiScreenList();

            foreach (var item in list)
                uiScreensData.Add(item.screenId, item);
        }

        public virtual void OpenScreen(string screenId)
        {
            if (screenId == currenScreen.screenId)
                return;

            var uiScreenToShowData = GetUiScreenData(screenId);

            if (!currenScreen.isConstant && !uiScreenToShowData.isPopUp)
            {
                var currentScreenElement = GetUiScreenUiElement(currenScreen);

                currentScreenElement.Hide();
            }

            var uiScreenToShow = GetUiScreenUiElement(uiScreenToShowData);
            uiScreenToShow.Show();

            currenScreen = uiScreenToShowData;
        }

        public UiScreenType GetUiScreenData(string id)
        {
            return uiScreensData[id];
        }

        public UIElement GetUiScreenUiElement(UiScreenType screen)
        {
            if (!uiScreens.ContainsKey(screen))
            {
                var screenData = GetUiScreenData(screen.screenId);
                UIElement element = Instantiate(screenData.prefab, content);

                uiScreens.Add(screen, element);
            }

            return uiScreens[screen];
        }
    }
}