using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Vacuname
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        [LabelText("����")][SerializeField]private Attribute _setAttribute;//���ļ������õ�����
        [HideInInspector]public Attribute attribute;//ʵ��ʹ�õ�����
        [SerializeField] private PlayerInput input;
        private float curAcceleraTime;
        private bool jumping;

        private Rigidbody2D rd;

        #region ��ʼ��

        private void Awake()
        {
            attribute = _setAttribute;
            rd=GetComponent<Rigidbody2D>();
            curAcceleraTime = 0;
            jumping = true;
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
            float input = Input.GetAxisRaw("Horizontal");
            Move(input);
            if(Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }

        private void Jump()
        {
            if (!jumping)
            {
                rd.velocity = new Vector2(rd.velocity.x, attribute.jumpStrength);
                jumping = true;
            }
                
        }
        
        private void Move(float input)
        {
            float curSpeed = rd.velocity.x;
            attribute.GetCurSpeed(input,ref curSpeed,ref curAcceleraTime);
            rd.velocity = new Vector2(curSpeed, rd.velocity.y);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            //����ǵ���layer�Ļ�
            if (Mathf.Abs(rd.velocity.y) <= 0.1f)
                jumping = false;
        }

    }
}

