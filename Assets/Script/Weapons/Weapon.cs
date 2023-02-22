using System;
using System.Collections.Generic;
using R0.Bullet;
using R0.Character;
using R0.ScriptableObjConfig;
using R0.SpellRel;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;


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
        private SpellCat _spellCat;  // 自己对应的子弹类型符文
        
        public BulletEmitter()
        {
            _canTrigger = true;
        }
        
        public BulletEmitter(Weapon weapon)
        {
            _canTrigger = true;
            _weapon = weapon;
        }

        public void BindWeapon(Weapon weapon, SpellCat spellCat)
        {
            _weapon = weapon;
            _spellCat = spellCat;
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

        public void GenBullets(BulletType bulletType)
        {
            // 生成子弹
            var curPos = _weapon.tCached.position;
            var ammoCount = _weapon.ammoCount;
            var data = (SpellData.BulletSpellDataStruct) SpellData.Instance.data[(int) _spellCat];
            var pointingQAngle = Quaternion.FromToRotation(Vector3.up, _weapon.pointingDir);
            
            if (ammoCount > 1)
            {
                var halfAngle = (ammoCount - 1) * _weapon.BulletAngle * 0.5f;
                var qAngle = pointingQAngle * Quaternion.AngleAxis(-halfAngle, Vector3.forward);
                for (var i = 0; i < ammoCount; i++)
                {
                    var bullet = BulletPoolMgr.Instance.GetBullet();
                    bullet.transform.position = curPos;
                    var randomAngle = Random.Range(-data.randomAngle, data.randomAngle);
                    var qua = qAngle * Quaternion.AngleAxis(randomAngle, Vector3.forward);
                    var moveDir = (qua * Vector3.up).normalized;
                    bullet.SetBasicParam(_weapon, _spellCat, 0f, moveDir);

                    qAngle *= _weapon.bulletInterQAngle;
                }
                return;
            }
            
            var bullet1 = BulletPoolMgr.Instance.GetBullet();
            bullet1.transform.position = _weapon.tCached.position;
            var randomAngle1 = Random.Range(-data.randomAngle, data.randomAngle);
            var qua1 = pointingQAngle * Quaternion.AngleAxis(randomAngle1, Vector3.forward);
            var moveDir1 = (qua1 * Vector3.up).normalized;
            bullet1.SetBasicParam(_weapon, _spellCat, 0f, moveDir1);
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
            SpellScroll.Instance.ApplySpellOnTrigger();
            
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
            ResetEmitters();
        }

        private void Update()
        {
            foreach (var e in emitters) e.UpdateTrigger();
        }
        
        /// <summary>
        /// 添加子弹附着的元素属性
        /// </summary>
        /// <param name="element">元素类型</param>
        public void AddElement(SpellElement element) => bulletElements.Add(element);

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
                var data = SpellData.Instance.data[(int) spell.spellCat];
                if (data.effect != SpellEffect.BulletSummon) continue;
                AddEmitter((BulletSpell) spell);
            }
        }
       
        private void AddEmitter(BulletSpell spell)
        {
            var emitter = BulletEmitterPoolMgr.GetEmitter();
            emitter.BindWeapon(this, spell.spellCat);
            spell.BindEmitter(emitter);
            emitters.Add(emitter);
        }
    }
}