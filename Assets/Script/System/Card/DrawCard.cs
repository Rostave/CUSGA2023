using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static R0.ScriptableObjConfig.SpellData;
using R0.SingaltonBase;
using R0.ScriptableObjConfig;
using System;
using UnityEngine.UI;
using MoreMountains.Feedbacks;
using Chronos;
using static MoreMountains.Feedbacks.MMFeedbacks;

namespace Vacuname
{
    public class DrawCard : SingletonBehaviour<DrawCard>
    {
        public List<SpellDataStruct> basicSpellPool;

        public List<Card> cards;

        public MMF_Player spreadFeedback;

        protected override void OnEnableInit()
        {
            basicSpellPool = SpellData.Instance.data;//暂时是抽所有的卡
        }

        public void EnableCards(bool can)
        {
            foreach(var a in cards)
            {
                a.GetComponent<Button>().interactable = can;
            }
        }

        private List<SpellDataStruct> DoDrawCard(List<SpellDataStruct> list,int minInclusive,int maxInclusive,int time)
        {
            List<int> drawInt = new List<int>();
            List<SpellDataStruct> drawCard = new List<SpellDataStruct>();
            for(int i=0;i<time;i++)
            {
                int newOne;
                do
                {
                    newOne = UnityEngine.Random.Range(minInclusive, maxInclusive);
                }
                while (drawInt.Contains(newOne));
                drawInt.Add(newOne);
            }
            foreach (int a in drawInt)
                drawCard.Add(list[a]);
            return drawCard;
        }

        public void DrawCardToSelect()
        {
            Timekeeper.instance.Clock("Root").localTimeScale = 0;
            int num = 3;
            List<SpellDataStruct> datas = DoDrawCard(basicSpellPool, 0, basicSpellPool.Count, num);
            for (int i = 0; i < num; i++)
                cards[i].Init(datas[i]);
            spreadFeedback.PlayFeedbacks();
        }
    }

}
