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
        [TabGroup("配置文件"), AssetsOnly, InlineEditor(InlineEditorModes.GUIOnly)]
        [LabelText("手感设置"), SerializeField]
        //[Title("@_setAttribute.acceleraTime")]
        public Attribute _setAttribute;//在文件里设置的属性
        //[HideInInspector]public Attribute attribute;//实际使用的属性

        #region 参与运动计算需要的参数
        private float curAcceleraTime;
        public JumpState jumpState;
        public float moveDirection;
        #endregion
        [HideInInspector]public Rigidbody2D rd;
        [HideInInspector] public Animator anima;

        [TabGroup("反馈"), SerializeField, InlineEditor(InlineEditorModes.GUIOnly)]
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
        public void Move(float input,bool setDirectly=false)
        {
            float curSpeed;
            if (setDirectly)//直接设置速度的情况
            {
                curSpeed = input;
                rd.velocity = new Vector2(curSpeed, rd.velocity.y);
                anima.SetFloat("Move", Mathf.Abs(curSpeed));
                return;
            }

            //标准化input
            input = input < 0 ? -1 : input > 0 ? 1 : 0;

            //这个改变朝向以后加入鼠标就可以不要这个了，现在只是测试用
            if (input != 0 && moveDirection != input)
            {
                moveDirection = input;
                Vector3 temp = transform.localScale;
                temp.x = moveDirection*Mathf.Abs(temp.x);
                transform.localScale = temp;
            }

            curSpeed = rd.velocity.x;
            _setAttribute.GetCurSpeed(input, ref curSpeed, ref curAcceleraTime);
            rd.velocity = new Vector2(curSpeed, rd.velocity.y);

            anima.SetFloat("Move", Mathf.Abs(curSpeed));
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            //TODO 加入判定：如果是地面layer的话
            jumpState = JumpState.ground;
        }
    }
}

