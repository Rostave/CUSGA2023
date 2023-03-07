using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vacuname
{
    public class Dash :BasicSkill
    {
        private bool canDash;
        private bool dashing;
        private float dashColdDown;

        [LabelText("冲刺速度")]
        public float dashSpeed;

        [LabelText("冲刺持续时间")]
        public float dashDuration;

        [LabelText("冲刺冷却时间")]
        public float maxDashCooldown;

        protected override void Awake()
        {
            skillName = "Dash";
            canDash = true;
            dashing = false;
            dashColdDown = 0;
            base.Awake();
        }

        public override void Effect()
        {
            if(canDash)
                StartCoroutine(Dashing());
        }

        private void Update()
        {
            if (!dashing && dashColdDown > 0)
            {
                dashColdDown -= Time.deltaTime;
                canDash = dashColdDown <= 0;
            }
        }

        IEnumerator Dashing()
        {
            Debug.Log(1);
            me.SetControllable(false);
            canDash = false;
            dashing = true;
            dashColdDown = maxDashCooldown;
            float dashTimeLeft = dashDuration;

            me.feedbacks.TryPlay("Dash");
            gameObject.layer = LayerMask.NameToLayer("Invincible");
            me.time.rigidbody2D.velocity = new Vector2(dashSpeed * me.GetMoveDirection(), me.time.rigidbody2D.velocity.y);

            while (dashTimeLeft > 0)
            {
                dashTimeLeft -= Time.deltaTime;
                yield return null;
            }
            gameObject.layer = LayerMask.NameToLayer("Player");
            dashing = false;
            me.SetControllable(true);
        }


    }
}

