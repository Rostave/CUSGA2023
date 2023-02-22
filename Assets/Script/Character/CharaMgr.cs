using System;
using System.Collections;
using System.Collections.Generic;
using R0.Bullet;
using R0.SingaltonBase;
using R0.SpellRel;
using R0.Weapons;
using Sirenix.OdinInspector;
using UnityEngine;
using Vacuname;

namespace R0.Character
{
    [Serializable]
    public enum Chara
    {
        [LabelText("流浪汉")] Tramp,
        [LabelText("公主")] Princess, 
        [LabelText("骑士")] Knight,
    }
    
    /// <summary>
    /// 角色组件组
    /// </summary>
    [Serializable]
    public class CharaComponentGroup
    {
        public Chara character;
        public Player playerCtrl;
        public Weapon weapon;
        public SpellScroll spellScroll;
    }

    /// <summary>
    /// 角色管理
    /// </summary>
    public class CharaMgr : SingletonBehaviour<CharaMgr>
    {
        /// <summary> 当前站场角色组件组 </summary> ///
        public CharaComponentGroup activeChara;
        [SerializeField, Space, Space] private List<CharaComponentGroup> characters;
        public Transform playerHome, spellSrollHome;

        [Space, Space, LabelText("角色切换等待时间")] public float charaSwitchWaitTime = 2f;
        private bool _canSwitchChara;
        private float _charaSwitchEndTime;
        private Coroutine _charaSwitchCoroutine;

        protected override void OnEnableInit() { }

        /// <summary>
        /// 切换角色
        /// </summary>
        /// <param name="character">要切换到的角色类型</param>
        public void SwitchChara(Chara character)
        {
            if (!_canSwitchChara) return;
            _canSwitchChara = false;

            _charaSwitchEndTime = Time.time + charaSwitchWaitTime;
            _charaSwitchCoroutine = StartCoroutine(SwitchCharaCoroutine(character));
        }

        /// <summary>
        /// 终止角色切换过程
        /// </summary>
        public void StopSwitchChara()
        {
            if (_charaSwitchCoroutine != null) StopCoroutine(_charaSwitchCoroutine);
        }

        private IEnumerator SwitchCharaCoroutine(Chara character)
        {
            if (Time.time < _charaSwitchEndTime)
            {
                // TODO : 跟新橘色切换UI进度条
                yield return null;
            }
            
            activeChara.playerCtrl.gameObject.SetActive(false);  // 隐藏老角色
            activeChara = characters[(int) character];           // ====== 更换玩家组件组 ======
            activeChara.playerCtrl.gameObject.SetActive(true);   // 显示新角色
        }
        
    }
}