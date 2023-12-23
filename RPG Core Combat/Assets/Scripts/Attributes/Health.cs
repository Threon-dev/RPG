using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISavable
    {
        private float _healthPoints = -1f;

        private bool isDead = false;

        private void Start()
        {
            BaseStats baseStats = GetComponent<BaseStats>();
            if (_healthPoints < 0)
            { 
                _healthPoints = baseStats.GetStat(Stat.Health);
            }
            baseStats.OnLevelUp += RegenerateHealth;
        }
        
        public void TakeDamage(GameObject instigator, float damage)
        {
            _healthPoints = Mathf.Max(_healthPoints - damage, 0);
            if (_healthPoints == 0)
            {
                AwardExperience(instigator);
                Die();
            }
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience != null)
            {
                experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
            }
        }

        private void Die()
        {
            if (isDead) return;
            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
        
        private void RegenerateHealth()
        {
            _healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public object CaptureState()
        {
            return _healthPoints;
        }

        public void RestoreState(object state)
        {
            _healthPoints = (float)state;
            
            if (_healthPoints == 0)
            {
                Die();
            }
        }

        public float GetPercentage()
        {
            return 100 * (_healthPoints / GetComponent<BaseStats>().GetStat(Stat.Health));
        }
        public bool IsDead()
        {
            return isDead;
        }
    }
}
