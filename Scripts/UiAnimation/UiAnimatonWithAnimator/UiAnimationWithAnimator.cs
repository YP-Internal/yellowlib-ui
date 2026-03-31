using UnityEngine;
using YellowPanda.AssetCreation;

namespace YellowPanda.UI.AnimatorUI
{
    public class UiAnimationWithAnimator : UiAnimation
    {
        [SerializeField] Animator animator;
        [SerializeField] OverridableVariable<UiAnimatonWithAnimatorData, UiAnimatonWithAnimatorDataSO> data;
        public override float AnimationTime => animator.GetCurrentAnimatorClipInfo(0).Length;

        public override bool IsPlaying => animator.gameObject.activeInHierarchy;

        public override void Init(UIElement target)
        {
            target.TryGetComponent(out animator);
        }

        public override void PlayAnimation()
        {
            var _data = data.Value;

            switch (_data.paramaterType)
            {
                case UiAnimatonWithAnimatorData.ParamaterType.Float:
                    animator.SetFloat(_data.paramaterName, _data.floatValue);
                    break;
                case UiAnimatonWithAnimatorData.ParamaterType.Int:
                    animator.SetInteger(_data.paramaterName, _data.intValue);
                    break;
                case UiAnimatonWithAnimatorData.ParamaterType.Trigger:
                    animator.SetTrigger(_data.paramaterName);
                    break;
                case UiAnimatonWithAnimatorData.ParamaterType.Bool:
                    animator.SetBool(_data.paramaterName, _data.boolValue);
                    break;
            }
        }

        public override void StopAnimation() { }
    }
}
