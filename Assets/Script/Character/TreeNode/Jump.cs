using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Vacuname
{
    public enum JumpState { ground, jump, fall }
    public class Jump:EnemyAction
    {
        public override void OnAwake()
        {
            base.OnAwake();
        }
        public override void OnStart()
        {
            me.Jump();
            Debug.Log("s");
        }

        public override TaskStatus OnUpdate()
        {
            return me.jumpState == JumpState.ground ? TaskStatus.Success : TaskStatus.Running;
        }

    }
}

