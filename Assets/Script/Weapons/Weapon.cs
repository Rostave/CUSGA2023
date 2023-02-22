using System;
using System.Collections.Generic;
using R0.ScriptableObjConfig;
using R0.SpellRel;
using UnityEngine;


namespace R0.Weapons
{
    /// <summary>
    /// 召唤物（子弹）管理类
    /// </summary>
    public class Weapon : MonoBehaviour
    {
        [HideInInspector] public Transform tCached;

        // 效果参数
        public float bulletDmgMultiplier = 1f;
        public int ammoCount;
        public List<SpellElement> bulletElements;
        
        private Quaternion _pointingAngle;
        [HideInInspector] public Quaternion bulletInterQAngle;
        private float _bulletAngle;  // 相邻子弹间开角 
        private float _lastUpdatePointingAngleTime;
        
        public float BulletAngle
        {
            get => _bulletAngle;
            set
            {
                _bulletAngle = value;
                bulletInterQAngle = Quaternion.AngleAxis(_bulletAngle, Vector3.forward);
            }
        }

        private void Start()
        {
            BulletAngle = SpellData.Instance.bulletInterAngle;
            tCached = transform;
        }
        
        /// <summary>
        /// 添加子弹附着的元素属性
        /// </summary>
        /// <param name="element">元素类型</param>
        public void AddElement(SpellElement element) => bulletElements.Add(element);

        /// <summary>
        /// 获取指向的方向四元数
        /// </summary>
        public Quaternion GetPointingAngle()
        {
            if (Time.time < _lastUpdatePointingAngleTime) return _pointingAngle;
            _lastUpdatePointingAngleTime = Time.time + 0.5f * Time.deltaTime;  // 一帧只更新一次即可
            
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            var position = tCached.position;
            position.z = 0;
            var dir = mousePos - position;
            _pointingAngle = Quaternion.FromToRotation(Vector3.up, dir);
            
            return _pointingAngle;
        }

        
    }
}