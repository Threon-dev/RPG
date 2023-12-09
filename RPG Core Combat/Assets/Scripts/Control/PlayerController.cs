using RPG.Combat;
using UnityEngine;
using RPG.Movement;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private Mover _mover;
        private Fighter _fighter;

        private void Awake()
        {
            _mover = GetComponent<Mover>();
            _fighter = GetComponent<Fighter>();
        }

        private void Update()
        {
            if (InteractWithCombat()) return;
            InteractWithMovement();
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (var raycastHit in hits)
            {
                CombatTarget combatTarget = raycastHit.transform.GetComponent<CombatTarget>();
                if(combatTarget == null) continue;
                if (Input.GetMouseButtonDown(0))
                {
                    _fighter.Attack(combatTarget);
                }
                return true;
            }

            return false;
        }
        private void InteractWithMovement()
        {
            if (Input.GetMouseButton(0))
            {
                MoveToCursor();
            }
        }
        private void MoveToCursor()
        {
            _mover.MoveTo(GetMouseWorldPoint());
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
