using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vacuname
{
    public class FlyDash:DashAttack
    {
        protected override Vector2 GetDashVelocity()
        {
            GameObject s = me.GetComponent<BehaviorTree>().GetVariable("target").GetValue() as GameObject;
            Vector2 dash = (s.transform.position - transform.position).normalized;
            dash *= dashSpeed;
            return dash;
        }
        protected void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.layer==LayerMask.NameToLayer("Ground"))
            {
                me.time.rigidbody2D.velocity = Vector2.zero;
            }
        }

    }

}
