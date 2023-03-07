using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.Events;

namespace Vacuname
{
    public class BasicSkill : MonoBehaviour
    {
        protected PolygonCollider2D range;
        
        protected Character me;
        [SerializeField]protected string skillName;

        protected virtual void Awake()
        {
            me= transform.GetComponentInParent<Character>();
            me.GetSkillDic().Add(skillName, new UnityAction(Effect));
            if(TryGetComponent(out range))
                range.enabled = false;
        }

        public virtual void Effect()
        {
        }
    }
}

