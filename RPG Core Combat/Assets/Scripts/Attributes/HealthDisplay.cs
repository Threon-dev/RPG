using System;
using TMPro;
using UnityEngine;


namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        private Health _health;

        private void Awake()
        {
            _health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }
        void Update()
        {
            GetComponent<TextMeshProUGUI>().text = String.Format("{0:0}/{1:0}", _health.GetHealthPoints(),_health.GetMaxHealthPoints());
        }
    }
}