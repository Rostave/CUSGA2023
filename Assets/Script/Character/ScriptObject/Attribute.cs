using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Vacuname
{
    [CreateAssetMenu(fileName = "�½�ɫ����", menuName = "��ɫ/����", order = 0)]
    public class Attribute : ScriptableObject
    {
        [VerticalGroup("�ƶ�")]
        [LabelText("���ٵ�����Ҫʱ��")]
        [TableColumnWidth(200)]
        public float maxAcceleraTime;

        [VerticalGroup("�ƶ�")]
        [LabelText("����ٶ�")] public float maxSpeed;

        [VerticalGroup("�ƶ�")]
        [LabelText("��Ծǿ��")] public float jumpStrength;

        [VerticalGroup("����")]
        [TableColumnWidth(200)]
        [LabelText("�������ֵ")] public float maxHealth;

        public void GetCurSpeed(float input, ref float curSpeed,ref float curAcceleraTime)
        {
            float realAccelerate;
            float realLimit;
            if (input < 0) realLimit = maxSpeed * -1;
            //if(input==0)realAccelerate=


            if (curSpeed==0)
            {
                curAcceleraTime = Mathf.Clamp(curAcceleraTime + Time.deltaTime, 0, maxAcceleraTime);
                curSpeed = Mathf.Lerp(0, maxSpeed, curAcceleraTime / maxAcceleraTime);
            }
            else if(input==0)
            {
                
            }
        }
    }

}

