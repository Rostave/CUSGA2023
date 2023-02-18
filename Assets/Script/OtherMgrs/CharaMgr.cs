using System;
using R0.SingaltonBase;
using R0.Weapons;

namespace R0.OtherMgrs
{
    public class CharaMgr : SingletonBehaviour<CharaMgr>
    {
        public Weapon playerWeapon;
        
        protected override void OnEnableInit()
        {
            
        }
    }
}