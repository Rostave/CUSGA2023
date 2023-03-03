using System;
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
        [SerializeField] private List<Spell> spells;

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
        public void ApplyNonBulletSpell()
        {
            // 计算支持生效的符文数
            var spellDataObj = SpellData.Instance;
            var supported = Mathf.CeilToInt(power / spellDataObj.powerPerFrame);
            supported = Math.Min(supported, activeSpellIndex);
            supported = Math.Min(supported, spells.Count);

            var spellData = spellDataObj.data;
            for (var i = 0; i < supported; i++)
            {
                var spell = spells[i];
                spell.isPowered = true;
                
                if (spellData[(int) spell.spellCat].effect == SpellEffect.BulletSummon) continue;
                
                // 超出部分计算符文的能量消耗
                if (i >= spellDataObj.powerFreeSpellCount)
                {
                    var remain = power - spellData[(int) spell.spellCat].powerCost;
                    remain = Mathf.Min(remain, 0f);  // 允许过量消耗
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

        [Space, Space, LabelText("符文预制体")] public GameObject spellPrefab;
        [Button("从xlsx更新符文数据", ButtonSizes.Large), GUIColor(0.4f, 0.8f, 1)]
        [DisableIf("@spellPrefab == null")]
        private void AddSpellFromInspector()
        {
            var spell = Instantiate(spellPrefab, transform);
            spellPrefab = null;
            AppendSpell(spell.GetComponent<Spell>());
            SpellScrollViewer.Instance.UpdateSpellScrollHud(this);
        }
        
    }
}