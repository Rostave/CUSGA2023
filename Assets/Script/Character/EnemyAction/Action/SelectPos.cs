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
        [SerializeField]protected SharedVector2 patrolPos;
        private Bounds bounds;

        public override void OnAwake()
        {
            base.OnAwake();
            GetRange();
        }

        protected virtual void GetRange()
        {
            range.enabled = true;
            bounds = range.bounds;
            range.enabled = false;
        }

        protected virtual Vector2 GetPos()
        {
            float randomX = Random.Range(bounds.min.x, bounds.max.x);
            float randomY = Random.Range(bounds.min.y, bounds.max.y);
            return new Vector2(randomX, randomY); 
        }

        public override TaskStatus OnUpdate()
        {
            patrolPos.Value = GetPos();
            return TaskStatus.Success;
        }

    }

}

