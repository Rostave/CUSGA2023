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
    /// ����ʱ�������ٶ�
    /// </summary>
    /// <param name="scale">����</param>
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
