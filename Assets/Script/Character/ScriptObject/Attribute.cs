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
        [LabelText("加速到满需要的时间")]
        [TableColumnWidth(200)]
        public float acceleraTime;
        [VerticalGroup("移动")]
        [LabelText("不控制，减速到0需要的时间")]
        public float deceleraTime;

        [VerticalGroup("移动")]
        [LabelText("最大速度")] 
        public float maxSpeed;

        [VerticalGroup("移动")]
        [LabelText("跳跃强度")] 
        public float jumpStrength;

        [VerticalGroup("移动")]
        [LabelText("冲刺速度")]
        public float dashSpeed;

        [VerticalGroup("移动")]
        [LabelText("冲刺持续时间")]
        public float dashDuration; 

        [VerticalGroup("移动")]
        [LabelText("冲刺冷却时间")]
        public float maxDashCooldown;

        [VerticalGroup("时间控制")]
        [LabelText("时间变慢所需时间")]
        public float slowDownTimer;

        [VerticalGroup("时间控制")]
        [LabelText("时间加速所需时间")]
        public float speedUpTimer;

        [VerticalGroup("时间控制")]
        [LabelText("时间变慢倍率")]
        public float slowDownTimeScale;

        [VerticalGroup("生存")]
        [TableColumnWidth(200)]
        [LabelText("最大生命值")] public float maxHealth;

        public void GetCurSpeed(float input, ref float curSpeed,ref float curAcceleraTime)
        {
            float acceleraDirection=1;//加速进度的方向
            float realLimit=maxSpeed;
            
            if (input == 0)
            {
                acceleraDirection = acceleraTime/deceleraTime*-1;
                realLimit = curSpeed >= 0 ? maxSpeed : -maxSpeed;
                curAcceleraTime = Mathf.Clamp(curAcceleraTime + Time.deltaTime * acceleraDirection, 0, acceleraTime);
                curSpeed = Mathf.Lerp(0, realLimit, curAcceleraTime / acceleraTime);
            }
            else
            {
                if (input * curSpeed < 0)
                {
                    curSpeed = 0;
                    curAcceleraTime = 0;
                }
                else
                {
                    if (input < 0) realLimit *= -1;
                    curAcceleraTime = Mathf.Clamp(curAcceleraTime + Time.deltaTime * acceleraDirection, 0, acceleraTime);
                    curSpeed = Mathf.Lerp(0, realLimit, curAcceleraTime / acceleraTime);
                }
            }
            
        }
    }

}

