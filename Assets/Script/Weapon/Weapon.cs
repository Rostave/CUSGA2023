using R0.ScriptableObjConfig;
using R0.SpellRel;
using UnityEngine;

namespace R0.Weapon
{
    /// <summary>
    /// 召唤物（子弹）管理类
    /// </summary>
    public class Weapon : MonoBehaviour
    {
        private float _triggerCd;
        private float _nextTriggerTime;
        private bool _canTrigger;

        private void Start()
        {
            _canTrigger = true;
        }

        private void Update()
        {
            if (!_canTrigger)
            {
                if (Time.time > _nextTriggerTime) _canTrigger = true;
                else return;
            }
            
            if (!Input.GetMouseButtonDown(0)) return;

            _canTrigger = false;
            _nextTriggerTime = Time.time + _triggerCd;
            TriggerAtk();
        }

        /// <summary>
        /// 更新攻击间隔
        /// </summary>
        /// <param name="cdIncrement">攻击间隔增量，负数为减</param>
        public void UpdateAtkCd(float cdIncrement) => _triggerCd = BulletData.Instance.defaultSummonCd + cdIncrement;

        private void TriggerAtk()
        {
            SpellScroll.Instance.ApplySpellOnTrigger();  // 开始攻击瞬间的符文结算
            
        }

    }
}