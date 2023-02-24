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
        [LabelText("�ƶ�����"), SerializeField]
        protected MoveAttribute moveAttribute;

        #region �����õ��¼�����
        private Dictionary<string, UnityAction> eventDic;
        public Dictionary<string, UnityAction> GetEventDic()
        {
            if (eventDic == null) eventDic = new Dictionary<string, UnityAction>();
            return eventDic;
        }
        public void AnimaEvent(string name)
        {
            if (eventDic.ContainsKey(name))
            {
                eventDic[name].Invoke();
            }
        }
        #endregion

        #region �����˶�������Ҫ�Ĳ���
        protected float curAcceleraTime;
        [HideInInspector]public JumpState jumpState;
        #region protected float moveDirection;
        protected float moveDirection;
        public float GetMoveDirection()
        {
            return moveDirection;
        }
        public void SetMoveDirection(float input)
        {
            input.Normalize();
            if (input != 0 && moveDirection != input)
            {
                moveDirection = input;
                Vector3 temp = transform.localScale;
                temp.x = moveDirection * Mathf.Abs(temp.x);
                transform.localScale = temp;
            }
        }
        #endregion
        #endregion

        [HideInInspector]public Rigidbody2D rd;
        [HideInInspector] public Animator anima;

        protected virtual void Awake()
        {
            rd = GetComponent<Rigidbody2D>();
            anima = GetComponent<Animator>();
            curAcceleraTime = 0;
            jumpState = JumpState.fall;
            moveDirection = 1;
        }
        public virtual void Jump()
        {
            if (jumpState == JumpState.ground)
            {
                rd.velocity = new Vector2(rd.velocity.x, moveAttribute.jumpStrength);
                jumpState = JumpState.jump;
            }
        }
        public virtual void Move(float input,bool setDirectly=false)
        {
            float curSpeed;
            if (setDirectly)//ֱ�������ٶȵ����
            {
                curSpeed = input;
                rd.velocity = new Vector2(curSpeed, rd.velocity.y);
                anima.SetFloat("Move", Mathf.Abs(curSpeed));
                return;
            }

            //��׼��input
            input.Normalize();

            if (input != 0 && moveDirection != input)
            {
                moveDirection = input;
                Vector3 temp = transform.localScale;
                temp.x = moveDirection*Mathf.Abs(temp.x);
                transform.localScale = temp;
            }

            curSpeed = rd.velocity.x;
            moveAttribute.GetCurSpeed(input, ref curSpeed, ref curAcceleraTime);
            rd.velocity = new Vector2(curSpeed, rd.velocity.y);

            anima.SetFloat("Move", Mathf.Abs(curSpeed));
        }

        protected void OnCollisionEnter2D(Collision2D collision)
        {
            //TODO �����ж�������ǵ���layer�Ļ�
            jumpState = JumpState.ground;
        }

    }
}

