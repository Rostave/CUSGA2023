using R0.EditorTool;
using UnityEngine;

namespace R0.ScriptableObjConfig
{
    [CreateAssetMenu(fileName = "符文卷轴数值", menuName = "符文卷轴数值", order = 0)]
    public class SpellScrollData : SingletonScriptableObj<SpellScrollData>
    {
        [Foldout("符文", true)]
        
        [CustomLabel("符文最大容纳量")]
        public byte maxSpellCapacity = 10;

        [CustomLabel("不消耗能量的符文数")]
        public byte powerFreeSpellCount = 3;
        
        
        [Foldout("符文能量", true)]
        
        [CustomLabel("最大能量容量")]
        public float maxSpellPower = 10f;
        
        [CustomLabel("初始能量")]
        public float initSpellPower = 1f;
        

    }
}