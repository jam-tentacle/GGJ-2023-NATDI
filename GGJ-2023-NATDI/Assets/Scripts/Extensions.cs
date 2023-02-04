using UnityEngine;

namespace NATDI
{
    public static class Extensions
    {
        public static void LookAt(this Transform transform, ITarget target)
        {
            transform.LookAt(target.Position);
        }
    }
}
