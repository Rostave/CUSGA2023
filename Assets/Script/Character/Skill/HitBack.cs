using Chronos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Vacuname
{
    public class HitBack : BasicSkill
    {
        [SerializeField]private float maxBackCoolDown;
        [SerializeField] private float initActTime;
        private float curBackCoolDown;
        private float remainActTime;
        
        protected override void Awake()
        {
            base.Awake();
            curBackCoolDown = 0;
        }

        private void Update()
        {
            if (curBackCoolDown > 0 && remainActTime <= 0)
                curBackCoolDown -= Time.deltaTime;
        }

        public override void Effect()
        {
            if (curBackCoolDown <= 0)
                StartCoroutine(Activiting());
        }
        public void Success()
{
            
        }

        IEnumerator Activiting()
        {
            curBackCoolDown = maxBackCoolDown;
            remainActTime = initActTime;
            range.enabled = true;

            while (remainActTime > 0)
            {
                remainActTime -= Time.deltaTime;
                yield return null;
            }
            range.enabled = false;
        }

    }
}



