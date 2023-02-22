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
        public SharedInt direction;
        public SharedGameObject target;
        public SharedVector3 targetPos;

        public override TaskStatus OnUpdate()
        {
            float input = target.Value.transform.position.x - transform.position.x;
            float distance = Vector2.Distance(transform.position, target.Value.transform.position);
            if (distance <= 3f)
            {
                me.Move(0,true);
                return TaskStatus.Success;
            }
            else if (distance > 27)
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

