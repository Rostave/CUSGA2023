using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Vacuname
{
    public class HitBack : MonoBehaviour
    {
        public float maxBackCoolDown;
        public float curBackCoolDown;
        public float remainActTime, initActTime;
        private PolygonCollider2D col;
        private void Awake()
        {
            col = GetComponent<PolygonCollider2D>();
            col.enabled = false;
            curBackCoolDown = 0;
        }

        private void Update()
        {
            if (curBackCoolDown > 0 && remainActTime <= 0)
                curBackCoolDown -= Time.deltaTime;
        }

        public void TryActive()
        {
            if (curBackCoolDown <= 0)
                StartCoroutine(Activiting());
        }


        IEnumerator Activiting()
        {
            curBackCoolDown = maxBackCoolDown;
            remainActTime = initActTime;
            col.enabled = true;

            while (remainActTime > 0)
            {
                remainActTime -= Time.deltaTime;
                yield return null;
            }
            col.enabled = false;
        }

    }
}



