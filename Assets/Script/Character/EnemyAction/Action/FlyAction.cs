using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Pathfinding;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vacuname
{
    
    public class FlyAction : Move
    {
        private Seeker seeker;
        private Path path;
        private int curPointIndex = 0;
        private bool reachLastPoint = false;
        public float closeDistance;
        private Vector2 targetPos;
        [SerializeField,LabelText("以x%的速度飞行")]private float speedScale=1;
        public override void OnAwake()
        {
            seeker = GetComponent<Seeker>();
            base.OnAwake();
        }
        public override void OnStart()
        {
            curPointIndex = 0;
            reachLastPoint = false;
            if (target.Value != null) targetPos = target.Value.transform.position;
            else
            {
                if (patrolPos.Value != NumberTool.NullV2) targetPos = patrolPos.Value;
                else targetPos = transform.position;
            }
            StartCoroutine(GetPathLoop());
        }

        public override void OnFixedUpdate()
        {
            Fly();
        }
        public override TaskStatus OnUpdate()
        {
            if (target.Value != null && patrolPos.Value != NumberTool.NullV2)
            {
                patrolPos.Value = NumberTool.NullV2;
                return TaskStatus.Failure;
            }

            //index走完，代表到达 
            if (curPointIndex >= path.vectorPath.Count)
            {
                reachLastPoint = true;
                StopAllCoroutines();
                return TaskStatus.Success;
            }
            else
            {
                return TaskStatus.Running;
            }

            
        }

        IEnumerator GetPathLoop()
        {
            GetPath();
            yield return new WaitForSecondsRealtime(0.5f);
        }
        private void GetPath()
        {
            if (seeker.IsDone())
            {
                seeker.StartPath(transform.position, targetPos, OnPathComplete);
            }
                
        }
        private void OnPathComplete(Path p)
        {
            if (!p.error)
            {
                curPointIndex = 0;
                reachLastPoint = false;
                path = p;
            }
        }
        public void Fly()
        {
            if ( path == null || reachLastPoint) return;

            Vector2 dire = (path.vectorPath[curPointIndex] - transform.position).normalized;
            Vector2 force = dire * me.time.fixedDeltaTime * me.moveAttribute.maxSpeed;
            force *= 100* speedScale;
            me.time.rigidbody2D.AddForce(force);

            me.transform.localScale = SpriteTool.GetScaleDirection(me.transform.localScale,dire.x*me.defaultScale);

            if (Vector2.Distance(path.vectorPath[curPointIndex], transform.position) <= closeDistance)
            {
                curPointIndex++;
            }
        }

    }
}