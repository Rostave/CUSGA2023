using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vacuname
{
    public class Move:EnemyAction
    {
        public EnemySkill curSkill;
        [HideInInspector]public SharedInt direction;
        public SharedGameObject target;
        public SharedVector2 patrolPos;

        public override TaskStatus OnUpdate()
        {
            if(target.Value!=null&&patrolPos.Value!=NumberTool.NullV2)
            {
                patrolPos.Value = NumberTool.NullV2;
                return TaskStatus.Failure;
            }

            float closeDistance = curSkill == null ? GetComponent<SpriteRenderer>().sprite.GetHeight() : curSkill.attackDistance;

            Vector2 targetPos;
            if (target.Value != null) targetPos = target.Value.transform.position;
            else
            {
                if (patrolPos.Value != NumberTool.NullV2) targetPos = patrolPos.Value;
                else targetPos = transform.position;
            }

            float distance = Vector2.Distance(transform.position, targetPos);
            float input = targetPos.x - transform.position.x;
            if (distance <= closeDistance)
            {
                me.Move(0,true);
                return TaskStatus.Success;
            }
            else if (target.Value != null && distance > me.visual.chaseRadius)
            {
                me.Move(0, true);
                target.Value=null;
                return TaskStatus.Failure;
            }
            else
            {
                me.Move(input);
                return TaskStatus.Running;
            }
        }
    }
}

