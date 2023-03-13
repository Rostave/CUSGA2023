using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Vacuname
{
    public class Toward : EnemyAction
    {
        public SharedGameObject target;

        public override TaskStatus OnUpdate()
        {
            float input = target.Value.transform.position.x - transform.position.x;
            me.SetTowardDirection(input);
            return TaskStatus.Success;
        }
    }
}

