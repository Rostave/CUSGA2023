using System;
using System.Collections.Generic;
using R0.SingaltonBase;
using Unity.VisualScripting;
using UnityEngine;

namespace R0.Bullet
{
    /// <summary>
    /// 子弹对象池管理器。单例模式
    /// </summary>
    public class BulletPoolMgr : SingletonBehaviour<BulletPoolMgr>
    {
        public GameObject bulletPrefab;
        public Transform bulletHome;
        
        private Stack<Bullet> _bullets;

        protected override void OnEnableInit()
        {
            _bullets = new Stack<Bullet>();
        }

        /// <summary>
        /// 取出一个Bullet对象
        /// </summary>
        public Bullet GetBullet()
        {
            if (_bullets.Count == 0)
            {
                return Instantiate(bulletPrefab, bulletHome).GetComponent<Bullet>();
            }

            var ret = _bullets.Pop();
            ret.gameObject.SetActive(true);
            return ret;
        }

        /// <summary>
        /// 回收子弹Bullet对象
        /// </summary>
        public void Recycle(Bullet bullet)
        {
            _bullets.Push(bullet);
        }
        
    }
}