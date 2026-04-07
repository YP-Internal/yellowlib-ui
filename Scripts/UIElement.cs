using System.Collections;
using Sirenix.OdinInspector;
using UnityEditor;
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
        [FoldoutGroup(GENERAL_SETTINGS)]
        public bool showElementObEnable = false;

        public bool IsShowing { get; private set; }

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
#if UNITY_EDITOR
                EditorApplication.delayCall += SetAnimationDelayedCall;
#endif
            }
            void SetAnimationDelayedCall()
            {
                UiAnimationComponentFactory.CreateOrSetAnimation(owner, animationType, behavior);
#if UNITY_EDITOR
                EditorApplication.delayCall -= SetAnimationDelayedCall;
#endif
            }

            [OnValueChanged(nameof(SetAnimation))]
            public UiAnimationComponentFactory.UiAnimationTypes animationType;
            [InlineEditor, ShowIf(nameof(IsValidAnimation))] public UiAnimation animation;
            bool IsValidAnimation => animationType != UiAnimationComponentFactory.UiAnimationTypes.None;

            [SerializeField, HorizontalGroup("Event", Width = 80), ToggleLeft] bool useEvent;
            [HorizontalGroup("Event"), ShowIf(nameof(useEvent))]
            public UnityEvent onEvent;
        }
        void UpdateShowEvent()
        {
            UpdateEventState(showEvent, showSettings);
            UpdateEventState(hideEvent, hideSettings);
            UpdateEventState(clickEvent, clickSettings);
            UpdateEventState(downEvent, downSettings);
            UpdateEventState(upEvent, upSettings);
            UpdateEventState(enterEvent, enterSettings);
            UpdateEventState(exitEvent, exitSettings);
        }
        void UpdateEventState(bool enabled, UIEventSettings settings)
        {
            if (settings == null || settings.animation == null)
                return;

            settings.animation.gameObject.SetActive(enabled);
        }
        [ToggleLeft, OnValueChanged(nameof(UpdateShowEvent))] public bool showEvent;
        [ShowIf(nameof(showEvent))]
        [BoxGroup("Show")] public UIEventSettings showSettings = new UIEventSettings();
        [ToggleLeft, OnValueChanged(nameof(UpdateShowEvent))] public bool hideEvent;
        [ShowIf(nameof(hideEvent))]
        [BoxGroup("Hide")] public UIEventSettings hideSettings = new UIEventSettings();
        [ToggleLeft, OnValueChanged(nameof(UpdateShowEvent))] public bool clickEvent;
        [ShowIf(nameof(clickEvent))]
        [BoxGroup("Click")] public UIEventSettings clickSettings = new UIEventSettings();
        [ToggleLeft, OnValueChanged(nameof(UpdateShowEvent))] public bool downEvent;
        [ShowIf(nameof(downEvent))]
        [BoxGroup("Down")] public UIEventSettings downSettings = new UIEventSettings();
        [ToggleLeft, OnValueChanged(nameof(UpdateShowEvent))] public bool upEvent;
        [ShowIf(nameof(upEvent))]
        [BoxGroup("Up")] public UIEventSettings upSettings = new UIEventSettings();
        [ToggleLeft, OnValueChanged(nameof(UpdateShowEvent))] public bool enterEvent;
        [ShowIf(nameof(enterEvent))]
        [BoxGroup("Enter")] public UIEventSettings enterSettings = new UIEventSettings();
        [ToggleLeft, OnValueChanged(nameof(UpdateShowEvent))] public bool exitEvent;
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

        [BoxGroup("Show")]
        [ShowIf(nameof(showEvent))]
        [Button]
        public void Show()
        {
            showSettings.onEvent?.Invoke();

            if (!gameObject.activeSelf)
                gameObject.SetActive(true);

            if (showEvent)
            {
                PlayAnimation(showSettings.animation);
            }

            IsShowing = true;
            OnShow();
        }

        [BoxGroup("Hide")]
        [ShowIf(nameof(hideEvent))]

        [BoxGroup("Hide")]
        [ShowIf(nameof(hideEvent))]
        [Button]
        public void Hide()
        {
            hideSettings.onEvent?.Invoke();

            if (hideEvent)
            {
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

            IsShowing = false;
            OnHide();
        }

        void DisableObjectWhenHide()
        {
            gameObject.SetActive(false);
            if (hideSettings.animation)
                hideSettings.animation.onStopAnimation.RemoveListener(DisableObjectWhenHide);
        }

        virtual protected void OnShow() { }
        virtual protected void OnHide() { }
        #endregion

        #region Pointer Events

        [BoxGroup("Down")]
        [ShowIf(nameof(downEvent))]
        [Button]
        public void PointerDown() => OnPointerDown(null);
        public void OnPointerDown(PointerEventData eventData)
        {
            downSettings.onEvent?.Invoke();
            if (downEvent)
            {
                PlayAnimation(downSettings.animation);
            }
        }

        [BoxGroup("Up")]
        [ShowIf(nameof(upEvent))]
        [Button]
        public void PointerUp() => OnPointerUp(null);
        public void OnPointerUp(PointerEventData eventData)
        {
            upSettings.onEvent?.Invoke();
            if (upEvent)
            {
                PlayAnimation(upSettings.animation);
            }
        }
        [BoxGroup("Enter")]
        [ShowIf(nameof(enterEvent))]

        [Button]
        public void PointerEnter() => OnPointerEnter(null);
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
        public void PointerExit() => OnPointerExit(null);
        public void OnPointerExit(PointerEventData eventData)
        {
            exitSettings.onEvent?.Invoke();

            if (exitEvent)
            {
                PlayAnimation(exitSettings.animation);
            }
        }

        [BoxGroup("Click")]

        [ShowIf(nameof(clickEvent))]
        [Button]
        public void PointerClick() => OnPointerClick(null);

        public void OnPointerClick(PointerEventData eventData)
        {
            clickSettings.onEvent?.Invoke();
            if (clickEvent)
            {
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
            if (showElementObEnable)
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
