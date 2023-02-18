using R0.Bullet;
using R0.ScriptableObjConfig;
using R0.SpellRel;
using Sirenix.OdinInspector;
using UnityEngine;

namespace R0.Weapons
{
    /// <summary>
    /// 召唤物（子弹）管理类
    /// </summary>
    public class Weapon : MonoBehaviour
    {
        [GUIColor(0.3f, 0.8f, 0.8f, 1f)]
        [SerializeField, DisplayAsString] private float triggerCd;
        
        private float _nextTriggerTime;
        private bool _canTrigger;
        
        // 效果参数
        public float bulletSpeedMultiplier = 1f;
        public float bulletDmgMultiplier = 1f;
        public SpellEffect bulletEffect;
        public float effectParamOnEnemy;
        
        private Vector3 _pointingDir;
        private float _bulletAngle;  // 相邻子弹间开角 
        public float BulletAngle
        {
            get => _bulletAngle;
            set
            {
                _bulletAngle = value;
                _bulletInterQAngle = Quaternion.AngleAxis(_bulletAngle, Vector3.forward);
            }
        }
        public int ammoCount;

        private Quaternion _bulletInterQAngle;

        private void Start()
        {
            _canTrigger = true;
            BulletAngle = SpellData.Instance.bulletInterAngle;
        }

        private void Update()
        {
            if (!_canTrigger)
            {
                if (Time.time > _nextTriggerTime) _canTrigger = true;
                else return;
            }
            
            if (!Input.GetMouseButton(0)) return;

            _canTrigger = false;
            TriggerAtk();
        }

        /// <summary>
        /// 更新攻击间隔
        /// </summary>
        /// <param name="cdIncrement">攻击间隔增量，负数为减</param>
        public void UpdateAtkCd(float cdIncrement) => triggerCd = SpellData.Instance.defaultSummonCd + cdIncrement;

        private void TriggerAtk()
        {
            // 开始攻击瞬间的符文结算
            SpellScroll.Instance.ApplySpellOnTrigger();
            
            _nextTriggerTime = Time.time + triggerCd;
            
            // 计算射击方向
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            var position = transform.position;
            position.z = 0;
            _pointingDir = (mousePos - position).normalized;
            
            // 生成子弹
            if (ammoCount > 1)
            {
                var halfAngle = (ammoCount - 1) * BulletAngle * 0.5f;
                var pointingQAngle = Quaternion.FromToRotation(Vector3.up, _pointingDir);
                var qAngle = pointingQAngle * Quaternion.AngleAxis(-halfAngle, Vector3.forward);
                for (var i = 0; i < ammoCount; i++)
                {
                    var bullet = BulletPoolMgr.Instance.GetBullet();
                    bullet.transform.position = position;
                    var moveDir = (qAngle * Vector3.up).normalized;
                    bullet.SetBasicParam(this, 0f, moveDir);

                    qAngle *= _bulletInterQAngle;
                }
                return;
            }
            
            var bullet1 = BulletPoolMgr.Instance.GetBullet();
            bullet1.transform.position = position;
            bullet1.SetBasicParam(this, 0f, _pointingDir);
            
        }

    }
}