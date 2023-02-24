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
            me.GetEventDic().Add(skillName, new UnityAction(Effect));
            range = GetComponent<PolygonCollider2D>();
            range.enabled = false;
        }

        public virtual void Effect()
        {
        }
    }
}

