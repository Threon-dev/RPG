using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private Transform trail;
        [SerializeField] private bool isHoming;
        [SerializeField] private GameObject hitEffect = null;
        private Health _target;
        private float _damage = 0;
        void Update()
        {
            if (_target == null) return;
            if (isHoming && !_target.IsDead())
            {
                transform.LookAt(GetAimLocation());
            }
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            float hitDistance = 0.1f;
        }

        public void SetTarget(Health target,float damage)
        {
            _target = target;
            _damage = damage;
            transform.LookAt(GetAimLocation());
        }
        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = _target.transform.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
            {
                return _target.transform.position;
            }
            return _target.transform.position + Vector3.up * targetCapsule.height / 1.5f;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == _target.gameObject)
            {
                if (_target.IsDead()) return;
                _target.TakeDamage(_damage);
                trail.SetParent(null);
                if (hitEffect != null)
                {
                    Instantiate(hitEffect, GetAimLocation(),transform.rotation);
                }
                Destroy(gameObject);
            }
        }
    }
}
