using R0.Bullet;
using R0.Character;
using R0.ScriptableObjConfig;
using UnityEngine;

namespace R0.SpellRel
{
    public class BulletSpell : Spell
    {
        private bool _canTrigger;
        private float _nextTriggerTime, _triggerCd;

        public override void Apply()
        {
            var bulletType = spellCat switch
            {
                SpellCat.SummonBeam => BulletType.MagicAmmo,
                SpellCat.SummonMagic => BulletType.Arrow,
                SpellCat.SummonSword => BulletType.Sword,
            };
            GenBullets(bulletType);
        }

        protected override void Start()
        {
            base.Start();
            var data = SpellData.Instance.GetBulletSpellData(spellCat);
            _triggerCd = data.cd;
        }

        private void Update()
        {
            if (!isActive || !isPowered) return;
            if (!_canTrigger)
            {
                if (Time.time > _nextTriggerTime) _canTrigger = true;
                else return;
            }
           
            if (!Input.GetMouseButton(0)) return;

            _canTrigger = false;
            TriggerAtk();
        }
        
        private void TriggerAtk()
        {
            // 开始攻击瞬间的符文结算
            var chara = CharaMgr.Instance.activeChara;
            
            chara.weapon.ResetEffectParam();  // weapon重置效果参数
            chara.spellScroll.ApplyNonBulletSpell();  // 结算其他非子弹类型符文
            
            Apply();  // 自己结算
            _nextTriggerTime = Time.time + _triggerCd;
        }

        private void GenBullets(BulletType bulletType)
        {
            // 生成子弹
            var weapon = CharaMgr.Instance.activeChara.weapon;
            var curPos = weapon.tCached.position;
            var ammoCount = weapon.ammoCount;
            var data = SpellData.Instance.GetBulletSpellData(spellCat);
            var pointingQAngle = weapon.GetPointingAngle();
            
            if (ammoCount > 1)
            {
                var halfAngle = (ammoCount - 1) * weapon.BulletAngle * 0.5f;
                var qAngle = pointingQAngle * Quaternion.AngleAxis(-halfAngle, Vector3.forward);
                for (var i = 0; i < ammoCount; i++)
                {
                    var bullet = BulletPoolMgr.Instance.GetBullet();
                    bullet.transform.position = curPos;
                    var randomAngle = Random.Range(-data.randomAngle, data.randomAngle);
                    var qua = qAngle * Quaternion.AngleAxis(randomAngle, Vector3.forward);
                    var moveDir = (qua * Vector3.up).normalized;
                    bullet.SetBasicParam(weapon, spellCat, 0f, moveDir);

                    qAngle *= weapon.bulletInterQAngle;
                }
                return;
            }
            
            var bullet1 = BulletPoolMgr.Instance.GetBullet();
            bullet1.transform.position = weapon.tCached.position;
            var randomAngle1 = Random.Range(-data.randomAngle, data.randomAngle);
            var qua1 = pointingQAngle * Quaternion.AngleAxis(randomAngle1, Vector3.forward);
            var moveDir1 = (qua1 * Vector3.up).normalized;
            bullet1.SetBasicParam(weapon, spellCat, 0f, moveDir1);
        }

    }
}