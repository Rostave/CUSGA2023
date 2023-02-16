﻿using UnityEngine;


namespace R0.EditorTool
{
    /// <summary>
    /// 字段在Inspector中显示自定义名称
    /// </summary>
    public class CustomLabelAttribute : PropertyAttribute
    {
        public readonly string name;

        /// <summary>
        /// 字段在Inspector中显示自定义名称
        /// </summary>
        /// <param name="customName">自定义名称</param>
        public CustomLabelAttribute(string customName) => name = customName;
    }
}