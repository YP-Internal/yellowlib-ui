using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace YellowPanda.UI
{
    public class UIElement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        #region General Settings

        [Title("General Settings")]
        [Tooltip("When true, Set GameObject active to false when finish the hide animation")]
        public bool disableObjectOnHide = true;
        public bool disableObjectOnStart = false;
        [Space(15)]

        #endregion

        #region Behaviors Variables
        [Title("Behaviors")]
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

        [BoxGroup("Show")]
        [ShowIf("@showBehavior")]
        [Button]
        #endregion

        #region Show / Hide Methods
        void Show() => Show(null);
        [BoxGroup("Show")]
        [ShowIf("@showBehavior")]
        [Button]
        public void Show(object parameters = null)
        {
            gameObject.SetActive(true);

            OnShow(parameters);

            if (showBehavior)
            {
                if (showAnimation)
                    showAnimation.Play();
                onShow.Invoke();
            }
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
            OnHide(parameters);
            if (hideBehavior)
            {
                onHide.Invoke();
                if (hideAnimation)
                {
                    if (hideAnimation)
                        hideAnimation.Play();

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
                onPointerDown.Invoke();
                downAnimation?.Play();
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
                onPointerUp.Invoke();
                upAnimation?.Play();
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
                onPointerEnter.Invoke();
                enterAnimation?.Play();
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
                onPointerExit.Invoke();
                exitAnimation?.Play();
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
                clickAnimation?.Play();
            }
        }

        #endregion

        #region Unity Methods
        protected virtual void Start()

        {
            if (disableObjectOnStart)
                gameObject.SetActive(false);
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
    }
}

