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
        private float moveDirection;
        private bool canDash;
        private bool dashing;
        private float dashColdDown;
        #endregion
        public Rigidbody2D rd;
        public Animator anima;

        [TabGroup("反馈"), SerializeField, InlineEditor(InlineEditorModes.GUIOnly)]
        private MMF_Player timeSlowFeedback, timeFastFeedback, dashFeedback;

        #region 初始绑定

        private void Awake()
        {
            rd = GetComponent<Rigidbody2D>();
            anima = GetComponent<Animator>();
            curAcceleraTime = 0;
            jumpState = JumpState.fall;
            canDash = true;
            dashing = false;
            moveDirection = 1;
            dashColdDown = 0;
        }

        private void Start()
        {
            CameraControl.Instance.ca.m_Follow = transform;
        }

        private void OnDisable()
        {
            //input.onMove -= Move;
        }
        #endregion

        private void Update()
        {

        }
        private void ControlTime()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                timeSlowFeedback?.PlayFeedbacks();
                TimeControl.Instance.SetTimeScale(_setAttribute.slowDownTimeScale, _setAttribute.slowDownTimer);
            }
            if (Input.GetKeyUp(KeyCode.Tab))
            {
                timeFastFeedback?.PlayFeedbacks();
                TimeControl.Instance.SetTimeScale(1f, _setAttribute.speedUpTimer);
            }
        }
        private void Dash()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
                StartCoroutine(Dashing());

            if (!dashing && dashColdDown > 0)
            {
                dashColdDown -= Time.deltaTime;
                canDash = dashColdDown <= 0;
            }
        }
        IEnumerator Dashing()
        {
            canDash = false; // 禁止再次冲刺
            dashing = true;
            dashColdDown = _setAttribute.maxDashCooldown;
            float dashTimeLeft = _setAttribute.dashDuration;

            dashFeedback?.PlayFeedbacks();
            //TODO 更新Layer以暂停与怪物layer的判定
            rd.velocity = new Vector2(_setAttribute.dashSpeed * moveDirection, rd.velocity.y);

            while (dashTimeLeft > 0)
            {
                dashTimeLeft -= Time.deltaTime;
                yield return null;
            }
            dashing = false;
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
            if (dashing)
                return;

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

            float curSpeed = rd.velocity.x;
            _setAttribute.GetCurSpeed(input, ref curSpeed, ref curAcceleraTime);
            rd.velocity = new Vector2(curSpeed, rd.velocity.y);
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            //TODO 加入判定：如果是地面layer的话
            jumpState = JumpState.ground;
        }
    }
}

