using System;
using R0.Bullet;
using R0.Character;
using R0.ScriptableObjConfig;
using R0.Weapons;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace R0.SpellRel
{
    /// <summary>
    /// 游戏内符文种类
    /// </summary>
    [Serializable]
    public enum SpellCat
    {
        [LabelText("召唤法球")] SummonMagicAmmo,
        [LabelText("召唤弓箭")] SummonArrow,
        [LabelText("召唤剑")] SummonSword,
        
        [LabelText("召唤高效符文")] SummonCdDec,
        [LabelText("增伤符文")] DmgInc,
        [LabelText("多发符文")] AmmoInc,
        
        [LabelText("冰元素符文")] CrystoElement,
        [LabelText("火元素符文")] PyroElement,
        [LabelText("毒元素符文")] ToxicoElement,
    }
    
    /// <summary>
    /// 符文效果
    /// </summary>
    [Serializable]
    public enum SpellEffect
    {
        [LabelText("召唤子弹")] BulletSummon,
        [LabelText("元素附着")] ElementAttach,
        [LabelText("属性修改")] PropMod,
        
        // 其他符文
        [LabelText("子弹多发")] BulletCount,
    }

    /// <summary>
    /// 元素类型
    /// </summary>
    [Serializable]
    public enum SpellElement
    {
        [LabelText("冰")] Crysto,
        [LabelText("火")] Pyro,
        [LabelText("毒")] Toxico,
    }
    
    /// <summary>
    /// 基础符文抽象类
    /// </summary>
    public abstract class Spell : MonoBehaviour
    {
        public SpellCat spellCat;
        public bool isActive;
        public bool isPowered;

        /// <summary>
        /// 应用符文效果
        /// </summary>
        public abstract void Apply();
    }

    public class BulletSpell : Spell
    {
        private bool _canTrigger;
        private float _nextTriggerTime, _triggerCd;

        public override void Apply()
        {
            var bulletType = spellCat switch
            {
                SpellCat.SummonMagicAmmo => BulletType.MagicAmmo,
                SpellCat.SummonArrow => BulletType.Arrow,
                SpellCat.SummonSword => BulletType.Sword,
            };
            GenBullets(bulletType);
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
        
        public void GenBullets(BulletType bulletType)
        {
            // 生成子弹
            var weapon = CharaMgr.Instance.activeChara.weapon;
            var curPos = weapon.tCached.position;
            var ammoCount = weapon.ammoCount;
            var data = (SpellData.BulletSpellDataStruct) SpellData.Instance.data[(int) spellCat];
            var pointingQAngle = Quaternion.FromToRotation(Vector3.up, weapon.pointingDir);
            
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

        private void TriggerAtk()
        {
            // 计算射击方向
            var weapon = CharaMgr.Instance.activeChara.weapon;
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            var position = weapon.tCached.position;
            position.z = 0;
            weapon.pointingDir = (mousePos - position).normalized;
            
            // 开始攻击瞬间的符文结算
            weapon.bulletElements.Clear();
            SpellScroll.Instance.ApplySpellOnTrigger();
            Apply();  // 自己结算
            
            _nextTriggerTime = Time.time + _triggerCd;
        }

    }
}