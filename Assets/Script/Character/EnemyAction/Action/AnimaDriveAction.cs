using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vacuname
{
    public class AnimaDriveAction : EnemyAction
    {
        /// <summary>
        /// 靠动画驱动Skill时使用这个（这个所绑定的Skill一定得在结束的时候修改这个动作的taskStatus，否则会卡住）
        /// 这个Action的唯一意义在于返回Skill的结果（不会触发Skill），以便在后续的行为树执行不同的路线
        /// </summary>
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

