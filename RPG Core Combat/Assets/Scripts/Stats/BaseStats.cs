using System;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1,99)]
        [SerializeField] private int startingLevel = 1;
        [SerializeField] private CharacterClass characterClass;
        [SerializeField] private Progression progression = null;
        [SerializeField] private GameObject levelUpParticleEffect;
        [SerializeField] private bool shouldUseModifiers;

        public event Action OnLevelUp;

        private int currentLevel = 0;

        private void Start()
        {
            currentLevel = CalculateLevel();
            Experience experience = GetComponent<Experience>();
            if (experience != null)
            {
                experience.OnExperienceGained += UpdateLevel;
            }
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel)
            {
                currentLevel = newLevel;
                OnLevelUp?.Invoke();
                LevelUpEffect();
            }
        }

        private void LevelUpEffect()
        {
            Instantiate(levelUpParticleEffect, transform);
        }

        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) *  (1 + GetPercentageModifier(stat)/100);
        }
        

        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(characterClass,stat, GetLevel());
        }

        public int GetLevel()
        {
            if (currentLevel < 1)
            {
                currentLevel = CalculateLevel();
            }
            return currentLevel;
        }
        private float GetAdditiveModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;
            float sum = 0;
            foreach (var provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetAdditiveModifiers(stat))
                {
                    sum += modifier;
                }
            }

            return sum;
        }
        
        private float GetPercentageModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;
            float sum = 0;
            foreach (var provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetPercentageModifiers(stat))
                {
                    sum += modifier;
                }
            }

            return sum;
        }
        private int CalculateLevel()
        {
            Experience experience = GetComponent<Experience>();
            if (experience == null) return startingLevel;
            float currentXP = experience.GetExperience();
            int PenultimteLevel = progression.GetLevels(characterClass,Stat.ExperienceToLevelUp);
            for (int level = 1; level < PenultimteLevel; level++)
            {
                float XPToLevelUp = progression.GetStat(characterClass, Stat.ExperienceToLevelUp, level);
                if (XPToLevelUp > currentXP)
                {
                    return level;
                }
            }

            return PenultimteLevel + 1;
        }
    }
}
