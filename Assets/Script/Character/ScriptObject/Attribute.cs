using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Vacuname
{
    [CreateAssetMenu(fileName = "新角色属性", menuName = "角色/属性", order = 0)]
    public class Attribute : ScriptableObject
    {
        [VerticalGroup("移动")]
        [LabelText("加速到满需要时间")]
        [TableColumnWidth(200)]
        public float maxAcceleraTime;

        [VerticalGroup("移动")]
        [LabelText("最大速度")] public float maxSpeed;

        [VerticalGroup("移动")]
        [LabelText("跳跃强度")] public float jumpStrength;

        [VerticalGroup("生存")]
        [TableColumnWidth(200)]
        [LabelText("最大生命值")] public float maxHealth;

        public void GetCurSpeed(float input, ref float curSpeed,ref float curAcceleraTime)
        {
            curAcceleraTime = Mathf.Clamp(curAcceleraTime + Time.deltaTime, 0, maxAcceleraTime);
            Debug.Log(curAcceleraTime);
            curSpeed = Mathf.Lerp(0, maxSpeed, curAcceleraTime / maxAcceleraTime);
        }
    }

}

