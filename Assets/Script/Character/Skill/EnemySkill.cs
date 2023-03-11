using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;
namespace Vacuname
{
    public class EnemySkill : BasicSkill
    {
        public EnemyAction enemyAction;
        [HideInInspector]public BaseEnemy en_me;
        [LabelText("当玩家在该距离内会开始攻击")]
        public float attackDistance;
        protected override void Awake()
        {
            en_me = transform.GetComponentInParent<BaseEnemy>();
            base.Awake();
        }

        private void OnDrawGizmosSelected()
        {
            Vector3 from = Quaternion.Euler(0, 0, -360 / 2) * Vector2.right* attackDistance;
            UnityEditor.Handles.DrawWireArc(transform.position, Vector3.forward, from, 360, attackDistance);
        }

    }
}