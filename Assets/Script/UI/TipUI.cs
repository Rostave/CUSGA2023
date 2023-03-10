using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R0.SingaltonBase;
using TMPro;

namespace Vacuname
{
    public class TipUI : SingletonBehaviour<TipUI>
    {
        [SerializeField] private TextMeshProUGUI tipText;
        protected override void OnEnableInit()
        {
            
        }

        public void UpdateTipText(Vector3 pos,bool active)
        {
            tipText.gameObject.SetActive(active);
            tipText.transform.position = pos;

        }

    }

}

