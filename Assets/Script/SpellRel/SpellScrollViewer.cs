using System;
using R0.Character;
using R0.ScriptableObjConfig;
using R0.SingaltonBase;
using UnityEngine;
using UnityEngine.UI;

namespace R0.SpellRel
{
    public class SpellScrollViewer : SingletonBehaviour<SpellScrollViewer>
    {
        public GameObject framePrefab;
        public float frameInterval;
        
        private Image _barImg;
        private Vector3[] _framePos;
        
        protected override void OnEnableInit() { }
        
        public void Awake()
        {
            var t = transform;
            _barImg = t.Find("PowerBar/Bar").GetComponent<Image>();
            
            var count = SpellData.Instance.maxSpellCapacity;
            _framePos = new Vector3[count];

            var pos = t.position;
            for (var i = 0; i < count; i++)
            {
                var frame = Instantiate(framePrefab, transform);
                frame.transform.position = pos;
                _framePos[i] = pos;
                pos.x += frameInterval;
                frame.transform.SetAsFirstSibling();
            }
        }

        /// <summary>
        /// 更新能量条UI显示
        /// </summary>
        public void UpdatePowerBarHud(float curPower) => _barImg.fillAmount = curPower / SpellData.Instance.maxSpellPower;

        public void UpdateSpellScrollHud(SpellScroll spellScroll)
        {
            var spells = spellScroll.GetSpells();
            for (var i = 0; i < spells.Count; i++)
            {
                spells[i].transform.position = _framePos[i];
            }
        }
        
    }
}