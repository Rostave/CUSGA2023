using UnityEngine;

namespace R0.Bullet
{
    /// <summary>
    /// 子弹基类，封装矢量移动逻辑
    /// </summary>
    public class Bullet : MonoBehaviour
    {
        /// <summary> transform缓存 </summary>
        private Transform _tCached, _imgTCached;
        
        /// <summary> 速度乘量 </summary>
        private float _speedMultiplier;
        private float _moveSpeed;
        private Vector3 _moveDir;
        private float _lifeEndTime;
        
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
        
        /// <summary>
        /// 设置子弹移动方向
        /// </summary>
        /// <param name="moveDir">三元向量的移动方向</param>
        public void SetMoveDir(ref Vector3 moveDir)
        {
            _moveDir = moveDir;
            if (IsBulletFacingDir) _imgTCached.rotation = Quaternion.FromToRotation(Vector3.up, moveDir);
        }

        private void Move()
        {
            _tCached.Translate(_moveDir * Time.deltaTime * _speedMultiplier * _moveSpeed);
        }

        /// <summary>
        /// 重置基本参数
        /// </summary>
        protected virtual void ResetParam()
        {
            _lifeEndTime = Time.time;
            
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