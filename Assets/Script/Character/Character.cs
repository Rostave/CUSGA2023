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
        private float moveDirection;
        private bool canDash;
        private bool dashing;
        private float dashColdDown;
        #endregion
        public Rigidbody2D rd;
        public Animator anima;

        [TabGroup("����"), SerializeField, InlineEditor(InlineEditorModes.GUIOnly)]
        private MMF_Player timeSlowFeedback, timeFastFeedback, dashFeedback;

        #region ��ʼ��

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
            canDash = false; // ��ֹ�ٴγ��
            dashing = true;
            dashColdDown = _setAttribute.maxDashCooldown;
            float dashTimeLeft = _setAttribute.dashDuration;

            dashFeedback?.PlayFeedbacks();
            //TODO ����Layer����ͣ�����layer���ж�
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

