using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Vacuname
{
    
    [RequireComponent(typeof(Rigidbody2D))]
    public class Character : MonoBehaviour
    {
        [TabGroup("�����ļ�"), AssetsOnly, InlineEditor(InlineEditorModes.GUIOnly)]
        [LabelText("�ָ�����"), SerializeField]
        //[Title("@_setAttribute.acceleraTime")]
        public Attribute _setAttribute;//���ļ������õ�����
        //[HideInInspector]public Attribute attribute;//ʵ��ʹ�õ�����

        #region �����˶�������Ҫ�Ĳ���
        private float curAcceleraTime;
        public JumpState jumpState;
        public float moveDirection;
        #endregion
        public Rigidbody2D rd;
        public Animator anima;

        [TabGroup("����"), SerializeField, InlineEditor(InlineEditorModes.GUIOnly)]
        private MMF_Player timeSlowFeedback, timeFastFeedback, dashFeedback;

        private void Awake()
        {
            rd = GetComponent<Rigidbody2D>();
            anima = GetComponent<Animator>();
            curAcceleraTime = 0;
            jumpState = JumpState.fall;
            moveDirection = 1;
        }
        public void Jump()
        {
            if (jumpState == JumpState.ground)
            {
                rd.velocity = new Vector2(rd.velocity.x, _setAttribute.jumpStrength);
                jumpState = JumpState.jump;
            }
        }
        public void Move(float input)
        {
            //��׼��input
            input = input < 0 ? -1 : input > 0 ? 1 : 0;

            //����ı䳯���Ժ�������Ϳ��Բ�Ҫ����ˣ�����ֻ�ǲ�����
            if (input != 0 && moveDirection != input)
            {
                moveDirection = input;
                Vector3 temp = transform.localScale;
                temp.x = moveDirection*Mathf.Abs(temp.x);
                transform.localScale = temp;
            }

            float curSpeed = rd.velocity.x;
            _setAttribute.GetCurSpeed(input, ref curSpeed, ref curAcceleraTime);
            rd.velocity = new Vector2(curSpeed, rd.velocity.y);
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            //TODO �����ж�������ǵ���layer�Ļ�
            jumpState = JumpState.ground;
        }
    }
}

