using UnityEngine;

namespace R0.ScriptableObjConfig
{
    /// <summary>
    /// scriptableObject单例模式抽象基类
    /// </summary>
    /// <typeparam name="T">实际ScriptableObject类型</typeparam>
    public abstract class SingaltonScriptableObj<T> : ScriptableObject where T : ScriptableObject
    {
        private static readonly string ScriptableObjPath = $"Data/{typeof(T).Name}";
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = Resources.Load<T>(ScriptableObjPath);
                if (Instance == null) Instance = CreateInstance<T>();
            }
        }
        
    }
}