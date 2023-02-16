using UnityEngine;

namespace R0.ScriptableObjConfig
{
    [CreateAssetMenu(fileName = "符文卷轴数值", menuName = "符文卷轴数值", order = 0)]
    public class SpellScrollData : SingaltonScriptableObj<SpellScrollData>
    {
        [CustomLabel("容纳最大符文数")]
        public byte maxSpellCapacity = 10;

        [CustomLabel("初始能量")]
        public byte initPower = 1;
        

    }
}