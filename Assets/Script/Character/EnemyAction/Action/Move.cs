using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vacuname
{
    public class Move:EnemyAction
    {
        //public SharedFloat attackDistance;
        public EnemySkill curSkill;
        [HideInInspector]public SharedInt direction;
        public SharedGameObject target;

        public override TaskStatus OnUpdate()
        {
            float input = target.Value.transform.position.x - transform.position.x;
            float distance = Vector2.Distance(transform.position, target.Value.transform.position);
            if (distance <= curSkill.attackDistance)//π•ª˜æ‡¿Î
            {
                me.Move(0,true);
                return TaskStatus.Success;
            }
            else if (distance > 27)// ”œﬂæ‡¿Î
            {
                me.Move(0, true);
                return TaskStatus.Failure;
            }
            else
            {
                me.Move(input);
                return TaskStatus.Running;
            }
            // 
        }
    }
}

