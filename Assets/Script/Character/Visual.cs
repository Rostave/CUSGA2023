using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Vacuname
{
    public class Visual: MonoBehaviour
    {
        public float visionDirection;
        public float visionRadius, visionAngle;
        public float chaseRadius;
        private Character character;
        private LayerMask layerMask;
        private void Awake()
        {
            character = GetComponentInParent<Character>();
            layerMask = LayerMask.GetMask("Player", "Invincible");
            chaseRadius = chaseRadius < visionRadius ? visionRadius : chaseRadius;
        }


        public bool TrySeePlayer(out GameObject respon)
        {
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, visionRadius,layerMask);
            respon = null;
            foreach (var col in cols)
            {
                if (col.isTrigger)
                {
                    if (IsInSector(col.transform.position))
                    {
                        respon = col.gameObject;
                        return true;
                    }
                }
            }
            return false;
        }

        private bool IsInSector(Vector2 point)
        {
            // 计算点到扇形中心的向量
            Vector2 direction = point - (Vector2)transform.position;
            // 如果点到扇形中心的距离大于半径，不在扇形内
            if (direction.magnitude > visionRadius)
                return false;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if (character.GetTowardDirection() > 0 && Mathf.Abs(angle) > visionAngle / 2)
            {
                return false;
            }
            else if (character.GetTowardDirection() < 0 && Mathf.Abs(angle) < 180 - visionAngle / 2)
            {
                return false;
            }

            //射线检测障碍物
            LayerMask layer = 1<<LayerMask.NameToLayer("Enemy");
            layer = ~layer;
            RaycastHit2D hit = Physics2D.Linecast((Vector2)transform.position, point,layer);//无视敌人的图层，免得自己被自己挡住
            Debug.DrawLine(transform.position, point);
            if (hit.collider.CompareTag("Player"))//线条碰到的第一个物体是玩家才能看到
            {
                return true;
            }
            else
                return false;
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            float direction = transform.parent.localScale.x >= 0 ? 1 : -1;
            if(character!=null)direction *= character.defaultScale;
            Vector3 from = Quaternion.Euler(0, 0, -visionAngle / 2) * Vector2.right*direction * visionRadius;
            Vector3 to = Quaternion.Euler(0, 0, visionAngle / 2) * Vector2.right * direction * visionRadius;
            UnityEditor.Handles.DrawWireArc(transform.position, Vector3.forward, from, visionAngle, visionRadius);
            UnityEditor.Handles.color = Color.red;
            UnityEditor.Handles.DrawWireArc(transform.parent.position, Vector3.forward, from, 360, chaseRadius);
            Gizmos.DrawLine(transform.position, transform.position + from);
            Gizmos.DrawLine(transform.position, transform.position + to);
        }
    }
}


