using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Vacuname
{
    public class Look : EnemyConditional
    {
        public SharedGameObject target;
        public override TaskStatus OnUpdate()
        {
            target.SetValue(GameObject.FindWithTag("Player"));
            float dis = Vector2.Distance(transform.position, target.Value.transform.position);
            if (dis <= 15f)
                return TaskStatus.Success;
            else
                return TaskStatus.Failure;
        }
    }
}

