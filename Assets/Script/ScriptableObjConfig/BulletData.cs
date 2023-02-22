using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace R0.ScriptableObjConfig
{
    [CreateAssetMenu(fileName = "BulletData", menuName = "子弹数值", order = 0)]
    public class BulletData : SingletonScriptableObj<BulletData>
    {
        [Serializable]
        public class BulletDataStruct
        {
            [ToggleLeft, LabelText("单项面板锁"), VerticalGroup("row1/left")]
            [SerializeField] private bool isSpellInfoLocked;
            
            [LabelText("名称"), DisableIf("isSpellInfoLocked")]
            [VerticalGroup("row1/left"), TableColumnWidth(200)]
            public string name;
            
            [LabelText("移动速度"), DisableIf("isSpellInfoLocked")]
            [VerticalGroup("row1/left"), SuffixLabel("unit/16ms", overlay: true)]
            public float moveSpeed;

            [LabelText("单次伤害"), VerticalGroup("row1/left"), DisableIf("isSpellInfoLocked")]
            public float dmg;

            [LabelText("图片朝向速度方向"), DisableIf("isSpellInfoLocked")]
            [TableColumnWidth(30), VerticalGroup("row2")]
            public bool isFacingDir;
            
            [HideLabel, PreviewField(58, ObjectFieldAlignment.Right)]
            [HorizontalGroup("row1", 58), VerticalGroup("row1/right"), DisableIf("isSpellInfoLocked")]
            public Sprite sprite;
            
            [LabelText("生命时长"), VerticalGroup("row2"), DisableIf("isSpellInfoLocked")]
            public float defaultLifeTime;

            [TextArea, LabelText("描述"), VerticalGroup("row3"), DisableIf("isSpellInfoLocked")]
            public string description;

            [DisplayAsString, VerticalGroup("row3")] public float dmgSpdRate;
        }

        [TableList]
        [LabelText("【各类型子弹属性】")]
        public BulletDataStruct[] bulletData;

        /// <summary>
        /// 更新伤害 / 速度 比例
        /// </summary>
        public void UpdateDmgSpdRate()
        {
            foreach (var d in bulletData) d.dmgSpdRate = d.moveSpeed / d.dmg;
        }
        
    }
}