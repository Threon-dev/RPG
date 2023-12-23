using System;
using RPG.Combat;
using TMPro;
using UnityEngine;

namespace RPG.Attributes
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        private Fighter _fighter;

        private void Awake()
        {
            _fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        void Update()
        {
            if (_fighter.GetTarget() == null)
            {
                GetComponent<TextMeshProUGUI>().text = "N/A";
                return;
            }

            Health health = _fighter.GetTarget();
            GetComponent<TextMeshProUGUI>().text = String.Format("{0:0.0}%", health.GetPercentage());
        }
    }
}
