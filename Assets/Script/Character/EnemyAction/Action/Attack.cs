using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vacuname
{
    public class Attack : EnemyAction
    {
        public EnemySkill attackSkill;

        public override void OnAwake()
        {
            attackSkill.enemyAction = this;
        }

        public override void OnStart()
        {
            taskStatus = TaskStatus.Running;
        }
        public override TaskStatus OnUpdate()
        {
            return taskStatus;
        }
    }
}

