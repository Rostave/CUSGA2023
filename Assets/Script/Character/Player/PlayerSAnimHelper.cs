using Spine.Unity;
using UnityEngine;

namespace R0
{
    public static class PlayerSAnimHelper
    {
        public static void PlayAnim(SkeletonAnimation sa, string animName, bool loop = true, int trackIndex = 0)
        {
            sa.AnimationState.SetAnimation(trackIndex, animName, loop);
        }
        
        public static void SwitchSkeletonData(SkeletonAnimation sa, SkeletonDataAsset sda)
        {
            sa.ClearState();
            sa.skeletonDataAsset = sda;
            sa.Initialize(true);
        }
    }
}