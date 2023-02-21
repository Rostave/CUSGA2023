using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Vacuname
{
    public class EnemyAction : Action
    {
        protected Character me;
        protected Animator anima;
        protected Rigidbody2D rd;
        public override void OnAwake()
        {
            me=GetComponent<Character>();
            anima = me.anima;
            rd = me.rd;
        }
    }
}

