using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R0.SingaltonBase;

public class TimeControl :SingletonBehaviour<TimeControl>
{
    private float defaultTimeScale;
    protected override void OnEnableInit()
    {
        defaultTimeScale = Time.deltaTime;
    }
    /// <summary>
    /// 设置时间流逝速度
    /// </summary>
    /// <param name="scale">哈哈</param>
    public void SetTimeScale(float scale,float timeToUpdate)
    {
        StopAllCoroutines();
        StartCoroutine(TimeScaleUpdating(scale,timeToUpdate));
    }

    IEnumerator TimeScaleUpdating(float scale,float timeToUpdate)
    {
        float timer = 0;
        float fromScale = Time.timeScale;

        while(timer<timeToUpdate)
        {
            float newScale = Mathf.Lerp(fromScale, scale, timer / timeToUpdate);
            Time.timeScale = newScale;
            Time.fixedDeltaTime = defaultTimeScale * newScale;
            timer += Time.unscaledDeltaTime;
            yield return null;
        }
        
    }


}
