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
            target = GameObject.FindWithTag("Player");
            return TaskStatus.Success;
        }
    }
}

