using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vacuname
{
    public class BasicSkill : MonoBehaviour
    {
        protected PolygonCollider2D atkRange;
        private void Awake()
        {
            atkRange = GetComponent<PolygonCollider2D>();
            atkRange.enabled = false;
        }
        public bool TryMakeDamage()
        {
            atkRange.enabled = true;
            var contactFilter2D = new ContactFilter2D();
            contactFilter2D.useTriggers = true;
            int layer = ~gameObject.layer;
            contactFilter2D.SetLayerMask(layer);
            List<Collider2D> hits = new List<Collider2D>();
            atkRange.OverlapCollider(contactFilter2D, hits);
            Player p=null;
            bool blocked = false;
            foreach (var a in hits)
            {
                if (a.CompareTag("Player"))
                {
                    p = a.GetComponent<Player>();
                }
                else if(a.CompareTag("Sheild"))
                {
                    blocked = true;
                    Debug.LogError("Blocked");
                    break;
                }
            }
            atkRange.enabled = false;

            if (blocked)
            {
                return false;
            }
            else
            {
                if (p != null)
                    Debug.Log("Hit");
                return true;
            }

            
        }
    }
}

