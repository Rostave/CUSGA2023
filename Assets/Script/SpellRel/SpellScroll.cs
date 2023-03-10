﻿using System;
using System.Collections.Generic;
using R0.ScriptableObjConfig;
using R0.SingaltonBase;
using R0.Weapons;
using Sirenix.OdinInspector;
using UnityEngine;

namespace R0.SpellRel
{
    /// <summary>
    /// 符文卷轴类
    /// </summary>
    public class SpellScroll : MonoBehaviour
    {
        /// <summary> 符文列表 </summary> ///
        [SerializeField, DisplayAsString] private List<Spell> spells;

        /// <summary> 激活到第几个符文 </summary> ///
        [SerializeField] private byte activeSpellIndex;
        
        /// <summary> 符文能量 </summary> ///
        [SerializeField] private float power;
        public float Power
        {
            get => power;
            set
            {
                power = value;
                SpellScrollViewer.Instance.UpdatePowerBarHud(power);
            }
        }

        private void Start()
        {
            Power = SpellData.Instance.initSpellPower;
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
        }
        
        /// <summary>
        /// 将第index个符文移出符文列表
        /// </summary>
        /// <param name="index">移除目标符文在列表里的角标</param>
        public void RemoveSpellAt(int index)
        {
            if (index < 0 || index >= spells.Count) return;
            spells.RemoveAt(index);
        }

        /// <summary>
        /// 应用并结算非子弹类型符文效果，更新符文的isPowered标识
        /// </summary>
        public void ApplyNonBulletSpell(Spell triggerBulletSpell)
        {
            // 计算支持生效的符文数
            var spellDataObj = SpellData.Instance;
            var supported = Mathf.CeilToInt(power / spellDataObj.powerPerFrame);
            // Debug.Log(supported);
            supported = Math.Min(supported, activeSpellIndex);
            // Debug.Log(supported);
            supported = Math.Min(supported, spells.Count);
            // Debug.Log(supported);
            
            var remain = power;
            var spellData = spellDataObj.data;
            for (var i = 0; i < supported; i++)
            {
                var spell = spells[i];
                spell.isPowered = true;

                if (spellData[(int) spell.spellCat].effect == SpellEffect.BulletSummon)
                {
                    if (spell == triggerBulletSpell && i >= spellDataObj.powerFreeSpellCount)
                    {
                        remain -= spellData[(int) triggerBulletSpell.spellCat].powerCost;
                    }
                    remain = Mathf.Max(remain, 0f);
                    Power = remain;
                    continue;
                }

                // 超出部分计算符文的能量消耗
                if (i >= spellDataObj.powerFreeSpellCount)
                {
                    remain -= spellData[(int) spell.spellCat].powerCost;
                    remain = Mathf.Max(remain, 0f);  // 允许过量消耗
                    Power = remain;
                }

                // 应用符文属性
                spell.Apply();
            }

            for (var i = supported; i < spells.Count; i++)
            {
                var spell = spells[i];
                spell.isPowered = false;
            }
        }

        [Space, Space, LabelText("符文预制体")] public List<GameObject> spellPrefab;
        [Button("添加至符文卷轴", ButtonSizes.Large), GUIColor(0.4f, 0.8f, 1)]
        [DisableIf("@spellPrefab.Count == 0"), DisableInEditorMode]
        private void AddSpellFromInspector()
        {
            foreach (var s in spellPrefab)
            {
                var spell = Instantiate(s, SpellScrollViewer.Instance.spellHome);
                AppendSpell(spell.GetComponent<Spell>());
            }
            
            spellPrefab.Clear();
            SpellScrollViewer.Instance.UpdateSpellScrollHud(this);
        }

        
        public void AddSpell(SpellData.SpellDataStruct spellStruct)
        {
            
        }
        
    }
}