using System;
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

        private Image[] _spellImg;

        /// <summary>
        /// 初始化函数，为控制初始化顺序顾为手动调用
        /// </summary>
        public void Init()
        {
            var count = SpellData.Instance.maxSpellCapacity;
            _spellImg = new Image[count];

            var pos = transform.position;
            for (var i = 0; i < count; i++)
            {
                var frame = Instantiate(framePrefab, transform);
                frame.transform.position = pos;
                pos.x += frameInterval;
                _spellImg[i] = frame.transform.Find("SpellImg").GetComponent<Image>();
            }

            UpdateSpellScrollHud();
        }
        
        /// <summary>
        /// 刷新显示符文卷轴HUD上的符文图标，应在修改符文列表后调用以同步显示
        /// </summary>
        public void UpdateSpellScrollHud()
        {
            var spells = SpellScroll.Instance.GetSpells();
            var spellCount = spells.Count;
            var spellData = SpellData.Instance;
            for (var i = 0; i < _spellImg.Length; i++)
            {
                if (i >= spellCount) _spellImg[i].enabled = false;
                else
                {
                    _spellImg[i].enabled = true;
                    _spellImg[i].sprite = spellData.spellData[spells[i].id].sprite;
                }
            }
        }

        protected override void OnEnableInit() { }
    }
}