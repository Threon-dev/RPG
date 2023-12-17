using System;
using System.Security.Cryptography;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed;
        private Health _target;
        private float _damage = 0;
        void Update()
        {
            transform.LookAt(GetAimLocation());
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            float hitDistance = 0.1f;
        }

        public void SetTarget(Health target,float damage)
        {
            _target = target;
            _damage = damage;
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
                _target.TakeDamage(_damage);
                Destroy(gameObject);
            }
        }
    }
}
