using System;
using R0.OtherMgrs;
using R0.ScriptableObjConfig;
using R0.Weapons;
using Sirenix.OdinInspector;
using UnityEngine;

namespace R0.SpellRel
{
    /// <summary>
    /// 符文基本效果
    /// </summary>
    [Serializable]
    public enum SpellEffect
    {
        [LabelText("召唤法球")] SummonMagicAmmo,
        [LabelText("召唤弓箭")] SummonArrow,
        [LabelText("召唤飞剑")] SummonSword,
        
        [LabelText("敌人变速 <乘数>")] EnemySpeed,
        [LabelText("子弹变速 <乘数>")] BulletSpeed,
        [LabelText("子弹多发 <个数>")] BulletCount,
        [LabelText("射击cd变速 <秒数>")] SummonCd,
    }
    
    /// <summary>
    /// 基础符文类
    /// </summary>
    [Serializable]
    public class Spell
    {
        public int id;

        /// <summary>
        /// 应用符文效果
        /// </summary>
        public virtual void Apply()
        {
            var data = SpellData.Instance.spellData[id];
            var weapon = CharaMgr.Instance.playerWeapon;
            
            switch (data.effect)
            {
                case SpellEffect.EnemySpeed:
                    weapon.bulletEffect = data.effect;
                    weapon.effectParamOnEnemy = data.effectParam;
                    break;
                case SpellEffect.BulletSpeed:
                    weapon.bulletSpeedMultiplier = data.effectParam;
                    break;
                case SpellEffect.BulletCount:
                    weapon.ammoCount = Mathf.FloorToInt(data.effectParam);
                    break;
                case SpellEffect.SummonCd:
                    weapon.UpdateAtkCd(data.effectParam);
                    break;
                default:
                    break;
            }
        }
        
    }
}