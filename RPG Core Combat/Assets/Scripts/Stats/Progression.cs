using System;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(menuName = "Create Progression", fileName = "Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] private ProgressionCharacterClass[] characterClasses = null;

        public float GetHealth(CharacterClass characterClass,int level)
        {
            foreach (var progressionClass in characterClasses)
            {
                if (progressionClass.characterClass == characterClass)
                {
                    return progressionClass.health[level - 1];
                }
            }
            return 0;
        }
        
        [Serializable]
        public class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public float[] health;
        }
    }
}
