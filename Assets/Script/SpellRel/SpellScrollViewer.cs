using System;
using R0.Character;
using R0.ScriptableObjConfig;
using R0.SingaltonBase;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace R0.SpellRel
{
    public class SpellScrollViewer : SingletonBehaviour<SpellScrollViewer>
    {
        public bool isSpellFacingDir;
        public float radius;
        public Transform spellHome;
        
        private Image _barImg;
        private Quaternion _startQAngle, _interQAngle;

        [Button("更新UI显示", ButtonSizes.Large), GUIColor(0.4f, 0.8f, 1)]
        private void InspectorUpdateSpellHud()
        {
            var count = SpellData.Instance.maxSpellCapacity;

            var angle = 360f / count;
            _interQAngle = Quaternion.AngleAxis(angle, Vector3.forward);
            
            if (count % 2 == 0) angle = 180f + 0.5f * angle;
            else angle = 180f;
            _startQAngle = Quaternion.AngleAxis(angle, Vector3.forward);
            
            var t = spellHome.transform;
            var q = _startQAngle;
            for (var i = 0; i < t.childCount; i++)
            {
                var tr = t.GetChild(i).transform;
                tr.position = spellHome.transform.position + radius * (q * Vector3.up).normalized;
                if (isSpellFacingDir) tr.rotation = q;
                q *= _interQAngle;
            }
        } 

        protected override void OnEnableInit() { }

        public void Awake()
        {
            var t = transform;
            _barImg = t.Find("PowerBar").GetComponent<Image>();

            var count = SpellData.Instance.maxSpellCapacity;

            var angle = 360f / count;
            _interQAngle = Quaternion.AngleAxis(angle, Vector3.forward);
            
            if (count % 2 == 0) angle = 180f + 0.5f * angle;
            else angle = 180f;
            _startQAngle = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        /// <summary>
        /// 更新能量条UI显示
        /// </summary>
        public void UpdatePowerBarHud(float curPower)
        {
            var per = curPower / SpellData.Instance.maxSpellPower;
            _barImg.fillAmount = 0.7f * per + 0.3f;
        }

        public void UpdateSpellScrollHud(SpellScroll spellScroll)
        {
            var spells = spellScroll.GetSpells();
            var q = _startQAngle;
            foreach (var t in spells)
            {
                t.transform.position = spellHome.transform.position + radius * (q * Vector3.up).normalized;
                if (isSpellFacingDir) t.transform.rotation = q;
                q *= _interQAngle;
            }
        }
        
    }
}