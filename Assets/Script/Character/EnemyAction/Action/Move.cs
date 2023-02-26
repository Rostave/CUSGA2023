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
            if(target.Value!=null&&patrolPos.Value!=NumberTool.NullV2)//发现玩家，中断巡逻
            {
                patrolPos.Value = NumberTool.NullV2;
                return TaskStatus.Failure;
            }

            float closeDistance = curSkill == null ? GetComponent<SpriteRenderer>().sprite.GetHeight() : curSkill.attackDistance;

            Vector2 targetPos = target.Value != null ? target.Value.transform.position : 
                patrolPos.Value != NumberTool.NullV2 ? patrolPos.Value : transform.position;

            float distance = Vector2.Distance(transform.position, targetPos);
            float input = targetPos.x - transform.position.x;
            if (distance <= closeDistance)//到达了
            {
                me.Move(0,true);
                return TaskStatus.Success;
            }
            else if (target.Value != null && distance > me.visual.visionRadius)//跟丢
            {
                me.Move(0, true);
                target.Value=null;
                return TaskStatus.Failure;
            }
            else//那就往那个地方前进
            {
                me.Move(input);
                return TaskStatus.Running;
            }
        }
    }
}

