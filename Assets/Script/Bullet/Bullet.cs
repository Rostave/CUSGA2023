using System;
using R0.ScriptableObjConfig;
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
        [LabelText("法球")] MgicAmmo, 
        [LabelText("弓箭")] Arrow, 
        [LabelText("飞剑")] Sword, 
    }
    
    /// <summary>
    /// 子弹基类，封装矢量移动逻辑
    /// </summary>
    public class Bullet : MonoBehaviour
    {
        public BulletType type;
        
        /// <summary> transform缓存 </summary>
        private Transform _tCached, _imgTCached;
        
        /// <summary> 速度乘量 </summary> ///
        private float _speedMultiplier;
        
        /// <summary> 移动速度 </summary> ///
        private float _moveSpeed;
        
        /// <summary> 移动方向 </summary>///
        private Vector3 _moveDir;
        
        /// <summary> 生命终止时间 </summary>///
        private float _lifeEndTime;
        
        /// <summary> 起始延时结束时间 </summary>///
        private float _initWaitEndTime;
        
        /// <summary> 单次伤害 </summary>///
        private float _dmg;
        
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
            if (IsBulletFacingDir) _imgTCached.rotation = Quaternion.FromToRotation(Vector3.up, _moveDir);
        }

        private void Move()
        {
            _tCached.Translate(_moveDir * Time.deltaTime * _speedMultiplier * _moveSpeed);
        }

        /// <summary>
        /// 重置基本参数
        /// </summary>
        public virtual void SetBasicParam(Weapon weapon, float initWaitTime)
        {
            var curTime = Time.time;
            _initWaitEndTime = curTime + initWaitTime;
            
            var data = BulletData.Instance.bulletData[(int) type];
            _moveSpeed = data.moveSpeed;
            IsBulletFacingDir = data.isFacingDir;
            
            _lifeEndTime = curTime + data.defaultLifeTime;
            _speedMultiplier = weapon.bulletSpeedMultiplier;
            _moveDir = weapon.pointingDir;
            SetBulletImgDir();

            _dmg = data.dmg * weapon.bulletDmgMultiplier;
        }

        /// <summary>
        /// 回收子弹进对象池
        /// </summary>
        public void Recycle()
        {
            
        }

        protected virtual void Update()
        {
            Move();
        }
        
    }
}