using UnityEngine;

namespace R0.Static
{
    /// <summary>
    /// 字符串哈希值
    /// </summary>
    public static class StrHash
    {
        /// <summary>
        /// 动画器(Animator)字符串哈希
        /// </summary>
        public static class Ani
        {
            public static readonly int Idle = Animator.StringToHash("Idle");
        }
        
        /// <summary>
        /// Shader属性(Uniform)字符串哈希
        /// </summary>
        public static class Uni
        {
            public static readonly int Color = Animator.StringToHash("_Color");
        }
        
    }
}