using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using R0;
using R0.Static;
using UnityEngine;
using UnityEngine.Events;

namespace Vacuname
{
    // 编码测试 - R0
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : Character
    {
        public enum AnimState
        {
            Idle, Move, Jump, Rest
        }

        [TabGroup("�����ļ�"), AssetsOnly, InlineEditor(InlineEditorModes.GUIOnly)]
        [LabelText("ʱ������"), SerializeField]
        protected TimeAttribute timeAttribute;

        protected AnimState animState;

        [SerializeField]private HitBack hitBack;

        #region ��̼������
        private bool canDash;
        private bool dashing;
        private float dashColdDown;
        #endregion

        #region ��ʼ��
        protected override void Awake()
        {
            base.Awake();

            canDash = true;
            dashing = false;
            dashColdDown = 0;
        }

        private void Start()
        {
            CameraControl.Instance.ca.m_Follow = transform;
            animState = AnimState.Idle;
        }
        #endregion

        private void Update()
        {
            //�������ԱȽϷ���

            if(rd.gravityScale != moveAttribute.gravity)
                rd.gravityScale = moveAttribute.gravity;

            Move(Input.GetAxisRaw("Horizontal"));
            Dash();
            Jump();
            ControlTime();
            HandleHitBack();
        }
        private void ControlTime()
        {
            if(Input.GetKeyDown(KeyCode.Tab))
            {
                feedbacks.TryPlay("TimeSlow");
            }
            if(Input.GetKeyUp(KeyCode.Tab))
            {
                feedbacks.TryPlay("TimeFast");
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
        public override void Jump()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                base.Jump();
            }
            else if (jumpState == JumpState.jump && rd.velocity.y < 0)
            {
                jumpState = JumpState.fall;
            }

        }
        public override void Move(float input, bool setDirectly = false)
        {
            if (dashing)
                return;

            if (Mathf.Abs(input) < 1e-6)
            {
                if (animState != AnimState.Idle)
                {
                    PlayerSAnimHelper.PlayAnim(s_anima, Const.Ani.Idle);
                    animState = AnimState.Idle;
                }
            }
            else if (animState != AnimState.Move)
            {
                PlayerSAnimHelper.PlayAnim(s_anima, Const.Ani.Move);
                animState = AnimState.Move;
            }

            base.Move(input, setDirectly);
        }
        IEnumerator Dashing()
        {
            canDash = false; // ��ֹ�ٴγ��
            dashing = true;
            dashColdDown = moveAttribute.maxDashCooldown;
            float dashTimeLeft = moveAttribute.dashDuration;

            feedbacks.TryPlay("Dash");
            //TODO ����Layer����ͣ�����layer���ж�
            rd.velocity = new Vector2(moveAttribute.dashSpeed * moveDirection, rd.velocity.y);

            while (dashTimeLeft > 0)
            {
               dashTimeLeft -= Time.deltaTime ;
               yield return null;
            }
            dashing = false;
        }
        private void HandleHitBack()
        {
            if (Input.GetKeyDown(KeyCode.E))
                hitBack.Effect();
        }
    }
}

