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
            int layer = ~gameObject.layer;
            contactFilter2D.SetLayerMask(layer);
            List<Collider2D> hits = new List<Collider2D>();
            range.OverlapCollider(contactFilter2D, hits);
            Player p = null;
            bool blocked = false;
            foreach (var a in hits)
            {
                if (a.CompareTag("Player"))
                {
                    p = a.GetComponent<Player>();
                    p.feedbacks.TryPlay("Hit");
                }
                else if (a.CompareTag("Sheild"))
                {
                    blocked = true;
                    me.feedbacks.TryPlay("Blocked");
                    Debug.Log(string.Format("<color=#ff0000>{0}</color>", "Blocked"));
                    break;
                }
            }
            range.enabled = false;

            if (blocked)
                enemyAction.SetTaskStatus(TaskStatus.Failure);
            else
                enemyAction.SetTaskStatus(TaskStatus.Success);
        }
    }
}