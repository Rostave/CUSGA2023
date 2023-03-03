using UnityEngine;

namespace R0.Static
{
    /// <summary>
    /// 字符串哈希值
    /// </summary>
    public static class Const
    {
        /// <summary>
        /// 动画器(SkeletonAnimation)字符串哈希
        /// </summary>
        public static class Ani
        {
            public static readonly int Idle = Animator.StringToHash("idle");
            public static readonly int Rest = Animator.StringToHash("rest");
            public static readonly int Move = Animator.StringToHash("move");
            public static readonly int Die = Animator.StringToHash("die");
            public static readonly int Hurt = Animator.StringToHash("hurt");
            public static readonly int Jump = Animator.StringToHash("jump");
        }
        
        /// <summary>
        /// Shader属性(Uniform)字符串哈希
        /// </summary>
        public static class Uni
        {
            public static readonly int Color = Animator.StringToHash("_Color");
        }
        
        /// <summary>
        /// 四元数常量
        /// </summary>
        public static class Qua
        {
            // 例: Clk45_5 : 顺时针旋转45.5角度
            // Clk - clockwise
            // AClk - anti-clockwise
            // 45_5 - 转45.5角度
            
            // 常用
            public static readonly Quaternion Zero = Quaternion.AngleAxis(0, Vector3.forward);
            // public static readonly Quaternion Clk5 = Quaternion.AngleAxis(-5f, Vector3.forward);
            // public static readonly Quaternion AClk5 = Quaternion.AngleAxis(5f, Vector3.forward);
            // public static readonly Quaternion Clk10 = Quaternion.AngleAxis(-10f, Vector3.forward);
            // public static readonly Quaternion AClk10 = Quaternion.AngleAxis(10f, Vector3.forward);
            // public static readonly Quaternion Clk15 = Quaternion.AngleAxis(-15f, Vector3.forward);
            // public static readonly Quaternion AClk15 = Quaternion.AngleAxis(15f, Vector3.forward);
            // public static readonly Quaternion Clk20 = Quaternion.AngleAxis(-20f, Vector3.forward);
            // public static readonly Quaternion AClk20 = Quaternion.AngleAxis(20f, Vector3.forward);
            // public static readonly Quaternion Clk30 = Quaternion.AngleAxis(-30f, Vector3.forward);
            // public static readonly Quaternion AClk30 = Quaternion.AngleAxis(30f, Vector3.forward);
            // public static readonly Quaternion Clk45 = Quaternion.AngleAxis(-45f, Vector3.forward);
            // public static readonly Quaternion AClk45 = Quaternion.AngleAxis(45f, Vector3.forward);
            // public static readonly Quaternion Clk60 = Quaternion.AngleAxis(-60f, Vector3.forward);
            // public static readonly Quaternion AClk60 = Quaternion.AngleAxis(60f, Vector3.forward);
            // public static readonly Quaternion Clk90 = Quaternion.AngleAxis(-90f, Vector3.forward);
            // public static readonly Quaternion AClk90 = Quaternion.AngleAxis(90f, Vector3.forward);
            // public static readonly Quaternion Clk120 = Quaternion.AngleAxis(-120f, Vector3.forward);
            // public static readonly Quaternion AClk120 = Quaternion.AngleAxis(120f, Vector3.forward);
            // public static readonly Quaternion Clk135 = Quaternion.AngleAxis(-135f, Vector3.forward);
            // public static readonly Quaternion AClk135 = Quaternion.AngleAxis(135f, Vector3.forward);
            // public static readonly Quaternion Clk180 = Quaternion.AngleAxis(-180f, Vector3.forward);

            // 特殊
            // public static readonly Quaternion Clk22_5 = Quaternion.AngleAxis(-22.5f, Vector3.forward);
            // public static readonly Quaternion Clk100 = Quaternion.AngleAxis(-100f, Vector3.forward);
            // public static readonly Quaternion AClk100 = Quaternion.AngleAxis(100f, Vector3.forward);
            // public static readonly Quaternion Clk160 = Quaternion.AngleAxis(-160f, Vector3.forward);
        }
        
    }
}