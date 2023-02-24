using System;
using R0.Bullet;
using R0.Character;
using R0.ScriptableObjConfig;
using R0.Weapons;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace R0.SpellRel
{
    /// <summary>
    /// 游戏内符文种类
    /// </summary>
    [Serializable]
    public enum SpellCat
    {
        [LabelText("黑钢巨剑")] SummonSword,
        [LabelText("镜之聚集")] SummonBeam,
        [LabelText("秘术法球")] SummonMagic,

        // [LabelText("冰元素符文")] ElementCrysto,
        [LabelText("心如火焚")] ElementPyro,
        [LabelText("巨蛇之牙")] ElementToxico,
        
        [LabelText("奥尔迡东之怒")] PropDmgInc,
        [LabelText("耶莱的恩赐")] PropBothDmgInc,
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
        public bool isActive = true;
        public bool isPowered = true;

        protected Image Img;

        /// <summary>
        /// 应用符文效果
        /// </summary>
        public abstract void Apply();

        protected virtual void Start()
        {
            Img = GetComponent<Image>();
        }
    }
    
}