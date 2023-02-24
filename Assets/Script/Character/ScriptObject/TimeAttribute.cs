using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
namespace Vacuname
{
    [CreateAssetMenu(fileName = "新角色属性", menuName = "角色/时间控制", order = 1)]
    public class TimeAttribute : ScriptableObject
    {
        [LabelText("时间变慢所需时间")]
        public float slowDownTimer;

        [LabelText("时间加速所需时间")]
        public float speedUpTimer;

        [LabelText("时间变慢倍率")]
        public float slowDownTimeScale;
    }

}


