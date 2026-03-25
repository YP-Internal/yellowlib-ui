using System;
using UnityEngine;
using YellowPanda.UI;
using YellowPanda.UI.AnimatorUI;
using YellowPanda.UI.LeanTweenUI;

public static class UiAnimationComponentFactory
{
    public enum UiAnimationTypes { None, LeanTweenTransform, LeanTweenColor, LeanTweenCanvasGroup, Animator, Custom }
    static Type GetUiAnimationType(UiAnimationTypes animationType)
    {
        return animationType switch
        {
            UiAnimationTypes.LeanTweenTransform => typeof(UiAnimationTransformLeanTween),
            UiAnimationTypes.LeanTweenColor => typeof(UiAnimationColorLeanTween),
            UiAnimationTypes.LeanTweenCanvasGroup => typeof(UiAnimationCanvasGroupLeanTween),
            UiAnimationTypes.Animator => typeof(UiAnimationWithAnimator),
            _ => throw new ArgumentException($"Unknown animation type: {animationType}")
        };
    }
    /// <summary>
    /// Instantiate a GameObject, if needed in and assign a UIAnimation script to it.
    /// </summary>
    public static void CreateOrSetAnimation(UIElement target, UiAnimationTypes animationType, UIElement.UIBehaviorsEvent category)
    {
        if (animationType == UiAnimationTypes.None)
        {
            target.SetUiAnimation(category, null);
            return;
        }

        if (animationType == UiAnimationTypes.Custom)
        {
            return;
        }

        var gameObjectName = $"{category} - {animationType}";

        if (!target.animationObjectHolder)
        {
            target.animationObjectHolder = new GameObject("Animations");
            target.animationObjectHolder.transform.SetParent(target.transform);
            target.animationObjectHolder.transform.localPosition = Vector3.zero;
        }

        Transform animationObject = target.animationObjectHolder.transform.Find(gameObjectName);

        UiAnimation animationToAdd;

        if (animationObject == null)
        {
            animationObject = new GameObject(gameObjectName).transform;
            animationObject.SetParent(target.animationObjectHolder.transform);
            animationObject.transform.localPosition = Vector3.zero;
        }

        if (animationObject.TryGetComponent(out UiAnimation animation))
            animationToAdd = animation;
        else
        {
            Type componentType = GetUiAnimationType(animationType);
            animationToAdd = animationObject.gameObject.AddComponent(componentType) as UiAnimation;
            animationToAdd.Init(target);
        }

        animationToAdd.gameObject.SetActive(true);

        target.SetUiAnimation(category, animationToAdd);
    }

}
