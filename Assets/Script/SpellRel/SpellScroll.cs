﻿using System;
using System.Collections.Generic;
using R0.ScriptableObjConfig;
using R0.SingaltonBase;
using R0.Static;
using R0.Weapons;
using UnityEngine;

namespace R0.SpellRel
{
    /// <summary>
    /// 符文卷轴类
    /// </summary>
    public class SpellScroll : SingletonBehaviour<SpellScroll>
    {
        /// <summary> 符文列表 </summary> ///
        [SerializeField] private List<Spell> spells;

        /// <summary> 激活到第几个符文 </summary> ///
        [SerializeField] private byte activeSpellIndex;
        
        /// <summary> 符文能量 </summary> ///
        [SerializeField] private float power;

        protected override void OnEnableInit()
        {
            PreApplyPowerFreeSpell();
            GetComponent<SpellScrollViewer>().Init();
            power = SpellData.Instance.initSpellPower;
        }

        /// <summary>
        /// 获取符文列表原引用
        /// </summary>
        /// <returns></returns>
        public ref List<Spell> GetSpells() => ref spells;

        /// <summary>
        /// 添加spell至符文列表
        /// </summary>
        /// <param name="spell">添加的符文</param>
        public void AppendSpell(Spell spell)
        {
            if (spells.Count > SpellData.Instance.maxSpellCapacity)
            {
                // TODO : UI面板移除选择的符文
            }
            spells.Add(spell);
            SpellScrollViewer.Instance.UpdateSpellScrollHud();
        }
        
        /// <summary>
        /// 将第index个符文移出符文列表
        /// </summary>
        /// <param name="index">移除目标符文在列表里的角标</param>
        public void RemoveSpellAt(int index)
        {
            if (index < 0 || index >= spells.Count) return;
            spells.RemoveAt(index);
            SpellScrollViewer.Instance.UpdateSpellScrollHud();
        }

        /// <summary>
        /// 预结算不消耗能量的符文效果
        /// </summary>
        public void PreApplyPowerFreeSpell()
        {
            var count = Math.Min(spells.Count, SpellData.Instance.powerFreeSpellCount);
            for (var i = 0; i < count; i++)
            {
                spells[i].Apply();
            }
        }

        /// <summary>
        /// 应用并结算符文效果
        /// </summary>
        public void ApplySpellOnTrigger()
        {
            // 计算支持生效的符文数，不考虑非能耗符文
            var spellDataObj = SpellData.Instance;
            var supported = Mathf.CeilToInt(power / spellDataObj.powerPerFrame);
            supported = Math.Min(supported, activeSpellIndex);

            if (supported < spellDataObj.powerFreeSpellCount) return;

            var spellData = spellDataObj.spellData;
            for (var i = spellDataObj.powerFreeSpellCount; i < supported; i++)
            {
                var spell = spells[i];

                // 超出部分计算符文消耗
                var remain = power - spellData[spell.id].powerCost;
                if (remain < 0) return;
                power = remain;
                
                // 应用符文属性
                spell.Apply();
            }
        }
        
    }
}