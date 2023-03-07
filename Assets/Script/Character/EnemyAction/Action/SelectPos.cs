using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Vacuname
{
    public class SelectPos : EnemyAction
    {
        [SerializeField]private BoxCollider2D range;
        [SerializeField] private SharedVector2 patrolPos;
        private Bounds bounds;

        public override void OnAwake()
        {
            base.OnAwake();
            range.enabled = true;
            bounds = range.bounds;
            range.enabled = false;
        }

        public override TaskStatus OnUpdate()
        {
            float randomX = Random.Range(bounds.min.x, bounds.max.x);
            float randomY = Random.Range(bounds.min.y, bounds.max.y);
            patrolPos.Value= new Vector2(randomX, randomY);
            return TaskStatus.Success;
        }

    }

}

