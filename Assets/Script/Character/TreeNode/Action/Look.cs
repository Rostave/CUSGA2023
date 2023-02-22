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
        float visionRadius=10, visionAngle=60f;
        LayerMask targetLayer = LayerMask.GetMask("Player");
        public override TaskStatus OnUpdate()
        {
            Collider[] targets = Physics.OverlapSphere(transform.position, visionRadius, targetLayer);
            foreach (Collider target in targets)
            {
                Vector3 direction = target.transform.position - transform.position;
                float angle = Vector3.Angle(direction, transform.forward);
                if (angle < visionAngle / 2f)
                {
                    return TaskStatus.Success;
                }
            }
            return TaskStatus.Failure;
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, visionRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * visionRadius);
            Vector3 leftDir = Quaternion.Euler(0, -visionAngle / 2f, 0) * transform.forward;
            Vector3 rightDir = Quaternion.Euler(0, visionAngle / 2f, 0) * transform.forward;
            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.position, transform.position + leftDir * visionRadius);
            Gizmos.DrawLine(transform.position, transform.position + rightDir * visionRadius);
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, leftDir * visionRadius);
            Gizmos.DrawRay(transform.position, rightDir * visionRadius);
        }
    }
}

