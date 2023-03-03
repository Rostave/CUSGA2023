using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vacuname
{
    public class AttackSkill : EnemySkill
    {
        public override void Effect()
        {
            range.enabled = true;
            var contactFilter2D = new ContactFilter2D();
            contactFilter2D.useTriggers = true;
            contactFilter2D.SetLayerMask(LayerMask.GetMask("Player"));
            List<Collider2D> hits = new List<Collider2D>();
            range.OverlapCollider(contactFilter2D, hits);
            Player p = null;
            bool blocked = false;
            foreach (var a in hits)
            {
                if (a.CompareTag("Player"))
                {
                    p = a.GetComponent<Player>();
                }
                else if (a.CompareTag("Sheild"))
                {
                    blocked = true;
                    if (a.transform.TryGetComponent(out HitBack hitback))
                        hitback.Success();
                    break;
                }
            }
            
            range.enabled = false;

            if (blocked)
            {
                me.feedbacks.TryPlay("Blocked");
                enemyAction.SetTaskStatus(TaskStatus.Failure);

            }
            else
            {
                if(p!=null)//没有被挡且打中了
                {
                    p.feedbacks.TryPlay("Hit");
                }
                enemyAction.SetTaskStatus(TaskStatus.Success);
            }
                
        }
    }
}