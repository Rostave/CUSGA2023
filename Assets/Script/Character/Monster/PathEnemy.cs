using Chronos;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vacuname
{
    [RequireComponent(typeof(Seeker))]
    public class PathEnemy : BaseEnemy
    {
        private Seeker seeker;
        Path path;
        int curPointIndex = 0;
        bool reachLastPoint = false;
        public float closeDistance;
        public Transform target;
        protected override void Awake()
        {
            seeker = GetComponent<Seeker>();
            base.Awake();
            
        }
        private void Start()
        {
            InvokeRepeating("GetPath", 0, 0.5f);
        }
        private void GetPath()
        {
            Debug.Log(1);
            if(target!=null&&seeker.IsDone())
                seeker.StartPath(transform.position, target.position, OnPathComplete);
        }
        private void OnPathComplete(Path p)
        {
            if(!p.error)
            {
                curPointIndex = 0;
                reachLastPoint = false;
                path = p;
            }
        }
        private void FixedUpdate()
        {
            Fly(target);
        }

        public void Fly(Transform target)
        {
            if (target==null||path == null || reachLastPoint) return;

            if (curPointIndex >= path.vectorPath.Count)
            {
                reachLastPoint = true;
                return;
            }
            Vector2 dire = (target.position - transform.position).normalized;
            Vector2 force = dire * time.fixedDeltaTime * moveAttribute.maxSpeed;
            force *= 100;
            time.rigidbody2D.AddForce(force);
            if(Vector2.Distance(target.position, transform.position)<=closeDistance)
            {
                curPointIndex++;
            }

        }

    }
}

