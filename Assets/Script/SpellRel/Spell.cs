using System;
using R0.Character;
using R0.ScriptableObjConfig;
using R0.Weapons;
using Sirenix.OdinInspector;
using UnityEngine;

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
    /// 基础符文类
    /// </summary>
    [Serializable]
    public class Spell
    {
        public SpellCat spellCat;

        /// <summary>
        /// 应用符文效果
        /// </summary>
        public virtual void Apply(BulletEmitter emitter)
        {
            var data = (SpellData.BulletSpellDataStruct) SpellData.Instance.data[(int) spellCat];
            var weapon = CharaMgr.Instance.activeChara.weapon;

            switch (data.effect)
            {
                case SpellEffect.BulletSummon:
                    SummonBullet();
                    break;
                case SpellEffect.ElementAttach:
                    AttachElement();
                    break;
                case SpellEffect.PropMod:
                    ModifyProperty();
                    break;
            }
        }

        private void SummonBullet()
        {
            
        }

        private void AttachElement()
        {
            
        }

        private void ModifyProperty()
        {
            
        }
        
    }
}