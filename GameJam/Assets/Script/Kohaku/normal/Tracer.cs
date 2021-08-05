using UnityEngine;

namespace Handy
{
    public class Tracer
    {
        /// <summary>
        /// ターゲットへ重なるようにして追跡する機能
        /// </summary>
        public static bool trace(GameObject target, GameObject self, Rigidbody selfrb, float traceSpeed = 1f)
        {
            var direction = (target.transform.position - self.transform.position).normalized;
            if (Vector3.Distance(target.transform.position, self.transform.position) < 0.1f)
            {
                selfrb.velocity = Vector3.zero;
                return false;
            }
            else selfrb.velocity = direction * traceSpeed;
            return true;
        }
        /// <summary>
        /// ターゲットとの範囲指定可能追跡機能
        /// </summary>
        public static bool trace(GameObject target, GameObject self, Rigidbody selfrb, float DontMoveAreaRange, float traceSpeed = 1f)
        {
            var direction = (target.transform.position - self.transform.position).normalized;
            if (Vector3.Distance(target.transform.position, self.transform.position) < DontMoveAreaRange)
            {
                selfrb.velocity = Vector3.zero;
                return false;
            }
            else selfrb.velocity = direction * traceSpeed;
            return true;
        }
    }
}