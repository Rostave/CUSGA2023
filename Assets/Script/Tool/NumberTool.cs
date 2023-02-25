using System.Collections;
using UnityEngine;

namespace Vacuname
{
    public static class NumberTool
    {
        public static Vector2 NullV2 = new Vector2(int.MinValue, int.MinValue);
        public static void Normalize(ref this float f)
        {
            f = f > 0 ? 1 : f < 0 ? -1 : 0;
        }
       
    }
}