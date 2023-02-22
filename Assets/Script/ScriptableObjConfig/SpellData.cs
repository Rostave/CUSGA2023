using System;
using System.Collections.Generic;
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
            [ToggleLeft, LabelText("单项面板锁"), VerticalGroup("row1/left")]
            [SerializeField] private bool isSpellInfoLocked;

            [LabelText("名称"), VerticalGroup("row1/left"), DisableIf("isSpellInfoLocked")]
            public string name;

            [LabelText("类型"), VerticalGroup("row1/left"), DisableIf("isSpellInfoLocked")]
            public SpellCat cat;
            
            [LabelText("效果"), VerticalGroup("row1/left"), DisableIf("isSpellInfoLocked")]
            public SpellEffect effect;
            
            [LabelText("单次能耗"), VerticalGroup("row1/left"), DisableIf("isSpellInfoLocked")]
            public float powerCost;

            [ShowIf("@effect != SpellEffect.Element")]
            [LabelText("效果参数"), VerticalGroup("row1/left"), DisableIf("isSpellInfoLocked")]
            public List<float> effectParam;

            [TextArea, LabelText("描述"), VerticalGroup("row2"), DisableIf("isSpellInfoLocked")]
            public string description;

            [HideLabel]
            [PreviewField(58, ObjectFieldAlignment.Right)]
            [HorizontalGroup("row1", 58), VerticalGroup("row1/right"), DisableIf("isSpellInfoLocked")]
            public Sprite spellSprite;
        }

        [Serializable]
        public class BulletSpellDataStruct : SpellDataStruct
        {
            [LabelText("子弹伤害"), VerticalGroup("row1/left"), DisableIf("isSpellInfoLocked")]
            public float dmg;
            
            [LabelText("子弹飞行速度"), VerticalGroup("row1/left"), DisableIf("isSpellInfoLocked")]
            public float moveSpd;
            
            [LabelText("射击间隔"), VerticalGroup("row1/left"), DisableIf("isSpellInfoLocked")]
            public float cd;
            
            [LabelText("生命时长"), VerticalGroup("row1/left"), DisableIf("isSpellInfoLocked")]
            public float defaultLifeTime;

            [HideLabel]
            [PreviewField(58, ObjectFieldAlignment.Right)]
            [HorizontalGroup("row1", 58), VerticalGroup("row1/right"), DisableIf("isSpellInfoLocked")]
            public Sprite bulletSprite;
            
            [TextArea, LabelText("攻击形式"), VerticalGroup("row2"), DisableIf("isSpellInfoLocked")]
            public string atkDesc;

            [LabelText("散射随机角"), VerticalGroup("row2"), DisableIf("isSpellInfoLocked")]
            public float randomAngle;
            
            [LabelText("受伤次数销毁"), VerticalGroup("row1/left"), DisableIf("isSpellInfoLocked")]
            public int destroyOnPlayerDmgCount;
            
            [LabelText("使用次数销毁"), VerticalGroup("row1/left"), DisableIf("isSpellInfoLocked")]
            public int destroyOnUsaageCount;
            
            [LabelText("图片朝向速度方向"), DisableIf("isSpellInfoLocked")]
            [TableColumnWidth(30), VerticalGroup("row1")]
            public bool isFacingDir;
            
            [DisplayAsString, VerticalGroup("row3")] public float dmgSpdRate;
        }


        [Serializable]
        public class ElementSpellDataStruct : SpellDataStruct
        {
            
        }
        
        [FoldoutGroup("【武器属性】", true), LabelText("默认子弹召唤cd")]
        [SuffixLabel("sec", true)]
        [GUIColor(0.3f, 0.8f, 0.8f, 1f)]
        public float defaultSummonCd;
        
        [FoldoutGroup("【武器属性】", true), LabelText("相邻子弹间开角")]
        [SuffixLabel("<角度制浮点数>", true)]
        [GUIColor(0.3f, 0.8f, 0.8f, 1f)]
        public float bulletInterAngle;

        [FoldoutGroup("【符文卷轴】"), LabelText("最大符文容量")]
        [GUIColor(0.3f, 0.8f, 0.8f, 1f)]
        public byte maxSpellCapacity = 10;
        
        [FoldoutGroup("【符文卷轴】"), LabelText("不耗能符文数")]
        [GUIColor(0.3f, 0.8f, 0.8f, 1f)]
        public byte powerFreeSpellCount = 3;
        
        [FoldoutGroup("【符文卷轴】"), LabelText("最大能量容量")]
        [GUIColor(0.3f, 0.8f, 0.8f, 1f)]
        public float maxSpellPower = 200f;
        
        [FoldoutGroup("【符文卷轴】"), LabelText("初始能量")]
        [GUIColor(0.3f, 0.8f, 0.8f, 1f)]
        public float initSpellPower = 200f;
        
        [FoldoutGroup("【符文卷轴】"), LabelText("一格表示的能量")]
        [GUIColor(0.3f, 0.8f, 0.8f, 1f)]
        public float powerPerFrame = 20f;

        // [Space, Space]
        // [InfoBox("符文`优先级`属性（整数）越小，优先级越高（越先生效）")]
        
        [TableList]
        [LabelText("【各类型符文属性】")]
        public List<SpellDataStruct> spellData;
        
        [Button("从xlsx导入符文数据", ButtonSizes.Large), GUIColor(0.4f, 0.8f, 1)]
        private void UpdatePowerFreeSpellEffect() => ExcelImporter.ImportData();
        
        /// <summary>
        /// 更新伤害 / 速度 比例
        /// </summary>
        public void UpdateDmgSpdRate()
        {
            foreach (var d in spellData)
            {
                if (d is not BulletSpellDataStruct bd) continue;
                bd.dmgSpdRate = bd.moveSpd / bd.dmg;
                
            }
        }

    }
}