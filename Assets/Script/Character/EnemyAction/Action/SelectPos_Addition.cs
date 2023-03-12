using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vacuname
{
    public class SelectPos_Addition : EnemyAction
    {
        [SerializeField]private Vector2 addtionPosition;
        [SerializeField] protected SharedVector2 patrolPos;
        public override TaskStatus OnUpdate()
        {
            Vector2 res = (Vector2)transform.position + addtionPosition;
            patrolPos.Value = res;
            return TaskStatus.Success;
        }
    }
}

