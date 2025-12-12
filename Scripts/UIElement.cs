using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace YellowPanda.UI
{
    public class UIElement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        #region General Settings
        const string GENERAL_SETTINGS = "General Settings";
        [FoldoutGroup(GENERAL_SETTINGS)]
        [Tooltip("When true, Set GameObject active to false when finish the hide animation")]
        public bool disableObjectOnHide = true;
        [FoldoutGroup(GENERAL_SETTINGS)]
        public bool disableObjectOnStart = false;
        [Space(15)]

        #endregion

        #region Behaviors Variables
        [ToggleLeft] public bool showBehavior;
        [BoxGroup("Show")]
        [ShowIf("@showBehavior")] public UiAnimation showAnimation;
        [BoxGroup("Show")]
        [ShowIf("@showBehavior")] public UnityEvent onShow;


        [ToggleLeft] public bool hideBehavior;
        [BoxGroup("Hide")]
        [ShowIf("@hideBehavior")] public UiAnimation hideAnimation;
        [BoxGroup("Hide")]
        [ShowIf("@hideBehavior")] public UnityEvent onHide;

        [ToggleLeft] public bool clickBehavior;
        [BoxGroup("Click")]
        [ShowIf("@clickBehavior")] public UiAnimation clickAnimation;
        [BoxGroup("Click")]
        [ShowIf("@clickBehavior")] public UnityEvent onPointerClick;
        [ToggleLeft] public bool downBehavior;
        [BoxGroup("Down")]
        [ShowIf("@downBehavior")] public UiAnimation downAnimation;
        [BoxGroup("Down")]
        [ShowIf("@downBehavior")] public UnityEvent onPointerDown;

        [ToggleLeft] public bool upBehavior;
        [BoxGroup("Up")]
        [ShowIf("@upBehavior")] public UiAnimation upAnimation;
        [BoxGroup("Up")]
        [ShowIf("@upBehavior")] public UnityEvent onPointerUp;

        [ToggleLeft] public bool enterBehavior;
        [BoxGroup("Enter")]
        [ShowIf("@enterBehavior")] public UiAnimation enterAnimation;
        [BoxGroup("Enter")]
        [ShowIf("@enterBehavior")] public UnityEvent onPointerEnter;

        [ToggleLeft] public bool exitBehavior;
        [BoxGroup("Exit")]
        [ShowIf("@exitBehavior")] public UiAnimation exitAnimation;
        [BoxGroup("Exit")]
        
        [ShowIf("@exitBehavior")] public UnityEvent onPointerExit;


        #endregion

        #region Show / Hide Methods
        
        [BoxGroup("Show")]
        [ShowIf("@showBehavior")]
        [Button]
        void Show() => Show(null);
        
        [BoxGroup("Show")]
        [ShowIf("@showBehavior")]
        [Button]
        public void Show(object parameters = null)
        {
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);

            if (showBehavior)
            {
                PlayAnimation(showAnimation);

                onShow.Invoke();
            }

            OnShow(parameters);
        }
        
        [BoxGroup("Hide")]
        [ShowIf("@hideBehavior")]
        [Button]
        void Hide() => Hide(null);
        
        [BoxGroup("Hide")]
        [ShowIf("@hideBehavior")]
        [Button]
        public void Hide(object parameters = null)
        {
            if (hideBehavior)
            {
                onHide.Invoke();
                if (hideAnimation)
                {
                    PlayAnimation(hideAnimation);

                    if (disableObjectOnHide)
                        hideAnimation.onStopAnimation.AddListener(DisableObjectWhenHide);
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
            hideAnimation.onStopAnimation.RemoveListener(DisableObjectWhenHide);
        }

        virtual protected void OnShow(object parameters = null) { }
        virtual protected void OnHide(object parameters = null) { }
        #endregion

        #region Pointer Events
        
        [BoxGroup("Down")]
        [ShowIf("@" + nameof(downBehavior))]
        [Button]
        void PointerDown() => OnPointerDown(null);
        public void OnPointerDown(PointerEventData eventData)
        {
            if (downBehavior)
            {
                PlayAnimation(downAnimation);
                onPointerDown.Invoke();
            }
        }
        
        [BoxGroup("Up")]
        [ShowIf("@" + nameof(upBehavior))]
        [Button]
        void PointerUp() => OnPointerUp(null);
        public void OnPointerUp(PointerEventData eventData)
        {
            if (upBehavior)
            {
                PlayAnimation(upAnimation);
                onPointerUp.Invoke();
            }
        }
        [BoxGroup("Enter")]
        [ShowIf("@" + nameof(enterBehavior))]
        
        [Button]
        void PointerEnter() => OnPointerEnter(null);
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (enterBehavior)
            {
                PlayAnimation(enterAnimation);
                onPointerEnter.Invoke();
            }
        }
        [BoxGroup("Exit")]
        [ShowIf("@" + nameof(exitBehavior))]
        
        [Button]
        void PointerExit() => OnPointerExit(null);
        public void OnPointerExit(PointerEventData eventData)
        {
            if (exitBehavior)
            {
                PlayAnimation(exitAnimation);
                onPointerExit.Invoke();
            }
        }

        [BoxGroup("Click")]
        
        [ShowIf("@" + nameof(clickBehavior))]
        [Button]
        void PointerClick() => OnPointerClick(null);

        public void OnPointerClick(PointerEventData eventData)
        {
            if (clickBehavior)
            {
                onPointerClick.Invoke();
                PlayAnimation(clickAnimation);
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
            showAnimation?.Stop();
            hideAnimation?.Stop();

            clickAnimation?.Stop();

            enterAnimation?.Stop();
            exitAnimation?.Stop();

            downAnimation?.Stop();
            upAnimation?.Stop();
        }
    }
}

