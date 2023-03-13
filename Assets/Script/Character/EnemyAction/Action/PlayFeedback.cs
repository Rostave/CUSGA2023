using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vacuname
{
    public class PlayFeedback : EnemyAction
    {
        [SerializeField] private string feedbackName;
        public override TaskStatus OnUpdate()
        {
            me.TryPlayFeedback(feedbackName);
            return TaskStatus.Success;
        }
    }
}

