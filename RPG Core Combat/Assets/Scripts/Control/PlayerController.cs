using RPG.Attributes;
using RPG.Combat;
using UnityEngine;
using RPG.Movement;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private Health _health;
        private Mover _mover;
        private Fighter _fighter;

        private void Awake()
        {
            _mover = GetComponent<Mover>();
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
        }

        private void Update()
        {
            if (_health.IsDead()) return;
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (var raycastHit in hits)
            {
                CombatTarget combatTarget = raycastHit.transform.GetComponent<CombatTarget>();
                
                if(combatTarget == null) continue;
                
                if(!_fighter.CanAttack(combatTarget.gameObject)) continue;
                
                if (Input.GetMouseButton(0))
                {
                    _fighter.StartAttack(combatTarget.gameObject);
                }
                return true;
            }

            return false;
        }
        private bool InteractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                { 
                    _mover.StartMoveAction(hit.point,1f);
                }
                
                return true;
            }

            return false;
        }
        
        private Vector3 GetMouseWorldPoint()
        {
            RaycastHit hit;
            Physics.Raycast(GetMouseRay(), out hit);
            return hit.point;
        }

        private static Ray GetMouseRay()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            return ray;
        }
    }
}
