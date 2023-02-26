using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackDictionary : SerializedMonoBehaviour
{
    [DictionaryDrawerSettings, SerializeField]
    private Dictionary<string, MMF_Player> feedbackDic = new Dictionary<string, MMF_Player>();
    public void TryPlay(string n)
    {
        if (feedbackDic.ContainsKey(n))
            feedbackDic[n].PlayFeedbacks();
    }
}
