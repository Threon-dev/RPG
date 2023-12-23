using System;
using RPG.Attributes;
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

        private const string weaponName = "Weapon";

        public void LaunchProjectile(Transform rightHand, Transform leftHand,Health target,float damage,GameObject instigator)
        {
            var handTransform = GetHandTransform(rightHand, leftHand);
            Projectile projectileInstance = Instantiate(projectile, handTransform.position,Quaternion.identity);
            projectileInstance.SetTarget(target,instigator,damage);
        }
        public void Spawn(Transform rightHandTransform,Transform leftHandTransform, Animator animator)
        {
            DestroyOldWeapon(rightHandTransform, leftHandTransform);

            if (equippedPrefab != null)
            {
                var handTransform = GetHandTransform(rightHandTransform, leftHandTransform);
                GameObject weapon =  Instantiate(equippedPrefab, handTransform);
                weapon.name = weaponName;
            }
            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (animatorOverride != null)
            { 
                animator.runtimeAnimatorController = animatorOverride;
            }
            else if(overrideController != null)
            { 
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
        }

        private void DestroyOldWeapon(Transform rightHandTransform, Transform leftHandTransform)
        {
            Transform oldWeapon = rightHandTransform.Find(weaponName);
            if (oldWeapon == null)
            { 
                oldWeapon = leftHandTransform.Find(weaponName);
            }

            if (oldWeapon == null)
            {
                return;
            }

            oldWeapon.name = "Destroying";
            Destroy(oldWeapon.gameObject);
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