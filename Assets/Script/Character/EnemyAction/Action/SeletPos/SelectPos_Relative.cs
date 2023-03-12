using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vacuname
{
    public class SelectPos_Relative : SelectPos
    {
        [SerializeField]protected Vector2 rangeOffset;
        [SerializeField] protected SharedGameObject target;
        protected override Vector2 GetPos()
        {
            Vector2 res = target.Value.transform.position;
            res += rangeOffset;
            Vector2 random = base.GetPos();
            random -= (Vector2)transform.position;
            res += random;
            return res;
        }
    }
}

