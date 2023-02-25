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

        public override TaskStatus OnUpdate()
        {
            Bounds bounds = range.bounds;
            float randomX = Random.Range(bounds.min.x, bounds.max.x);
            float randomY = Random.Range(bounds.min.y, bounds.max.y);
            patrolPos.Value= new Vector2(randomX, randomY);
            return TaskStatus.Success;
        }

    }

}

