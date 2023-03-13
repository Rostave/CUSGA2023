using BehaviorDesigner.Runtime;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vacuname
{
    [RequireComponent(typeof(BehaviorTree))]
    public class BaseEnemy : Character
    {
        public Visual visual;
        protected BehaviorTree bt;
        protected override void Awake()
        {
            bt = GetComponent<BehaviorTree>();
            base.Awake();
        }
        private void Start()
        {
            sm_anima.AnimationState.SetAnimation(0, "idle", true);
        }
    }


}
