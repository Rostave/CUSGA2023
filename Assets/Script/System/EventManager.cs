using System;
using System.Collections.Generic;
using R0.SingaltonBase;

//参考学习于https://blog.csdn.net/qq_46044366/article/details/122722948

/// <summary>
/// 便于触发事件的扩展类
/// </summary>
public static class EventTriggerExtra
{
    /// <summary>
    /// 触发事件（无参数）
    /// </summary>
    /// <param name="sender">触发源</param>
    /// <param name="eventName">事件名</param>
    public static void TriggerEvent(this object sender, string eventName)
    {
        EventManager.Instance.TriggerEvent(eventName);
    }
    /// <summary>
    /// 触发事件（1个参数）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sender">触发源</param>
    /// <param name="eventName">事件名</param>
    /// <param name="info">参数</param>
    public static void TriggerEvent<T>(this object sender, string eventName, T info)
    {
        EventManager.Instance.TriggerEvent(eventName, info);
    }
    /// <summary>
    /// 触发事件（2个参数）
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <param name="sender">触发源</param>
    /// <param name="eventName">事件名</param>
    /// <param name="info1">参数1</param>
    /// <param name="info2">参数2</param>
    public static void TriggerEvent<T1, T2>(this object sender, string eventName, T1 info1, T2 info2)
    {
        EventManager.Instance.TriggerEvent(eventName, info1, info2);
    }
    /// <summary>
    /// 触发事件（3个参数）
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <param name="sender">触发源</param>
    /// <param name="eventName">事件名</param>
    /// <param name="info1">参数1</param>
    /// <param name="info2">参数2</param>
    /// <param name="info3">参数3</param>
    public static void TriggerEvent<T1, T2, T3>(this object sender, string eventName, T1 info1, T2 info2, T3 info3)
    {
        EventManager.Instance.TriggerEvent(eventName, info1, info2, info3);
    }
    /// <summary>
    /// 触发事件（4个参数）
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <param name="sender">触发源</param>
    /// <param name="eventName">事件名</param>
    /// <param name="info1">参数1</param>
    /// <param name="info2">参数2</param>
    /// <param name="info3">参数3</param>
    /// <param name="info4">参数4</param>
    public static void TriggerEvent<T1, T2, T3, T4>(this object sender, string eventName, T1 info1, T2 info2, T3 info3, T4 info4)
    {
        EventManager.Instance.TriggerEvent(eventName, info1, info2, info3, info4);
    }
}

/// <summary>
/// 事件管理中心
/// </summary>
public class EventManager : SingletonBase<EventManager>
{
    //key —— 事件的名字（比如：怪物死亡，玩家死亡，通关 等等）
    //value —— 对应的是 监听这个事件 对应的委托函数们
    private Dictionary<string, List<Delegate>> eventDic = new Dictionary<string, List<Delegate>>();

    /// <summary>
    /// 监听事件
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="callback"></param>
    public void AddListenerBase(string eventName, Delegate callback)
    {
        if (eventDic.ContainsKey(eventName))
        {
            eventDic[eventName].Add(callback);
        }
        else
        {
            eventDic.Add(eventName, new List<Delegate>() { callback});
        }
    }
    /// <summary>
    /// 监听不需要参数传递的事件
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="callback"></param>
    public void AddListener(string eventName, Action callback)
    {
        AddListenerBase(eventName, callback);
    }
    /// <summary>
    /// 添加事件监听（1个参数）
    /// </summary>
    /// <param name="name">事件的名字</param>
    /// <param name="action">准备用来处理事件 的委托函数</param>
    public void AddListener<T>(string eventName, Action<T> callback)
    {
        AddListenerBase(eventName, callback);
    }
    /// <summary>
    /// 添加事件监听（2个参数）
    /// </summary>
    /// <param name="eventName">事件的名字</param>
    /// <param name="callback">准备用来处理事件 的委托函数</param>
    public void AddListener<T1, T2>(string eventName, Action<T1, T2> callback)
    {
        AddListenerBase(eventName, callback);
    }
    /// <summary>
    /// 添加事件监听（3个参数）
    /// </summary>
    /// <param name="eventName">事件的名字</param>
    /// <param name="callback">准备用来处理事件 的委托函数</param>
    public void AddListener<T1, T2, T3>(string eventName, Action<T1, T2, T3> callback)
    {
        AddListenerBase(eventName, callback);
    }
    /// <summary>
    /// 添加事件监听（4个参数）
    /// </summary>
    /// <param name="eventName">事件的名字</param>
    /// <param name="callback">准备用来处理事件 的委托函数</param>
    public void AddListener<T1, T2, T3, T4>(string eventName, Action<T1, T2, T3, T4> callback)
    {
        AddListenerBase(eventName, callback);
    }
    /// <summary>
    /// 移除事件
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="callback"></param>
    public void RemoveListenerBase(string eventName, Delegate callback)
    {
        if (eventDic.TryGetValue(eventName, out List<Delegate> eventList))
        {
            eventList.Remove(callback);
            //事件列表没有事件时，将该事件键值对从字典中移除
            if (eventList.Count == 0)
            {
                eventDic.Remove(eventName);
            }
        }
    }

