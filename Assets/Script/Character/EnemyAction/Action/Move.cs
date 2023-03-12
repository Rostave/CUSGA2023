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
        public SharedVector2 movePos;

        public override TaskStatus OnUpdate()
        {
            //突然发现目标则停止巡逻
            if(target.Value!=null&&movePos.Value!=NumberTool.NullV2)
            {
                movePos.Value = NumberTool.NullV2;
                return TaskStatus.Failure;
            }

            //攻击技能的停止距离替代默认停止距离
            float closeDistance = curSkill == null ? GetComponent<SpriteRenderer>().sprite.GetHeight() : curSkill.attackDistance;

            //计算目标点
            Vector2 targetPos=GetTargetPos();

            float distance = Vector2.Distance(transform.position, targetPos);
            float input = targetPos.x - transform.position.x;
            //小于停止距离返回成功
            if (distance <= closeDistance)
            {
                me.Move(0,true);
                return TaskStatus.Success;
            }
            //目标太远则丢失
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

        protected virtual Vector2 GetTargetPos()
        {
            Vector2 targetPos;
            if (target.Value != null) targetPos = target.Value.transform.position;
            else
            {
                if (movePos.Value != NumberTool.NullV2) targetPos = movePos.Value;
                else targetPos = transform.position;
            }
            return targetPos;
        }
    }
}

