using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float weaponDamage = 5f;
        [SerializeField] private float timeBetweenAttacks = 1f;
        
        private Health target;
        private Mover _mover;
        private Animator _animator;
        private float timeSinceLastAttack = Mathf.Infinity;

        private void Awake()
        {
            _mover = GetComponent<Mover>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (target != null)
            {
                if (Vector3.Distance(transform.position, target.transform.position) < weaponRange)
                {
                    _mover.Cancel();
                    AttackBehaviour();
                }
                else
                {
                    _mover.MoveTo(target.transform.position);
                }
            }
        }

        private void AttackBehaviour()
        {
            if (timeSinceLastAttack >= timeBetweenAttacks)
            {
                if (target.IsDead)
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
            return targetToTest != null && !targetToTest.IsDead;
        }
        // Animation event
        private void Hit()
        {
            if (target == null) return;
            target.TakeDamage(weaponDamage);
            print("Hit");
        }
        public void StartAttack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
            print("Take that you short, squat peasant!");
        }
        
        public void Cancel()
        {
            StopAttack();
            target = null;
        }
    }
}
