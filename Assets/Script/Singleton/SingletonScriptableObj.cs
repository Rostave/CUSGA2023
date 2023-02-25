using UnityEngine;

namespace R0.ScriptableObjConfig
{
    /// <summary>
    /// scriptableObject单例模式抽象基类
    /// </summary>
    /// <typeparam name="T">实际ScriptableObject类型</typeparam>
    public abstract class SingletonScriptableObj<T> : ScriptableObject where T : ScriptableObject
    {
        private static readonly string ScriptableObjPath = $"Data/{typeof(T).Name}";

        private static T _instance { get; set; }
       
        public static T Instance 
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<T>(ScriptableObjPath);
                    if (_instance == null) _instance = CreateInstance<T>();
                }
                return _instance;
            }
            set => _instance = value;
        }
    }
}