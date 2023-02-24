using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Vacuname
{
    public class EnemyConditional : Conditional
    {
        protected Rigidbody2D rd;
        protected Animator anima;
        protected Character me;
        
        public override void OnAwake()
        {
            me = GetComponent<Character>();
            anima = me.anima;
            rd = me.rd;

        }
    }
}