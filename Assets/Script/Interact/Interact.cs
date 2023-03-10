using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix;
using Sirenix.OdinInspector;

namespace Vacuname
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Interact : MonoBehaviour
    {
        [HideInInspector] public bool triger;//����ʽ�����л�ʽ�������ʵ�ʵĻ����о���
        protected bool active;//�л�ʽ���õ��������
        protected Animator anima;
        [SerializeField,LabelText("����ʹ�õĴ���")] protected int useableTime;
        protected BoxCollider2D col;
        protected virtual void Awake()
        {
            col = GetComponent<BoxCollider2D>();
        }

        public void Triggered()//������Ұ��»��߹��������ĺ���
        {
            if (useableTime <= 0)
                return;
            useableTime--;
            if (triger)
                ActiveEffect();
            else
            {
                if (!active)
                    ActiveEffect();
                else
                    NegativeEffect();
            }
        }
        protected virtual void ActiveEffect()
        {
            active = true;
        }

        protected virtual void NegativeEffect()
        {
            active = false;
        }
    }
}


