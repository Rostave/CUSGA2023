using System;
using System.Collections.Generic;
using R0.Bullet;
using R0.Character;
using R0.ScriptableObjConfig;
using R0.SpellRel;
using Sirenix.OdinInspector;
using UnityEngine;


namespace R0.Weapons
{
    /// <summary>
    /// 子弹发射器对象池管理
    /// </summary>
    public static class BulletEmitterPoolMgr
    {
        private static Queue<BulletEmitter> _emitters;
        public static BulletEmitter GetEmitter() => _emitters.Count == 0 ? new BulletEmitter() : _emitters.Dequeue();
        public static void Recycle(BulletEmitter emitter) => _emitters.Enqueue(emitter);
    }
    
    /// <summary>
    /// 子弹发射器
    /// </summary>
    [Serializable]
    public class BulletEmitter
    {
        private BulletType _bulletType;
        public BulletEmitter()
        {
            _canTrigger = true;
        }
        
        public BulletEmitter(Weapon weapon, BulletType bulletType)
        {
            _canTrigger = true;
            _weapon = weapon;
            _bulletType = bulletType;
        }

        public void BindWeapon(Weapon weapon, BulletType bulletType)
        {
            _weapon = weapon;
            _bulletType = bulletType;
        }

        [GUIColor(0.3f, 0.8f, 0.8f, 1f)]
        [SerializeField, DisplayAsString] private float triggerCd;
        
        private Weapon _weapon;
        private float _nextTriggerTime;
        private bool _canTrigger;

        public void UpdateTrigger()
        {
            if (!_canTrigger)
            {
                if (Time.time > _nextTriggerTime) _canTrigger = true;
                else return;
            }
           
            // if (!Input.GetMouseButton(0)) return;

            _canTrigger = false;
            TriggerAtk();
        }

        /// <summary>
        /// 更新攻击间隔
        /// </summary>
        /// <param name="cdIncrement">攻击间隔增量，负数为减</param>
        public void UpdateAtkCd(float cdIncrement) => triggerCd = SpellData.Instance.defaultSummonCd + cdIncrement;

        public void GenBullets()
        {
            // 生成子弹
            var curPos = _weapon.tCached.position;
            var ammoCount = _weapon.ammoCount;
            if (ammoCount > 1)
            {
                var halfAngle = (ammoCount - 1) * _weapon.BulletAngle * 0.5f;
                var pointingQAngle = Quaternion.FromToRotation(Vector3.up, _weapon.pointingDir);
                var qAngle = pointingQAngle * Quaternion.AngleAxis(-halfAngle, Vector3.forward);
                for (var i = 0; i < ammoCount; i++)
                {
                    var bullet = BulletPoolMgr.Instance.GetBullet();
                    bullet.transform.position = curPos;
                    var moveDir = (qAngle * Vector3.up).normalized;
                    bullet.SetBasicParam(_weapon, 0f, moveDir);

                    qAngle *= _weapon.bulletInterQAngle;
                }
                return;
            }
            
            var bullet1 = BulletPoolMgr.Instance.GetBullet();
            bullet1.transform.position = _weapon.tCached.position;
            bullet1.SetBasicParam(_weapon, 0f, _weapon.pointingDir);
        }

        private void TriggerAtk()
        {
            // 计算射击方向
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            var position = _weapon.tCached.position;
            position.z = 0;
            _weapon.pointingDir = (mousePos - position).normalized;
            
            // 开始攻击瞬间的符文结算
            _weapon.bulletElements.Clear();
            SpellScroll.Instance.ApplySpellOnTrigger(this);
            
            _nextTriggerTime = Time.time + triggerCd;
        }
    }

    /// <summary>
    /// 召唤物（子弹）管理类
    /// </summary>
    public class Weapon : MonoBehaviour
    {
        [HideInInspector] public Transform tCached;
        [SerializeField] private List<BulletEmitter> emitters;
        
        private BulletEmitter _defaultEmitter;  // 默认发射器
        public BulletType defaultBulletType;
        
        // 效果参数
        public float bulletDmgMultiplier = 1f;
        public SpellEffect bulletEffect;
        public int ammoCount;
        public List<SpellElement> bulletElements;
        
        [HideInInspector] public Vector3 pointingDir;
        [HideInInspector] public Quaternion bulletInterQAngle;
        private float _bulletAngle;  // 相邻子弹间开角 
        
        public float BulletAngle
        {
            get => _bulletAngle;
            set
            {
                _bulletAngle = value;
                bulletInterQAngle = Quaternion.AngleAxis(_bulletAngle, Vector3.forward);
            }
        }

        private void Start()
        {
            BulletAngle = SpellData.Instance.bulletInterAngle;
            tCached = transform;
            _defaultEmitter = new BulletEmitter(this, defaultBulletType);
            ResetEmitters();
        }

        private void Update()
        {
            _defaultEmitter.UpdateTrigger();
            foreach (var e in emitters) e.UpdateTrigger();
        }
        
        /// <summary>
        /// 添加子弹附着的元素属性
        /// </summary>
        /// <param name="element">元素类型</param>
        public void AddElement(SpellElement element) => bulletElements.Add(element);

        /// <summary>
        /// 更新射击cd
        /// </summary>
        /// <param name="cdIncrement"></param>
        public void UpdateAtkCd(float cdIncrement)
        {
            foreach (var e in emitters) e.UpdateAtkCd(cdIncrement);
        }

        /// <summary>
        /// （符文改变后）重置发射器列表
        /// </summary>
        public void ResetEmitters()
        {
            foreach (var e in emitters) BulletEmitterPoolMgr.Recycle(e);
            var chara = CharaMgr.Instance.activeChara;
            var spells = chara.spellScroll.GetSpells();
            foreach (var spell in spells)
            {
                var data = SpellData.Instance.spellData[(int) spell.spellCat];
                if (data.effect != SpellEffect.SummonBullet) continue;
                AddEmitter(chara.bulletType);
            }
        }
       
        private void AddEmitter(BulletType bulletType)
        {
            var emitter = BulletEmitterPoolMgr.GetEmitter();
            emitter.BindWeapon(this, bulletType);
            emitters.Add(emitter);
        }
    }
}