using System;
using System.Collections.Generic;
using R0.ScriptableObjConfig;
using R0.SpellRel;
using R0.Static;
using UnityEngine;
using R0.Weapons;
using Sirenix.OdinInspector;

namespace R0.Bullet
{
    /// <summary>
    /// 子弹类型
    /// </summary>
    [Serializable]
    public enum BulletType
    {
        [LabelText("法球")] MagicAmmo,
        [LabelText("弓箭")] Arrow,
        [LabelText("剑")] Sword,
    }
    
    /// <summary>
    /// 子弹基类，封装矢量移动逻辑
    /// </summary>
    public class Bullet : MonoBehaviour
    {
        /// <summary> transform缓存 </summary>
        private Transform _tCached, _imgTCached;
        
        /// <summary> 速度乘量 </summary> ///
        [GUIColor(0.3f, 0.8f, 0.8f, 1f)]
        [SerializeField, DisplayAsString] private float speedMultiplier;
        
        /// <summary> 移动速度 </summary> ///
        [GUIColor(0.3f, 0.8f, 0.8f, 1f)]
        [SerializeField, DisplayAsString] private float moveSpeed;
        
        /// <summary> 移动方向 </summary>///
        [SerializeField, DisplayAsString] private Vector3 moveDir;
        
        /// <summary> 生命终止时间 </summary>///
        [GUIColor(0.3f, 0.8f, 0.8f, 1f)]
        [SerializeField, DisplayAsString] private float lifeEndTime;
        
        /// <summary> 起始延时结束时间 </summary>///
        [GUIColor(0.3f, 0.8f, 0.8f, 1f)]
        [SerializeField, DisplayAsString] private float initWaitEndTime;
        
        /// <summary> 单次伤害 </summary>///
        [GUIColor(0.3f, 0.8f, 0.8f, 1f)]
        [SerializeField, DisplayAsString] private float dmg;
        
        /// <summary> buff效果 </summary> ///
        [GUIColor(0.3f, 0.8f, 0.8f, 1f)]
        [SerializeField, DisplayAsString] private SpellEffect effect;
        
        /// <summary> 附着元素 </summary> ///
        [GUIColor(0.3f, 0.8f, 0.8f, 1f)]
        [SerializeField, DisplayAsString] private List<SpellElement> elements;

        private bool _isCompleteInitWait;
        
        protected SpriteRenderer SpriteRenderer;
        protected bool IsBulletFacingDir;

        protected virtual void Awake()
        {
            _tCached = transform;
            SpriteRenderer = _tCached.Find("Image").GetComponent<SpriteRenderer>();
            _imgTCached = SpriteRenderer.transform;
        }

        /// <summary>
        /// 获取transform原引用
        /// </summary>
        /// <returns>缓存的transform原引用</returns>
        public ref Transform GetT() => ref _tCached;
        
        /// <summary>
        /// 获取子弹图片transform原引用
        /// </summary>
        /// <returns>缓存的子弹图片transform原引用</returns>
        public ref Transform GetImgT() => ref _imgTCached;
        
        private void SetBulletImgDir()
        {
            _imgTCached.rotation = IsBulletFacingDir 
                ? Quaternion.FromToRotation(Vector3.up, moveDir) 
                : Const.Qua.Zero;
        }

        private void Move()
        {
            _tCached.Translate(moveDir * Time.deltaTime * speedMultiplier * moveSpeed);
        }

        /// <summary>
        /// 重置基本参数
        /// </summary>
        public virtual void SetBasicParam(Weapon weapon, SpellCat spellCat, float initWaitTime, Vector3 dir)
        {
            var curTime = Time.time;
            initWaitEndTime = curTime + initWaitTime;
            _isCompleteInitWait = false;
            
            var data = (SpellData.BulletSpellDataStruct) SpellData.Instance.data[(int) spellCat];

            dmg = data.dmg * weapon.bulletDmgMultiplier;
            moveSpeed = dmg * data.dmgSpdRate;

            SpriteRenderer.sprite = data.bulletSprite;
            IsBulletFacingDir = data.isFacingDir;
            lifeEndTime = curTime + data.defaultLifeTime;
            effect = weapon.bulletEffect;
            moveDir = dir;
            SetBulletImgDir();
            
            elements.Clear();
            foreach (var ele in weapon.bulletElements) elements.Add(ele);
            
        }

        protected virtual void Update()
        {
            if (!_isCompleteInitWait)
            {
                if (Time.time > initWaitEndTime) _isCompleteInitWait = true;
                else return;
            }
            
            if (Time.time > lifeEndTime) BulletPoolMgr.Instance.Recycle(this);
            Move();
        }
        
    }
}