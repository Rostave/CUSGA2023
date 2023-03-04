using System;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using R0;
using R0.Character;
using R0.Static;
using UnityEngine;
using UnityEngine.Events;

namespace Vacuname
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : Character
    {
        public enum AnimState
        {
            Idle, Move, Rest, JumpUp, JumpFall
        }

        [SerializeField]protected AnimState animState;

        #region Hitback
        [SerializeField,TabGroup("技能"),InlineEditor(InlineEditorModes.GUIOnly)]private HitBack hitBack;
        public HitBack GetGitBack()
        {
            return hitBack;
        }
        #endregion

        #region 移动所需变量
        private bool canDash;
        private bool dashing;
        private float dashColdDown;
        #endregion

        #region 初始化
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

            if(time.rigidbody2D.gravityScale != moveAttribute.gravity)
                time.rigidbody2D.gravityScale = moveAttribute.gravity;

            Move(Input.GetAxisRaw("Horizontal"));
            Dash();
            Jump();
            ControlTime();
            HandleHitBack();
            AnimStateUpdate();
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
            else if (jumpState == JumpState.jump && time.rigidbody2D.velocity.y < 0)
            {
                jumpState = JumpState.fall;
            }
        }
        public override void Move(float input, bool setDirectly = false)
        {
            if (dashing)
                return;
            base.Move(input, setDirectly);
        }
        IEnumerator Dashing()
        {
            canDash = false;
            dashing = true;
            dashColdDown = moveAttribute.maxDashCooldown;
            float dashTimeLeft = moveAttribute.dashDuration;

            feedbacks.TryPlay("Dash");
            gameObject.layer = LayerMask.NameToLayer("Invincible");
            time.rigidbody2D.velocity = new Vector2(moveAttribute.dashSpeed * moveDirection, time.rigidbody2D.velocity.y);

            while (dashTimeLeft > 0)
            {
                dashTimeLeft -= Time.deltaTime;
                yield return null;
            }
            gameObject.layer = LayerMask.NameToLayer("Player");
            dashing = false;
        }
        private void HandleHitBack()
        {
            if (Input.GetKeyDown(KeyCode.E))
                hitBack.Effect();
        }

        protected override void OnCharacterLandGround()
        {
            base.OnCharacterLandGround();

            // if (jumpState == JumpState.ground) return;
            // if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) < Const.IdlePrecision)
            // {
            //     anima.SetTrigger(Const.Ani.Idle);
            //     animState = AnimState.Idle;
            //     Debug.Log("idle2");
            // }
            // else
            // {
            //     anima.SetTrigger(Const.Ani.Move);
            //     animState = AnimState.Move;
            //     Debug.Log("move2");
            // }
        }

        private void AnimStateUpdate()
        {
            // Debug.Log(time.rigidbody2D.velocity.y);
            if (Mathf.Abs(rd.velocity.y) > Const.JumpPrecision)
            {
                // 跳跃
                if (time.rigidbody2D.velocity.y > 0)
                {
                    if (animState < AnimState.JumpUp)
                    {
                        anima.SetTrigger(Const.Ani.JumpUp);
                        animState = AnimState.JumpUp;
                        Debug.Log("up");
                    }
                }
                else
                {
                    if (animState < AnimState.JumpFall)
                    {
                        anima.SetTrigger(Const.Ani.JumpFall);
                        animState = AnimState.JumpFall;
                        Debug.Log("fall");
                    }
                }

                // anima.SetFloat(Const.Ani.VelocityY, rd.velocity.y);
            }
            else
            {
                // 水平
                if (animState == AnimState.JumpUp) return;
                if (Mathf.Abs(Input.GetAxis("Horizontal")) < Const.IdlePrecision)
                {
                    if (animState != AnimState.Idle)
                    {
                        anima.SetTrigger(Const.Ani.Idle);
                        animState = AnimState.Idle;
                        Debug.Log("idle");
                    }
                }
                else if (animState < AnimState.Move)
                {
                    anima.SetTrigger(Const.Ani.Move);
                    animState = AnimState.Move;
                    Debug.Log("move");
                }
            }
        }
        
    }
}

