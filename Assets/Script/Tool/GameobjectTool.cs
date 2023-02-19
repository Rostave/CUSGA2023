using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameobjectTool
{
    /// <summary>
    /// 找到子物体的零件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <param name="name">子物体名</param>
    /// <param name="res">零件类型</param>
    /// <returns></returns>
    public static bool TryGetComponentByChildName<T>(this Transform t, string name,out T res)where T:Component
    {
        res=null;
        Transform child = t.Find(name);
        if (child == null)
            return false;
        else
            return child.TryGetComponent<T>(out res);
    }
}
