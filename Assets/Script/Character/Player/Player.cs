using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Vacuname
{
    public enum JumpState {ground,jump,fall }
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        [LabelText("����")][SerializeField]private Attribute _setAttribute;//���ļ������õ�����
        //[HideInInspector]public Attribute attribute;//ʵ��ʹ�õ�����

        #region �����˶�������Ҫ�Ĳ���
        [SerializeField] private PlayerInput input;
        private float curAcceleraTime;
        private JumpState jumpState;
        private float moveDirection;
        private bool canDash;
        private bool dashing;
        private float dashColdDown;
        #endregion
        private Rigidbody2D rd;

        #region Feedbacks
        public MMF_Player timeSlowFeedback;
        #endregion

        #region ��ʼ��

        private void Awake()
        {
            rd=GetComponent<Rigidbody2D>();
            curAcceleraTime = 0;
            jumpState = JumpState.fall;
            canDash = true;
            dashing = false;
            moveDirection = 1;
            dashColdDown = 0;

            transform.TryGetComponentByChildName<MMF_Player>("TimeSlow",out timeSlowFeedback);
        }

        private void OnEnable()
        {
            //input.onMove += Move;
            CameraControl.Instance.ca.m_Follow = transform;
        }
        private void OnDisable()
        {
            //input.onMove -= Move;
        }
        #endregion

        private void Update()
        {
            //�������ԱȽϷ���

            if(rd.gravityScale != _setAttribute.gravity)
                rd.gravityScale = _setAttribute.gravity;

            Move(Input.GetAxisRaw("Horizontal"));
            Dash();
            Jump();
            ControlTime();
        }

        private void ControlTime()
        {
            if(Input.GetKeyDown(KeyCode.Tab))
            {
                timeSlowFeedback?.PlayFeedbacks();
                TimeControl.Instance.SetTimeScale(_setAttribute.slowDownTimeScale,_setAttribute.slowDownTimer);
            }
            else if(Input.GetKeyUp(KeyCode.Tab))
            {
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

            //TODO ����Layer����ͣ�����layer���ж�
            rd.velocity = new Vector2(_setAttribute.dashSpeed * moveDirection, rd.velocity.y);

            while (dashTimeLeft > 0)
            {
               dashTimeLeft -= Time.deltaTime ;
               yield return null;
            }
            dashing = false;
        }

        private void Jump()
        {
            if(jumpState==JumpState.ground&&Input.GetKeyDown(KeyCode.Space))
            {
                rd.velocity = new Vector2(rd.velocity.x, _setAttribute.jumpStrength);
                jumpState = JumpState.jump;
            }
            else if (jumpState == JumpState.jump && rd.velocity.y < 0)
            {
                jumpState = JumpState.fall;
            }

        }

        private void Move(float input)
        {
            if (dashing)
                return;
            
            //����ı䳯���Ժ�������Ϳ��Բ�Ҫ����ˣ�����ֻ�ǲ�����
            if(input!=0&&moveDirection!=input)
            {
                moveDirection = input;
                Vector3 temp = transform.localScale;
                temp.x = moveDirection;
                transform.localScale = temp;
            }    

            float curSpeed = rd.velocity.x;
            _setAttribute.GetCurSpeed(input,ref curSpeed,ref curAcceleraTime);
            rd.velocity = new Vector2(curSpeed, rd.velocity.y);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            //TODO �����ж�������ǵ���layer�Ļ�
            jumpState = JumpState.ground;
        }
    }
}

