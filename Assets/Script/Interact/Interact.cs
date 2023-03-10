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
        [HideInInspector] public bool triger;//触发式或者切换式，这个在实际的机关中决定
        protected bool active;//切换式会用到这个变量
        protected Animator anima;
        [SerializeField,LabelText("可以使用的次数")] protected int useableTime;
        protected BoxCollider2D col;
        protected virtual void Awake()
        {
            col = GetComponent<BoxCollider2D>();
        }

        public void Triggered()//当被玩家按下或者勾到触发的函数
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


