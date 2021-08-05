using UnityEngine;

namespace Handy
{
    public class Tracer
    {
        /// <summary>
        /// �^�[�Q�b�g�֏d�Ȃ�悤�ɂ��ĒǐՂ���@�\
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
        /// �^�[�Q�b�g�Ƃ͈͎̔w��\�ǐՋ@�\
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