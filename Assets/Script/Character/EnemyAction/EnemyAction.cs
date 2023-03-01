using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Vacuname
{
    public class EnemyAction : Action
    {
        protected BaseEnemy me;
        protected Animator anima;
        protected Rigidbody2D rd;
        protected TaskStatus taskStatus;
        public void SetTaskStatus(TaskStatus newOne)
        {
            taskStatus = newOne;
        }

        public override void OnAwake()
        {
            me=GetComponent<BaseEnemy>();
            TryGetComponent<Animator>(out anima);
            rd = GetComponent<Rigidbody2D>();
        }

        internal void SetTaskStatus(object failure)
        {
            throw new System.NotImplementedException();
        }
    }
}

