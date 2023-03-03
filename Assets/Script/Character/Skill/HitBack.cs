using Chronos;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Vacuname
{
    public class HitBack : BasicSkill
    {
        [SerializeField,LabelText("��ȴʱ��")] private float maxBackCoolDown;
        [SerializeField, LabelText("��������ʱ��")] private float startTime;
        [SerializeField,LabelText("��Чʱ��")] private float initActTime;
        [SerializeField,LabelText("�ӵ�ʱ�䱶��")] private float bulletTimeScale;
        [SerializeField,LabelText("�ӵ�ʱ�䳤��")] private float bulletTime;
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
            if (curBackCoolDown <= 0 && remainActTime <= 0)
                StartCoroutine(Activiting());
        }
        public void Success()
        {
            StartCoroutine(Successing());
        }
        
        IEnumerator Successing()
        {
            remainActTime = 0;
            curBackCoolDown = maxBackCoolDown;
            Timekeeper.instance.Clock("Root").localTimeScale = bulletTimeScale;
            yield return new WaitForSecondsRealtime(bulletTime);
            Timekeeper.instance.Clock("Root").localTimeScale = 1f;
        }

        IEnumerator Activiting()
        {
            //curBackCoolDown = maxBackCoolDown;
            remainActTime = initActTime;
            range.enabled = true;

            while (remainActTime > 0)
            {
                remainActTime -= Time.deltaTime;
                yield return null;
            }
            Debug.Log("������ʧ");
            range.enabled = false;
        }

    }
}



