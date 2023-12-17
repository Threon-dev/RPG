using System;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] private GameObject equippedPrefab = null;
        [SerializeField] private AnimatorOverrideController animatorOverride = null;
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float weaponDamage = 5f;
        [SerializeField] private bool isRightHand = true;
        [SerializeField] private Projectile projectile = null;

        public float WeaponRange => weaponRange;
        public float WeaponDamage => weaponDamage;

        public bool HasProjectile => projectile != null;

        public void LaunchProjectile(Transform rightHand, Transform leftHand,Health target,float damage)
        {
            var handTransform = GetHandTransform(rightHand, leftHand);
            Projectile projectileInstance = Instantiate(projectile, handTransform.position,Quaternion.identity);
            projectileInstance.SetTarget(target,damage);
        }
        
        public void Spawn(Transform rightHandTransform,Transform leftHandTransform, Animator animator)
        {
            if (equippedPrefab != null)
            {
                var handTransform = GetHandTransform(rightHandTransform, leftHandTransform);
                Instantiate(equippedPrefab, handTransform);
            }
            if (animatorOverride != null)
            { 
                animator.runtimeAnimatorController = animatorOverride;
            }
        }

        private Transform GetHandTransform(Transform rightHandTransform, Transform leftHandTransform)
        {
            Transform handTransform;
            if (isRightHand) handTransform = rightHandTransform;
            else handTransform = leftHandTransform;
            return handTransform;
        }
    }
}