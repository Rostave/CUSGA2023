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
        [SerializeField]private bool _isAiming;
        private Bone _aimBone;
        private SkeletonAnimation _sAnima;

        #region 初始化
        private void Start()
        {
            CameraControl.Instance.ca.m_Follow = transform;
            animState = AnimState.Idle;
            transform.Find("Spine1").TryGetComponent(out _sAnima);
            _aimBone = _sAnima.skeleton.FindBone("target3");
        }
        #endregion

        private void Update()
        {

            if(time.rigidbody2D.gravityScale != moveAttribute.gravity)
                time.rigidbody2D.gravityScale = moveAttribute.gravity;

            Move(Input.GetAxisRaw("Horizontal"));
            HandleDash();
            Jump();
            ControlTime();
            HandleHitBack();
            AnimStateUpdate();
        }
        private void ControlTime()
        {
            if(Input.GetKeyDown(KeyCode.Tab))
            {
                TryPlayFeedback("TimeSlow");
            }
            if(Input.GetKeyUp(KeyCode.Tab))
            {
                TryPlayFeedback("TimeFast");
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
        private void HandleDash()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
                TrigerSkill("Dash");
        }
        private void HandleHitBack()
        {
            if (Input.GetKeyDown(KeyCode.E))
                TrigerSkill("HitBack");
        }

        private void AnimStateUpdate()
        {
            // 攻击
            if (Input.GetMouseButtonDown(0))
            {
                // anima.SetTrigger(Const.Ani.Aim);
                var aimTrack = _sAnima.AnimationState.SetAnimation(2, "Aim", true);
                aimTrack.AttachmentThreshold = 1f;
                aimTrack.MixDuration = 0f;
                _isAiming = true;
                
                var shootTrack = _sAnima.AnimationState.SetAnimation(1, "Attack", false);
                shootTrack.AttachmentThreshold = 1f;
                shootTrack.MixDuration = 0f;
                var empty1 = _sAnima.state.AddEmptyAnimation(1, 0.5f, 0.1f);
                empty1.AttachmentThreshold = 1f;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                // anima.SetTrigger(Const.Ani.Null);
                var empty2 = _sAnima.state.AddEmptyAnimation(2, 0.5f, 0.1f);
                empty2.AttachmentThreshold = 1f;
                _isAiming = false;
            }

            if (_isAiming)
            {
                var mousePosition = Input.mousePosition;
                var worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
                var skeletonSpacePoint = _sAnima.transform.InverseTransformPoint(worldMousePosition);
                skeletonSpacePoint.x *= _sAnima.Skeleton.ScaleX;
                skeletonSpacePoint.y *= _sAnima.Skeleton.ScaleY;
                _aimBone.SetLocalPosition(skeletonSpacePoint);
            }
            
            if (Mathf.Abs(rd.velocity.y) > Const.JumpTolerance)
            {
                // 跳跃
                if (time.rigidbody2D.velocity.y > 0)
                {
                    if (animState >= AnimState.JumpUp) return;
                    // anima.SetTrigger(Const.Ani.JumpUp);
                    _sAnima.AnimationState.SetAnimation(0, "Jump-up1", false);
                    animState = AnimState.JumpUp;
                }
                else if (animState < AnimState.JumpFall)
                {
                    // anima.SetTrigger(Const.Ani.JumpFall);
                    _sAnima.AnimationState.SetAnimation(0, "Jump-down2", true);
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
                    _sAnima.AnimationState.SetAnimation(0, "Idle", true);
                    animState = AnimState.Idle;
                }
                else if (animState != AnimState.Move)
                {
                    // anima.SetTrigger(Const.Ani.Move);
                    _sAnima.AnimationState.SetAnimation(0, "Move", true);
                    animState = AnimState.Move;
                }
            }
        }
        
        
    }
}

