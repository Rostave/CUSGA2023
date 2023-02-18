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
        [LabelText("����")][SerializeField]private Attribute _setAttribute;//���ļ������õ�����
        [HideInInspector]public Attribute attribute;//ʵ��ʹ�õ�����
        [SerializeField] private PlayerInput input;
        private float curAcceleraTime;

        private Rigidbody2D rd;
        private BoxCollider2D col;

        #region ��ʼ��

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

