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
        private Visual visual;

        public override void OnAwake()
        {
            visual = this.gameObject.GetComponentInChildren<Visual>();
        }

        public override TaskStatus OnUpdate()
        {
            GameObject g;
            if (visual.TrySeePlayer(out g))
            {
                target.Value = g;
                return TaskStatus.Success;
            }
            else
                return TaskStatus.Failure;
        }
    }
}

