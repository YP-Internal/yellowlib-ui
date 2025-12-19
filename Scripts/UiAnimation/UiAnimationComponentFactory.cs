using System;
using Codice.CM.SEIDInfo;
using UnityEditorInternal;
using UnityEngine;
using YellowPanda.UI;

public static class UiAnimationComponentFactory
{
    public enum UiAnimationTypes { None, LeanTween }
    static Type GetUiAnimationType(UiAnimationTypes animationType)
    {
        return animationType switch
        {
            UiAnimationTypes.LeanTween => typeof(UiAnimationTransformLeanTween),
            _ => throw new ArgumentException($"Unknown animation type: {animationType}")
        };
    }
    public static void CreateAnimation(UIElement target, UiAnimationTypes animationType, UIElement.UIBehaviorsEvent category)
    {
        if (animationType == UiAnimationTypes.None)
        {
            target.SetUiAnimation(category, null);
            return;
        }

        var gameObjectName = $"{category} - {animationType}";

        if (!target.animationObjectHolder)
        {
            target.animationObjectHolder = new GameObject($"{target.name} Animation Holder");
            target.animationObjectHolder.transform.SetParent(target.transform);
        }


        Transform animationObject = target.animationObjectHolder.transform.Find(gameObjectName);

        UiAnimation animationToAdd;

        if (animationObject == null)
        {
            animationObject = new GameObject(gameObjectName).transform;
            animationObject.SetParent(target.animationObjectHolder.transform);
        }

        if (animationObject.TryGetComponent(out UiAnimation animation))
            animationToAdd = animation;
        else
        {
            Type componentType = GetUiAnimationType(animationType);
            animationToAdd = animationObject.gameObject.AddComponent(componentType) as UiAnimation;
            animationToAdd.Init(target);
        }

        target.SetUiAnimation(category, animationToAdd);
    }
}
