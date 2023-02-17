using System;
using R0.ScriptableObjConfig;
using Sirenix.OdinInspector;
using UnityEngine;

namespace R0.SpellRel
{
    /// <summary>
    /// 符文效果启用时机
    /// </summary>
    [Serializable]
    public enum SpellEffectActivationTime
    { 
        [LabelText("子弹发射前")] OnWeaponTrigger,
        [LabelText("子弹击中敌人后")] OnHitEnemy,
    }

    /// <summary>
    /// 符文基本效果
    /// </summary>
    [Serializable]
    public enum SpellEffect
    {
        [LabelText("敌人变速 <乘数>")] EnemySpeed,
        [LabelText("子弹变速 <乘数>")] BulletSpeed,
        [LabelText("子弹多发 <个数>")] BulletCount,
        [LabelText("射击cd变速 <乘数>")] SummonCd,
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
        public virtual void Apply(params object[] param)
        {
            var data = SpellData.Instance.spellData[id];
            
        }
        
    }
}