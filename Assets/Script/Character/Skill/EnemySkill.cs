using System.Collections;
using UnityEngine;

namespace Vacuname
{
    public class EnemySkill : BasicSkill
    {
        public EnemyAction enemyAction;
        public BaseEnemy en_me;
        public float attackDistance;
        protected override void Awake()
        {
            en_me = transform.GetComponentInParent<BaseEnemy>();
            base.Awake();
        }
    }
}