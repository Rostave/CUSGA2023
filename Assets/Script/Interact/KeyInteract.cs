using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Vacuname
{
    public class KeyInteract : Interact
    {
        protected bool canInteract=false;
        [SerializeField,LabelText("提示按键文字的位置")]private Transform tipPositon;

        private void Update()
        {
            if(canInteract)
            {
                if(Input.GetKeyDown(KeyCode.E))
                {
                    TipUI.Instance.UpdateTipText(tipPositon.position, false);
                    Triggered();
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D c)
        {
            if(c.CompareTag("Player")&&useableTime>0)
            {
                canInteract = true;
                TipUI.Instance.UpdateTipText(tipPositon.position,true);
            }
        }

        private void OnTriggerExit2D(Collider2D c)
        {
            if (c.CompareTag("Player"))
            {
                canInteract = false;
                TipUI.Instance.UpdateTipText(tipPositon.position, false);
            }

        }
    }
}

