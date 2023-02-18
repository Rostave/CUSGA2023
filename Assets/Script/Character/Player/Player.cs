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
        [LabelText("属性")][SerializeField]private Attribute _setAttribute;//在文件里设置的属性
        [HideInInspector]public Attribute attribute;//实际使用的属性
        [SerializeField] private PlayerInput input;
        private float curAcceleraTime;
        private bool jumping;

        private Rigidbody2D rd;

        #region 初始绑定

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
            //如果是地面layer的话
            if (Mathf.Abs(rd.velocity.y) <= 0.1f)
                jumping = false;
        }

    }
}

