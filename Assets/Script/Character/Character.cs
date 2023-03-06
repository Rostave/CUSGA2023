using System;
using Chronos;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using R0.Static;
using UnityEngine;
using UnityEngine.Events;

namespace Vacuname
{
    
    [RequireComponent(typeof(Rigidbody2D))]
    public class Character : MonoBehaviour
    {
        [TabGroup("配置"), AssetsOnly, InlineEditor(InlineEditorModes.GUIOnly)]
        [LabelText("移动设置"), SerializeField]
        protected MoveAttribute moveAttribute;
        protected Timeline time;//有问题，在编辑器编辑提示什么ASSET，不过暂时没出毛病

        #region 动画用事件字典
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

        #region 反馈字典
        [TabGroup("反馈"), InlineEditor(InlineEditorModes.GUIOnly)]
        public FeedbackDictionary feedbacks;
        #endregion

        #region 参与运动计算需要的参数
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
        [HideInInspector]public Animator anima;
        [TabGroup("配置")]
        // public SkeletonAnimation s_anima;
        public SkeletonMecanim sm_anima;

        protected virtual void Awake()
        {
            time = GetComponent<Timeline>();
            rd = GetComponent<Rigidbody2D>();
            TryGetComponent<Animator>(out anima);
            if (anima == null) transform.Find("Spine").TryGetComponent<Animator>(out anima);
            curAcceleraTime = 0;
            jumpState = JumpState.fall;
            moveDirection = 1;
        }
        public virtual void Jump()
        {
            if (jumpState == JumpState.ground)
            {
                time.rigidbody2D.velocity = new Vector2(time.rigidbody2D.velocity.x, moveAttribute.jumpStrength);
                jumpState = JumpState.jump;
            }
        }
        public virtual void Move(float input,bool setDirectly=false)
        {
            float curSpeed;
            if (setDirectly)//直接设置速度的情况
            {
                curSpeed = input;

                time.rigidbody2D.velocity = new Vector2(curSpeed, time.rigidbody2D.velocity.y);
                anima?.SetFloat("Move", Mathf.Abs(curSpeed));
                return;
            }

            //标准化input
            input.Normalize();

            if (input != 0 && Math.Abs(moveDirection - input) > Const.IdleTolerance)
            {
                moveDirection = input;
                Vector3 temp = transform.localScale;
                temp.x = moveDirection*Mathf.Abs(temp.x);
                transform.localScale = temp;
            }

            curSpeed = time.rigidbody2D.velocity.x;
            moveAttribute.GetCurSpeed(input, ref curSpeed, ref curAcceleraTime);
            time.rigidbody2D.velocity = new Vector2(curSpeed, time.rigidbody2D.velocity.y);

            anima?.SetFloat("Move", Mathf.Abs(curSpeed));
        }

        protected void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer==LayerMask.NameToLayer("Ground"))
                if (collision.transform.position.y < transform.position.y)
                {
                    jumpState = JumpState.ground;
                }
        }

    }
}
