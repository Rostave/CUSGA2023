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
            visual = transform.GetComponentInChildren<Visual>();
        }

        public override TaskStatus OnUpdate()
        {
            if(target.Value==null)
                if (visual.TrySeePlayer(out GameObject g))
                {
                    target.Value = g;
                }
            return TaskStatus.Running;
        }
    }
}

