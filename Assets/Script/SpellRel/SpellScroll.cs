using System.Collections.Generic;
using R0.ScriptableObjConfig;
using R0.Static;

namespace R0.SpellRel
{
    /// <summary>
    /// 符文卷轴类
    /// </summary>
    public class SpellScroll
    {
        /// <summary> 符文列表 </summary>
        private List<Spell> _spells;

        /// <summary> 符文能量 </summary>
        private float _power;
        
        public SpellScroll()
        {
            _spells = new List<Spell>();
        }

        /// <summary>
        /// 添加spell至符文列表
        /// </summary>
        /// <param name="spell">添加的符文</param>
        public void AppendSpell(Spell spell)
        {
            if (_spells.Count > SpellScrollData.Instance.maxSpellCapacity)
            {
                // TODO : UI面板移除选择的符文
            }
            _spells.Add(spell);
            SpellScrollViewer.Instance.UpdateSpellScrollHud();
        }
        
        /// <summary>
        /// 将第index个符文移出符文列表
        /// </summary>
        /// <param name="index">移除目标符文在列表里的角标</param>
        public void RemoveSpellAt(int index)
        {
            if (index < 0 || index >= _spells.Count) return;
            _spells.RemoveAt(index);
            SpellScrollViewer.Instance.UpdateSpellScrollHud();
        }
        
    }
}