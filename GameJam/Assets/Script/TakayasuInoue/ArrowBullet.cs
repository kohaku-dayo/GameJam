using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Triggers;
using UniRx;

namespace Goriyasu
{
    public class ArrowBullet : MonoBehaviour,IBullet
    {
        [SerializeField] float m_attackParameter;
        [SerializeField] float m_speed;
        [SerializeField] GameObject m_effect;
        GameObject m_owner;
        GameObject m_target;
        Rigidbody m_rigid;
        public void Initialize(GameObject owner, GameObject target)
        {
            m_owner = owner;
            m_target = target;
  
            GetComponent<Rigidbody>().velocity =
                (m_target.transform.position - this.transform.position).normalized * m_speed;
        }

        public void SetAttackParameter(float attack)
        {
            m_attackParameter = attack;
        }

        public void SetSpeed(float speed)
        {
            m_speed = speed;
        }

        private void Start()
        {
            this.OnTriggerEnterAsObservable()
                .Where(other => other.gameObject.tag == "Enemy")
                .Subscribe(other => { other.gameObject.GetComponent<IDamage>().Damage(m_attackParameter);Destroy(this.gameObject); });

            this.OnTriggerEnterAsObservable()
                .Where(other => other.gameObject.tag == "DestroyWall")
                .Subscribe(other =>  Destroy(this.gameObject));
        }
    }
}
