using System.Collections;
using UnityEngine;

namespace Vacuname
{
    public static class SpriteTool
    {
        public static float GetHeight(this Sprite sprite)
        {
            Bounds bounds = sprite.bounds;
            float spriteHeight = bounds.size.y;
            return spriteHeight;
        }

        public static Vector3 SetScaleDirection(Vector3 scale,float x)
        {
            scale.x = Mathf.Abs(scale.x) * (x < 0 ? -1 : 1);
            return scale;
        }

    }
}