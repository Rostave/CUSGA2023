using BehaviorDesigner.Runtime.Tasks;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Vacuname
{
    public class DashAttack : EnemySkill
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

        private bool dashBlocked;

        protected override void Awake()
        {
            skillName = "DashAttack";
            canDash = true;
            dashing = false;
            dashColdDown = 0;
            base.Awake();
        }

        public override void Effect()
        {
            if (canDash)
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

        protected virtual Vector2 GetDashVelocity()
        {
            return new Vector2(dashSpeed * me.GetMoveDirection(), me.time.rigidbody2D.velocity.y);
        }

        IEnumerator Dashing()
        {
            range.enabled = true;
            dashBlocked = false;
            me.SetControllable(false);
            canDash = false;
            dashing = true;
            dashColdDown = maxDashCooldown;
            float dashTimeLeft = dashDuration;

            me.TryPlayFeedback("Dash");
            me.time.rigidbody2D.velocity = GetDashVelocity();

            while (dashTimeLeft > 0)
            {
                dashTimeLeft -= Time.deltaTime;
                yield return null;
            }
            dashing = false;
            me.SetControllable(true);
            //enemyAction.SetTaskStatus(TaskStatus.Success);
            range.enabled = false;
        }

        private void OnTriggerEnter2D(Collider2D a)
        {
            if (a.CompareTag("Sheild"))
            {
                dashBlocked = true;
                me.TryPlayFeedback("Blocked");
                if (a.transform.TryGetComponent(out HitBack hitback))
                    hitback.Success();
            }
            else if(!dashBlocked&&a.CompareTag("Player"))
            {
                Player player = a.GetComponent<Player>();
                player.TryPlayFeedback("Hit");
            }
        }


    }
}