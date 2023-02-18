using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Vacuname
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class Player : MonoBehaviour
    {
        [LabelText("属性")][SerializeField]private Attribute _setAttribute;//在文件里设置的属性
        [HideInInspector]public Attribute attribute;//实际使用的属性
        [SerializeField] private PlayerInput input;
        private float curAcceleraTime;

        private Rigidbody2D rd;
        private BoxCollider2D col;

        #region 初始绑定

        private void Awake()
        {
            attribute = _setAttribute;
            rd=GetComponent<Rigidbody2D>();
            col = GetComponent<BoxCollider2D>();
            curAcceleraTime = 0;
        }

        private void OnEnable()
        {
            //input.onMove += Move;
            input.onDisMove += DisMove;
            CameraControl.Instance.ca.m_Follow = transform;
        }
        private void OnDisable()
        {
            //input.onMove -= Move;
            input.onDisMove -= DisMove;
        }
        #endregion

        private void Update()
        {
            float input = Input.GetAxis("Horizontal");
            input = input > 0 ? 1 : input < 0 ? -1 : 0;
            Move(input);
        }

        private void DisMove()
        {
            
        }

        private void Move(float input)
        {
            float curSpeed = rd.velocity.x;
            attribute.GetCurSpeed(input,ref curSpeed,ref curAcceleraTime);
            rd.velocity = new Vector2(curSpeed, rd.velocity.y);
        }

        
    }
}

