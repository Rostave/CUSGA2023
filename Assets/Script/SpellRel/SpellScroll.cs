using System;
using System.Collections.Generic;
using R0.ScriptableObjConfig;
using R0.SingaltonBase;
using R0.Static;
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
        private byte _activeSpellIndex;

        /// <summary> 符文能量 </summary> ///
        private float _power;
        private int _supportedSpellIndex;

        protected override void OnEnableInit()
        {
            _supportedSpellIndex = Mathf.CeilToInt(_power);
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
        /// 应用并结算 `子弹发射前` 的符文效果
        /// </summary>
        public void ApplySpellOnTrigger()
        {
            var supported = Mathf.CeilToInt(_power);
            if (supported == _supportedSpellIndex) return;  // 激活符文相同则没必要更新符文效果
            
            var activeIndex = _power < _activeSpellIndex ? Mathf.CeilToInt(_power) : _activeSpellIndex;
            for (var i = 0; i < activeIndex; i++)
            {
                var spellData = SpellData.Instance.spellData[i];
                if (spellData.activationTime != SpellEffectActivationTime.OnWeaponTrigger) continue;
                
                var spell = spells[i];

                // 超出部分计算符文消耗
                var data = SpellData.Instance;
                if (i >= data.powerFreeSpellCount)
                {
                    var remain = _power - data.spellData[spell.id].powerCost;
                    if (remain < 0) return;
                    _power = remain;
                }
                
                // 应用符文属性
                spell.Apply();
            }
        }
        
    }
}