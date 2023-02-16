using UnityEngine;

namespace R0.SingaltonBase
{
    /// <summary>
    /// 单例模式MonoBehaviour抽象基类
    /// </summary>
    /// <typeparam name="T">实际MonoBehaviour类型</typeparam>
    public abstract class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }

        /// <summary>
        /// 初始化。在OnEnable内调用
        /// </summary>
        protected abstract void OnEnableInit();

        protected virtual void OnEnable()
        {
            if (Instance == null)
            {
                Instance = (T) (object) this;
                OnEnableInit();
            }
        }
    }
}