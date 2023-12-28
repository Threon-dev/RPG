using System.Collections.Generic;
using RPG.Attributes;
using RPG.Core;
using RPG.Movement;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISavable, IModifierProvider
    {
        [SerializeField] private float timeBetweenAttacks = 1f;
        [SerializeField] private Transform rightHandTransform = null;
        [SerializeField] private Transform leftHandTransform = null;
        [SerializeField] private Weapon defaultWeapon = null;
        [SerializeField] private string defaultWeaponName = "Unarmed";
        

        private Health target;
        private Mover _mover;
        private Animator _animator;
        private float timeSinceLastAttack = Mathf.Infinity;
        private Weapon _currentWeapon;

        private void Awake()
        {
            _mover = GetComponent<Mover>();
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            if(_currentWeapon == null) EquipWeapon(defaultWeapon);
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (target != null)
            {
                if (Vector3.Distance(transform.position, target.transform.position) < _currentWeapon.WeaponRange)
                {
                    _mover.Cancel();
                    AttackBehaviour();
                }
                else
                {
                    _mover.MoveTo(target.transform.position,1f);
                }
            }
        }

        public void EquipWeapon(Weapon weapon)
        {
            _currentWeapon = weapon;
            _currentWeapon.Spawn(rightHandTransform,leftHandTransform,_animator);
        }

        public Health GetTarget()
        {
            return target;
        }
        private void AttackBehaviour()
        {
            if (timeSinceLastAttack >= timeBetweenAttacks)
            {
                if (target.IsDead())
                {
                    Cancel();
                    return;
                }
                transform.LookAt(target.transform);
                TriggerAttack();
                timeSinceLastAttack = 0;
            }
        }

        private void TriggerAttack()
        {
            _animator.ResetTrigger("stopAttack");
            _animator.SetTrigger("attack");
        }
        private void StopAttack()
        {
            _animator.ResetTrigger("attack");
            _animator.SetTrigger("stopAttack");
        }

        public bool CanAttack(GameObject combatTarget)
        {
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }
        // Animation event
        private void Hit()
        {
            if (target == null) return;

            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            if (_currentWeapon.HasProjectile)
            {
                _currentWeapon.LaunchProjectile(rightHandTransform,leftHandTransform,target,_currentWeapon.WeaponDamage,gameObject,damage);
            }
            else
            {
                target.TakeDamage(gameObject,damage);
            }
        }
        
        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return _currentWeapon.WeaponDamage;
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return _currentWeapon.PercentageBonus;
            }
        }

        //Animation event
        private void Shoot()
        {
            Hit();
        }
        public void StartAttack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }
        
        public void Cancel()
        {
            StopAttack();
            target = null;
            _mover.Cancel();
        }

        public object CaptureState()
        {
            return _currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            Weapon weapon = Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }
    }
}
