﻿// using R0.EditorTool;
// using UnityEditor;
// using UnityEngine;
//
// /// <summary>
// /// 定义对带有 `CustomLabelAttribute` 特性的字段的面板内容的绘制行为
// /// </summary>
// [CustomPropertyDrawer(typeof(CustomLabelAttribute))]
// public class CustomLabelDrawer: PropertyDrawer
// {
//     private GUIContent _label;
//     public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//     {
//         if (_label == null)
//         {
//             var name = (attribute as CustomLabelAttribute)?.name;
//             _label = new GUIContent(name);
//         }
//         EditorGUI.PropertyField(position, property, _label);
//     }
// }