    /// <summary>
    /// 移除不需要参数的事件
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="callback"></param>
    public void RemoveListener(string eventName, Action callback)
    {
        RemoveListenerBase(eventName, callback);
    }
    /// <summary>
    /// 移除对应的事件监听（1个参数）
    /// </summary>
    /// <param name="eventName">事件的名字</param>
    /// <param name="callback">对应之前添加的委托函数</param>
    public void RemoveListener<T>(string eventName, Action<T> callback)
    {
        RemoveListenerBase(eventName, callback);
    }
    /// <summary>
    /// 移除对应的事件监听（2个参数）
    /// </summary>
    /// <param name="eventName">事件的名字</param>
    /// <param name="callback">对应之前添加的委托函数</param>
    public void RemoveListener<T1, T2>(string eventName, Action<T1, T2> callback)
    {
        RemoveListenerBase(eventName, callback);
    }
    /// <summary>
    /// 移除对应的事件监听（3个参数）
    /// </summary>
    /// <param name="eventName">事件的名字</param>
    /// <param name="callback">对应之前添加的委托函数</param>
    public void RemoveListener<T1, T2, T3>(string eventName, Action<T1, T2, T3> callback)
    {
        RemoveListenerBase(eventName, callback);
    }
    /// <summary>
    /// 移除对应的事件监听（4个参数）
    /// </summary>
    /// <param name="eventName">事件的名字</param>
    /// <param name="callback">对应之前添加的委托函数</param>
    public void RemoveListener<T1, T2, T3, T4>(string eventName, Action<T1, T2, T3, T4> callback)
    {
        RemoveListenerBase(eventName, callback);
    }
    /// <summary>
    /// 事件触发（不需要参数的）
    /// </summary>
    /// <param name="eventName"></param>
    public void TriggerEvent(string eventName)
    {
        if (eventDic.ContainsKey(eventName))
        {
            foreach (Delegate callback in eventDic[eventName])
            {
                (callback as Action)?.Invoke();
            }
        }
    }
    /// <summary>
    /// 事件触发（1个参数）
    /// </summary>
    /// <param name="eventName">哪一个名字的事件触发了</param>
    public void TriggerEvent<T>(string eventName, T info)
    {
        if (eventDic.ContainsKey(eventName))
        {            
            foreach (Delegate callback in eventDic[eventName])
            {
                (callback as Action<T>)?.Invoke(info); 
            }
        }
    }
    /// <summary>
    /// 事件触发（2个参数）
    /// </summary>
    /// <param name="eventName">哪一个名字的事件触发了</param>
    public void TriggerEvent<T1, T2>(string eventName, T1 info1, T2 info2)
    {
        if (eventDic.ContainsKey(eventName))
        {
            foreach (Delegate callback in eventDic[eventName])
            {
                (callback as Action<T1, T2>)?.Invoke(info1, info2);
            }
        }
    }
    /// <summary>
    /// 事件触发（3个参数）
    /// </summary>
    /// <param name="eventName">哪一个名字的事件触发了</param>
    public void TriggerEvent<T1, T2, T3>(string eventName, T1 info1, T2 info2, T3 info3)
    {
        if (eventDic.ContainsKey(eventName))
        {
            foreach (Delegate callback in eventDic[eventName])
            {
                (callback as Action<T1, T2, T3>)?.Invoke(info1, info2, info3);
            }
        }
    }
    /// <summary>
    /// 事件触发（4个参数）
    /// </summary>
    /// <param name="eventName">哪一个名字的事件触发了</param>
    public void TriggerEvent<T1, T2, T3, T4>(string eventName, T1 info1, T2 info2, T3 info3, T4 info4)
    {
        if (eventDic.ContainsKey(eventName))
        {
            foreach (Delegate callback in eventDic[eventName])
            {
                (callback as Action<T1, T2, T3, T4>)?.Invoke(info1, info2, info3, info4);
            }
        }
    }
    /// <summary>
    /// 清空事件中心
    /// </summary>
    public void Clear()
    {
        eventDic.Clear();
    }
}
