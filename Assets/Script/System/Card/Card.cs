using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R0;
using static R0.ScriptableObjConfig.SpellData;
using TMPro;
using UnityEngine.UI;
using R0.SpellRel;
using UnityEngine.EventSystems;
using MoreMountains.Feedbacks;

namespace Vacuname
{
    public class Card : MonoBehaviour
    {
        [HideInInspector] public SpellDataStruct spellData;
        [SerializeField] private TextMeshProUGUI title, intro, type;
        [SerializeField] private Image illstration;
        private Dictionary<SpellEffect, string> effectNameDic = new Dictionary<SpellEffect, string>()
        {{SpellEffect.BulletSummon,"攻击" },{SpellEffect.ElementAttach,"元素" },{SpellEffect.PropMod,"属性" },{SpellEffect.BulletCount,"数量" } };
        public void Init(SpellDataStruct data)
        {
            spellData = data;
            title.text = spellData.name;
            intro.text = spellData.description;
            type.text = effectNameDic[spellData.effect];
            illstration.sprite = spellData.spellSprite;
        }

        public void OnCardClick()
        {
            DrawCard.Instance.spreadFeedback?.PlayFeedbacks();
        }

    }
}

