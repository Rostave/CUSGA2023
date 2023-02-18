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
            [LabelText("名称")]
            [VerticalGroup("row1/left")]
            [TableColumnWidth(200)]
            public string name;
            
            [LabelText("移动速度")]
            [VerticalGroup("row1/left")]
            [SuffixLabel("unit/16ms", overlay: true)]
            public float moveSpeed;

            [LabelText("单次伤害")]
            [VerticalGroup("row1/left")]
            public float dmg;

            [LabelText("图片朝向速度方向")]
            [TableColumnWidth(30)]
            [VerticalGroup("row2")]
            public bool isFacingDir;
            
            [HideLabel]
            [PreviewField(58, ObjectFieldAlignment.Right)]
            [HorizontalGroup("row1", 58), VerticalGroup("row1/right")]
            public Sprite sprite;
            
            [LabelText("生命时长")]
            [VerticalGroup("row2")]
            public float defaultLifeTime;

            [TextArea]
            [LabelText("描述")]
            [VerticalGroup("row3")]
            public string description;
        }
        
        [FoldoutGroup("【公有属性】", true), LabelText("子弹召唤cd")]
        [SuffixLabel("unit/16ms", true)]
        [GUIColor(0.3f, 0.8f, 0.8f, 1f)]
        public float defaultSummonCd;
        
        [TableList]
        [LabelText("【各类型子弹属性】")]
        public BulletDataStruct[] bulletData;
        
    }
}