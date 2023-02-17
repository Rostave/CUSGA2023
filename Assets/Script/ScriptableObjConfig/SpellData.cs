using System;
using R0.SpellRel;
using Sirenix.OdinInspector;
using UnityEngine;

namespace R0.ScriptableObjConfig
{
    [CreateAssetMenu(fileName = "SpellData", menuName = "符文数值", order = 0)]
    public class SpellData : SingletonScriptableObj<SpellData>
    {
        [Serializable]
        public class SpellDataStruct
        {
            [LabelText("名称")]
            [VerticalGroup("row1/left")]
            public string name;

            [LabelText("单次能耗")]
            [VerticalGroup("row1/left")]
            public float powerCost;

            [LabelText("效果")]
            [VerticalGroup("row1/left")]
            public SpellEffect effect;
            
            [LabelText("效果发动时机")]
            [VerticalGroup("row1/left")]
            public SpellEffectActivationTime activationTime;

            [LabelText("效果参数")]
            [VerticalGroup("row1/left")]
            public float effectParam;

            [TextArea]
            [LabelText("描述")]
            public string description;
            
            [HideLabel]
            [PreviewField(58, ObjectFieldAlignment.Right)]
            [HorizontalGroup("row1", 58), VerticalGroup("row1/right")]
            public Sprite sprite;
        }
        
        [FoldoutGroup("【符文卷轴】")]
        [LabelText("最大符文容量")]
        [GUIColor(0.3f, 0.8f, 0.8f, 1f)]
        public byte maxSpellCapacity = 10;
        
        [FoldoutGroup("【符文卷轴】")]
        [LabelText("不耗能符文数")]
        [GUIColor(0.3f, 0.8f, 0.8f, 1f)]
        public byte powerFreeSpellCount = 3;
        
        [FoldoutGroup("【符文卷轴】")]
        [LabelText("最大能量容量")]
        [GUIColor(0.3f, 0.8f, 0.8f, 1f)]
        public float maxSpellPower = 10f;
        
        [FoldoutGroup("【符文卷轴】")]
        [LabelText("初始能量")]
        [GUIColor(0.3f, 0.8f, 0.8f, 1f)]
        public float initSpellPower = 1f;
        
        [TableList]
        [LabelText("【各类型符文属性】")]
        public SpellDataStruct[] spellData;
    }
}