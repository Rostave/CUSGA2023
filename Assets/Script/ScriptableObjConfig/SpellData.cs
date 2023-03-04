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
        /// <summary>
        /// 基本符文数据管理类
        /// </summary>
        [Serializable]
        public class SpellDataStruct
        {
            [ToggleLeft, LabelText("单项面板锁"), VerticalGroup("row1/left")]
            [SerializeField] public bool isSpellInfoLocked;

            [LabelText("名称"), VerticalGroup("row1/left"), DisableIf("isSpellInfoLocked")]
            public string name;

            [LabelText("类型"), VerticalGroup("row1/left"), DisableIf("isSpellInfoLocked")]
            public SpellCat cat;
            
            [LabelText("效果"), VerticalGroup("row1/left"), DisableIf("isSpellInfoLocked")]
            public SpellEffect effect;
            
            [LabelText("单次能耗"), VerticalGroup("row1/left"), DisableIf("isSpellInfoLocked")]
            public float powerCost;

            [TextArea, LabelText("描述"), VerticalGroup("row2"), DisableIf("isSpellInfoLocked")]
            public string description;

            [HideLabel]
            [PreviewField(58, ObjectFieldAlignment.Right)]
            [HorizontalGroup("row1", 58), VerticalGroup("row1/right"), DisableIf("isSpellInfoLocked")]
            public Sprite spellSprite;
        }

        /// <summary>
        /// 子弹类型符文数据管理类
        /// </summary>
        [Serializable]
        public class BulletSpellDataStruct : SpellDataStruct
        {
            [LabelText("子弹伤害")]
            [VerticalGroup("row1/left"), DisableIf("isSpellInfoLocked"), GUIColor(0.678f,0.95f,0.184f)]
            public float dmg;
            
            [LabelText("子弹飞行速度")]
            [VerticalGroup("row1/left"), DisableIf("isSpellInfoLocked"), GUIColor(0.678f,0.95f,0.184f)]
            public float moveSpd;
            
            [LabelText("速度伤害比")]
            [DisplayAsString, VerticalGroup("row1/left"), GUIColor(0.678f,0.95f,0.184f)] 
            public float dmgSpdRate;
            
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
            
            [LabelText("受伤次数销毁"), VerticalGroup("row2"), DisableIf("isSpellInfoLocked")]
            public int destroyOnPlayerDmgCount;
            
            [LabelText("使用次数销毁"), VerticalGroup("row2"), DisableIf("isSpellInfoLocked")]
            public int destroyOnUsaageCount;
            
            [LabelText("图片朝向速度方向"), VerticalGroup("row2"), DisableIf("isSpellInfoLocked")]
            public bool isFacingDir;
        }
        
        /// <summary>
        /// 元素符文数据管理类
        /// </summary>
        [Serializable]
        public class ElementSpellDataStruct : SpellDataStruct
        {
            [LabelText("元素属性"), VerticalGroup("row2"), DisableIf("isSpellInfoLocked")]
            public SpellElement elementType;
        }
        
        /// <summary>
        /// 属性符文数据管理类
        /// </summary>
        [Serializable]
        public class PropModSpellDataStruct : SpellDataStruct
        {
            
        }
        
        /// <summary>
        /// 特殊符文数据管理类
        /// </summary>
        [Serializable]
        public class SpecialSpellDataStruct : SpellDataStruct
        {
            [ShowIf("@effect != SpellEffect.Element")]
            [LabelText("效果参数"), VerticalGroup("row1/left"), DisableIf("isSpellInfoLocked")]
            public List<float> effectParam;
        }

        // 公有属性 ================================================
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

        // 独有属性 ================================================
        // [Space, Space]
        // [InfoBox("符文`优先级`属性（整数）越小，优先级越高（越先生效）")]

        [Space, Space, TableList, LabelText("【子弹型符文属性】")] 
        public List<BulletSpellDataStruct> bulletSpellData;
        
        [TableList, LabelText("【元素型符文属性】")] 
        public List<ElementSpellDataStruct> elementSpellData;
        
        [TableList, LabelText("【属性型符文属性】")] 
        public List<PropModSpellDataStruct> propModSpellData;
        
        [TableList, LabelText("【特殊类型符文属性】")] 
        public List<SpecialSpellDataStruct> specialSpellData;
            
        [HideInInspector]
        public List<SpellDataStruct> data;  // 所有spell
        
        [Button("从xlsx更新符文数据", ButtonSizes.Large), GUIColor(0.4f, 0.8f, 1)]
        private void UpdatePowerFreeSpellEffect() => ExcelImporter.ImportSpellData();

        [SerializeField, DisplayAsString] private int eleOffset, propOffset, specialOffset;
        public BulletSpellDataStruct GetBulletSpellData(SpellCat spellCat) => bulletSpellData[(int) spellCat];
        public ElementSpellDataStruct GetElementtSpellData(SpellCat spellCat) => elementSpellData[(int) spellCat - eleOffset];
        public PropModSpellDataStruct GetPropModSpellData(SpellCat spellCat) => propModSpellData[(int) spellCat - propOffset];
        public SpecialSpellDataStruct GetSpecialSpellData(SpellCat spellCat) => specialSpellData[(int) spellCat - specialOffset];
       

        /// <summary>
        /// 汇总所有符文
        /// </summary>
        public void IntegrateSpells()
        {
            UpdateDmgSpdRate();
            foreach (var s in bulletSpellData) data.Add(s);
            foreach (var s in elementSpellData) data.Add(s);
            foreach (var s in propModSpellData) data.Add(s);
            foreach (var s in specialSpellData) data.Add(s);
            
            eleOffset = bulletSpellData.Count;
            propOffset = eleOffset + elementSpellData.Count;
            specialOffset = propOffset + propModSpellData.Count;
        }
        
        /// <summary>
        /// 更新伤害 / 速度 比例
        /// </summary>
        private void UpdateDmgSpdRate()
        {
            foreach (var d in bulletSpellData)
            {
                d.dmgSpdRate = d.moveSpd / d.dmg;
            }
        }

        // /// <summary>
        // /// 清空所有符文列表
        // /// </summary>
        // public void ClearSpellList()
        // {
        //     data.Clear();
        //     bulletSpellData.Clear();
        //     propModSpellData.Clear();
        //     elementSpellData.Clear();
        //     bulletSpellData.Clear();
        //     specialSpellData.Clear();
        // }
    }
}