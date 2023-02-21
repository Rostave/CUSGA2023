using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vacuname
{
    public class Attack : EnemyAction
    {
        public SharedGameObject act;
        private BasicSkill attack;
        public override void OnAwake()
        {
            base.OnAwake();
            attack = act.Value.GetComponent<BasicSkill>();
        }

        private bool cutDown;
        public override void OnStart()
        {
            cutDown = attack.TryMakeDamage();
        }
        public override TaskStatus OnUpdate()
        {
            return cutDown ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}

