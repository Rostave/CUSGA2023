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
        private Stack<Bullet> _bullets;

        protected override void OnEnableInit()
        {
            _bullets = new Stack<Bullet>();
        }

        // /// <summary>
        // /// 取出一个Bullet对象
        // /// </summary>
        // public void GetBullet<>()
        // {
        //     
        // }
        
    }
}