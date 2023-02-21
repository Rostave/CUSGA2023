using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : Action
{
    public SharedFloat attackDistance;
    public SharedInt direction;
    public SharedTransform target;

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Success;
       // float distance = Vector2.Distance(transform.position, );
    }
}
