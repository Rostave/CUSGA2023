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

    }
}