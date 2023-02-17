// //  Project : UNITY FOLDOUT
// // Contacts : Pix - ask@pixeye.games
//
//
// namespace R0.EditorTool
// {
// 	using UnityEngine;
//
// 	/// <summary>
// 	/// 字段在Inspector里可以被合进folder或展开
// 	/// </summary>
// 	public class FoldoutAttribute : PropertyAttribute
// 	{
// 		public string Name;
// 		public bool FoldEverything;
//
// 		/// <summary>
// 		/// 字段在Inspector里可以被合进folder或展开
// 		/// </summary>
// 		/// <param name="groupName">字段组名称</param>
// 		/// <param name="isFoldEverything">是否把被修饰字段后所有字段合进组里</param>
// 		public FoldoutAttribute(string groupName, bool isFoldEverything = false)
// 		{
// 			FoldEverything = isFoldEverything;
// 			Name = groupName;
// 		}
// 	}
// }