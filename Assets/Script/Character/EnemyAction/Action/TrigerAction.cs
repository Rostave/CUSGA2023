using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vacuname
{
    public class TrigerAction : EnemyAction
    {
        /// <summary>
        /// 会执行相应Skill的Effect并且直接返回Success
        /// </summary>
        public EnemySkill attackSkill;
        public override void OnAwake()
        {
            attackSkill.enemyAction = this;
        }
        public override TaskStatus OnUpdate()
        {
            attackSkill.Effect();
            return TaskStatus.Success;
        }
    }
}