using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vacuname
{
    public class Chest:KeyInteract
    {
        protected override void Awake()
        {
            base.Awake();
            triger = true;
        }
        protected override void ActiveEffect()
        {
            base.ActiveEffect();
            DrawCard.Instance.DrawCardToSelect();
        }
    }
}

