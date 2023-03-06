using System;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using R0;
using R0.Character;
using R0.Static;
using Spine;
using Spine.Unity;
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
        private Bone _aimBone;
        [SerializeField]private bool _isAiming;

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
            _aimBone = s_anima.skeleton.FindBone("target3");
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

        private void AnimStateUpdate()
        {
            // 攻击
            if (Input.GetMouseButtonDown(0))
            {
                // anima.SetTrigger(Const.Ani.Aim);
                var aimTrack = s_anima.AnimationState.SetAnimation(2, "Aim", true);
                aimTrack.AttachmentThreshold = 1f;
                aimTrack.MixDuration = 0f;
                _isAiming = true;
                
                var shootTrack = s_anima.AnimationState.SetAnimation(1, "Attack", false);
                shootTrack.AttachmentThreshold = 1f;
                shootTrack.MixDuration = 0f;
                var empty1 = s_anima.state.AddEmptyAnimation(1, 0.5f, 0.1f);
                empty1.AttachmentThreshold = 1f;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                // anima.SetTrigger(Const.Ani.Null);
                var empty2 = s_anima.state.AddEmptyAnimation(2, 0.5f, 0.1f);
                empty2.AttachmentThreshold = 1f;
                _isAiming = false;
            }

            if (_isAiming)
            {
                var mousePosition = Input.mousePosition;
                var worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
                var skeletonSpacePoint = s_anima.transform.InverseTransformPoint(worldMousePosition);
                skeletonSpacePoint.x *= s_anima.Skeleton.ScaleX;
                skeletonSpacePoint.y *= s_anima.Skeleton.ScaleY;
                _aimBone.SetLocalPosition(skeletonSpacePoint);
            }
            
            if (Mathf.Abs(rd.velocity.y) > Const.JumpTolerance)
            {
                // 跳跃
                if (time.rigidbody2D.velocity.y > 0)
                {
                    if (animState >= AnimState.JumpUp) return;
                    // anima.SetTrigger(Const.Ani.JumpUp);
                    s_anima.AnimationState.SetAnimation(0, "Jump-up1", false);
                    animState = AnimState.JumpUp;
                }
                else if (animState < AnimState.JumpFall)
                {
                    // anima.SetTrigger(Const.Ani.JumpFall);
                    s_anima.AnimationState.SetAnimation(0, "Jump-down2", true);
                    animState = AnimState.JumpFall;
                }   
            }
            else
            {
                if (animState == AnimState.JumpUp) return;
                if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 0)
                {
                    if (animState == AnimState.Idle) return;
                    // anima.SetTrigger(Const.Ani.Idle);
                    s_anima.AnimationState.SetAnimation(0, "Idle", true);
                    animState = AnimState.Idle;
                }
                else if (animState != AnimState.Move)
                {
                    // anima.SetTrigger(Const.Ani.Move);
                    s_anima.AnimationState.SetAnimation(0, "Move", true);
                    animState = AnimState.Move;
                }
            }
        }
        
        
    }
}

