using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Vacuname
{
    [CreateAssetMenu(fileName = "�½�ɫ����", menuName = "��ɫ/����", order = 0)]
    public class Attribute : ScriptableObject
    {
        [FoldoutGroup("�ƶ�")]
        [LabelText("���ٵ�����Ҫ��ʱ��")]
        [TableColumnWidth(200)]
        public float acceleraTime;
        [FoldoutGroup("�ƶ�")]
        [LabelText("�����ƣ����ٵ�0��Ҫ��ʱ��")]
        public float deceleraTime;

        [FoldoutGroup("�ƶ�")]
        [LabelText("����ϵ��")]
        public float gravity;

        [FoldoutGroup("�ƶ�")]
        [LabelText("����ٶ�")] 
        public float maxSpeed;

        [FoldoutGroup("�ƶ�")]
        [LabelText("��Ծǿ��")] 
        public float jumpStrength;

        [FoldoutGroup("�ƶ�")]
        [LabelText("����ٶ�")]
        public float dashSpeed;

        [FoldoutGroup("�ƶ�")]
        [LabelText("��̳���ʱ��")]
        public float dashDuration; 

        [FoldoutGroup("�ƶ�")]
        [LabelText("�����ȴʱ��")]
        public float maxDashCooldown;

        [FoldoutGroup("ʱ�����")]
        [LabelText("ʱ���������ʱ��")]
        public float slowDownTimer;

        [FoldoutGroup("ʱ�����")]
        [LabelText("ʱ���������ʱ��")]
        public float speedUpTimer;

        [FoldoutGroup("ʱ�����")]
        [LabelText("ʱ���������")]
        public float slowDownTimeScale;

        public void GetCurSpeed(float input, ref float curSpeed,ref float curAcceleraTime)
        {
            float acceleraDirection=1;//���ٽ��ȵķ���
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

