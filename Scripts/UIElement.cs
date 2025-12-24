using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace YellowPanda.UI
{
    public class UIElement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public enum UIBehaviorsEvent { Show, Hide, Click, Down, Up, Enter, Exit }

        #region General Settings
        const string GENERAL_SETTINGS = "General Settings";
        [FoldoutGroup(GENERAL_SETTINGS)]
        [Tooltip("Object That keep the object animations. Is used when a new animation is automatic created")]
        public GameObject animationObjectHolder;

        [FoldoutGroup(GENERAL_SETTINGS)]
        [Tooltip("When true, Set GameObject active to false when finish the hide animation")]
        public bool disableObjectOnHide = true;

        [FoldoutGroup(GENERAL_SETTINGS)]
        [Tooltip("When true, automatically disables the object on start")]
        public bool disableObjectOnStart = false;

        #endregion

        #region Behaviors Variables
        private void OnValidate()
        {
            showSettings.owner = this; showSettings.behavior = UIBehaviorsEvent.Show;
            hideSettings.owner = this; hideSettings.behavior = UIBehaviorsEvent.Hide;
            clickSettings.owner = this; clickSettings.behavior = UIBehaviorsEvent.Click;
            downSettings.owner = this; downSettings.behavior = UIBehaviorsEvent.Down;
            upSettings.owner = this; upSettings.behavior = UIBehaviorsEvent.Up;
            enterSettings.owner = this; enterSettings.behavior = UIBehaviorsEvent.Enter;
            exitSettings.owner = this; exitSettings.behavior = UIBehaviorsEvent.Exit;
        }

        [System.Serializable]
        public class UIEventSettings
        {
            [HideInInspector] public UIElement owner;
            [HideInInspector] public UIBehaviorsEvent behavior;

            void SetAnimation()
            {
                UiAnimationComponentFactory.CreateAnimation(owner, animationType, behavior);
            }
            [OnValueChanged(nameof(SetAnimation))]
            public UiAnimationComponentFactory.UiAnimationTypes animationType;
            [InlineEditor] public UiAnimation animation;

            public UnityEvent onEvent;
        }

        [ToggleLeft] public bool showEvent;
        [ShowIf(nameof(showEvent))]
        [BoxGroup("Show")] public UIEventSettings showSettings = new UIEventSettings();
        [ToggleLeft] public bool hideEvent;
        [ShowIf(nameof(hideEvent))]
        [BoxGroup("Hide")] public UIEventSettings hideSettings = new UIEventSettings();
        [ToggleLeft] public bool clickEvent;
        [ShowIf(nameof(clickEvent))]
        [BoxGroup("Click")] public UIEventSettings clickSettings = new UIEventSettings();
        [ToggleLeft] public bool downEvent;
        [ShowIf(nameof(downEvent))]
        [BoxGroup("Down")] public UIEventSettings downSettings = new UIEventSettings();
        [ToggleLeft] public bool upEvent;
        [ShowIf(nameof(upEvent))]
        [BoxGroup("Up")] public UIEventSettings upSettings = new UIEventSettings();
        [ToggleLeft] public bool enterEvent;
        [ShowIf(nameof(enterEvent))]
        [BoxGroup("Enter")] public UIEventSettings enterSettings = new UIEventSettings();
        [ToggleLeft] public bool exitEvent;
        [ShowIf(nameof(exitEvent))]
        [BoxGroup("Exit")] public UIEventSettings exitSettings = new UIEventSettings();

        public UiAnimation GetUiAnimation(UIBehaviorsEvent uiBehaviorsEvent)
        {
            return uiBehaviorsEvent switch
            {
                UIBehaviorsEvent.Show => showSettings.animation,
                UIBehaviorsEvent.Hide => hideSettings.animation,
                UIBehaviorsEvent.Click => clickSettings.animation,
                UIBehaviorsEvent.Down => downSettings.animation,
                UIBehaviorsEvent.Up => upSettings.animation,
                UIBehaviorsEvent.Enter => enterSettings.animation,
                UIBehaviorsEvent.Exit => exitSettings.animation,
                _ => null,
            };
        }

        public void SetUiAnimation(UIBehaviorsEvent uiBehaviorsEvent, UiAnimation animation)
        {
            var settings = GetAnimationSettings(uiBehaviorsEvent);
            if (settings.animation)
                settings.animation.gameObject.SetActive(false);
            settings.animation = animation;
        }

        public UIEventSettings GetAnimationSettings(UIBehaviorsEvent uiBehaviorsEvent)
        {
            return uiBehaviorsEvent switch
            {
                UIBehaviorsEvent.Show => showSettings,
                UIBehaviorsEvent.Hide => hideSettings,
                UIBehaviorsEvent.Click => clickSettings,
                UIBehaviorsEvent.Down => downSettings,
                UIBehaviorsEvent.Up => upSettings,
                UIBehaviorsEvent.Enter => enterSettings,
                UIBehaviorsEvent.Exit => exitSettings,
                _ => throw new System.NotImplementedException(),
            };
        }

        #endregion

        #region Show / Hide Methods

        [BoxGroup("Show")]
        [ShowIf(nameof(showEvent))]
        [Button]
        void Show() => Show(null);

        [BoxGroup("Show")]
        [Tooltip("Call Show(object parameters = null) with custom paramaters")]
        [ShowIf(nameof(showEvent))]
        [Button]
        public void Show(object parameters = null)
        {
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);

            if (showEvent)
            {
                PlayAnimation(showSettings.animation);
                showSettings.onEvent?.Invoke();
            }

            OnShow(parameters);
        }

        [BoxGroup("Hide")]
        [ShowIf(nameof(hideEvent))]
        [Button]
        void Hide() => Hide(null);

        [BoxGroup("Hide")]
        [ShowIf(nameof(hideEvent))]
        [Tooltip("Call Hide(object parameters = null) with custom paramaters")]
        [Button]
        public void Hide(object parameters = null)
        {
            if (hideEvent)
            {
                hideSettings.onEvent?.Invoke();
                if (hideSettings.animation)
                {
                    PlayAnimation(hideSettings.animation);

                    if (disableObjectOnHide)
                        hideSettings.animation.onStopAnimation.AddListener(DisableObjectWhenHide);
                }
                else
                {
                    if (disableObjectOnHide)
                        gameObject.SetActive(false);
                }
            }
            else
            {
                if (disableObjectOnHide)
                    gameObject.SetActive(false);
            }

            OnHide(parameters);
        }

        void DisableObjectWhenHide()
        {
            gameObject.SetActive(false);
            if (hideSettings.animation)
                hideSettings.animation.onStopAnimation.RemoveListener(DisableObjectWhenHide);
        }

        virtual protected void OnShow(object parameters = null) { }
        virtual protected void OnHide(object parameters = null) { }
        #endregion

        #region Pointer Events

        [BoxGroup("Down")]
        [ShowIf(nameof(downEvent))]
        [Button]
        void PointerDown() => OnPointerDown(null);
        public void OnPointerDown(PointerEventData eventData)
        {
            if (downEvent)
            {
                PlayAnimation(downSettings.animation);
                downSettings.onEvent?.Invoke();
            }
        }

        [BoxGroup("Up")]
        [ShowIf(nameof(upEvent))]
        [Button]
        void PointerUp() => OnPointerUp(null);
        public void OnPointerUp(PointerEventData eventData)
        {
            if (upEvent)
            {
                PlayAnimation(upSettings.animation);
                upSettings.onEvent?.Invoke();
            }
        }
        [BoxGroup("Enter")]
        [ShowIf(nameof(enterEvent))]

        [Button]
        void PointerEnter() => OnPointerEnter(null);
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (enterEvent)
            {
                PlayAnimation(enterSettings.animation);
                enterSettings.onEvent?.Invoke();
            }
        }
        [BoxGroup("Exit")]
        [ShowIf(nameof(exitEvent))]

        [Button]
        void PointerExit() => OnPointerExit(null);
        public void OnPointerExit(PointerEventData eventData)
        {
            if (exitEvent)
            {
                PlayAnimation(exitSettings.animation);
                exitSettings.onEvent?.Invoke();
            }
        }

        [BoxGroup("Click")]

        [ShowIf(nameof(clickEvent))]
        [Button]
        void PointerClick() => OnPointerClick(null);

        public void OnPointerClick(PointerEventData eventData)
        {
            if (clickEvent)
            {
                clickSettings.onEvent?.Invoke();
                PlayAnimation(clickSettings.animation);
            }
        }

        #endregion

        #region Unity Methods
        protected virtual void Start()
        {
            if (disableObjectOnStart)
                gameObject.SetActive(false);
        }
        private void OnEnable()
        {
            Show();
        }
        #endregion

        #region Ui State
        [Space(15)]
        [Title("UI State")]
        [SerializeField] UiState uiState;
        [Button]
        public void UpdateState(object parameters)
        {
            uiState.UpdateState(parameters);
        }
        #endregion

        void PlayAnimation(UiAnimation animation)
        {
            if (animation)
            {
                StopAllAnimation();
                animation.Play();
            }
        }
        void StopAllAnimation()
        {
            showSettings.animation?.Stop();
            hideSettings.animation?.Stop();

            clickSettings.animation?.Stop();

            enterSettings.animation?.Stop();
            exitSettings.animation?.Stop();

            downSettings.animation?.Stop();
            upSettings.animation?.Stop();
        }

    }
}
