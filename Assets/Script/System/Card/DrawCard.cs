using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static R0.ScriptableObjConfig.SpellData;
using R0.SingaltonBase;
using R0.ScriptableObjConfig;
using System;

namespace Vacuname
{
    public class DrawCard : SingletonBehaviour<DrawCard>
    {
        public List<SpellDataStruct> basicSpellPool;

        public Card textCard;
        protected override void OnEnableInit()
        {
            basicSpellPool = SpellData.Instance.data;//暂时是抽所有的卡
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                List<SpellDataStruct>test= DoDrawCard(basicSpellPool, 0, basicSpellPool.Count, 3);
                textCard.Init(test[0]);
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
    }

}
