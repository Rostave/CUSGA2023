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
            curAcceleraTime = Mathf.Clamp(curAcceleraTime + Time.deltaTime, 0, maxAcceleraTime);
            Debug.Log(curAcceleraTime);
            curSpeed = Mathf.Lerp(0, maxSpeed, curAcceleraTime / maxAcceleraTime);
        }
    }

}

