using Sirenix.OdinInspector;
using UnityEngine;

namespace YellowPanda.UI.AnimatorUI
{

    public class UiAnimatonWithAnimatorDataSO : OverridableVariableSO<UiAnimatonWithAnimatorData> { }

    [System.Serializable]
    public class UiAnimatonWithAnimatorData
    {
        public ParamaterType paramaterType;
        public string paramaterName;

        [ShowIf("@IsParamaterType(ParamaterType.Float)")] public float floatValue;
        [ShowIf("@IsParamaterType(ParamaterType.Int)")] public int intValue;
        [ShowIf("@IsParamaterType(ParamaterType.Bool)")] public bool boolValue;

        bool IsParamaterType(ParamaterType paramaterType) => this.paramaterType == paramaterType;
        public enum ParamaterType { Float, Int, Trigger, Bool }

    }


}