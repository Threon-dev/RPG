using System;
using RPG.Attributes;
using RPG.Combat;
using UnityEngine;
using RPG.Movement;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float maxNavMeshProjectionDistance = 1f;
        private Health _health;
        private Mover _mover;
        private Fighter _fighter;
        
        [Serializable]
        struct CursorMapping
        {
            public CursorType Type;
            public Texture2D Texture;
            public Vector2 Hotspot;
        }

        [SerializeField] private CursorMapping[] cursorMappings = null;

        private void Awake()
        {
            _mover = GetComponent<Mover>();
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
        }

        private void Update()
        {
            if (InteractWithUI())
            {
                SetCursor(CursorType.UI);
                return;
            }
            if (_health.IsDead())
            { 
                SetCursor(CursorType.None);
                return;
            }

            if (InteractWithComponent()) return;
            if (InteractWithMovement()) return;
            SetCursor(CursorType.None);
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = RaycastAllSorted();
            foreach (var raycastHit in hits)
            {
                IRaycastable[] raycastables = raycastHit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }

            return false;
        }

        private RaycastHit[] RaycastAllSorted()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            float[] distances = new float[hits.Length];
            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }
            Array.Sort(distances,hits);
            return hits;
        }

        private bool InteractWithUI()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }

        private bool InteractWithMovement()
        {
            //RaycastHit hit;
            //bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            Vector3 nearestPosition;
            bool hasHit = RaycastNavmesh(out nearestPosition);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                { 
                    _mover.StartMoveAction(nearestPosition,1f);
                }
                
                SetCursor(CursorType.Movement);
                return true;
            }

            return false;
        }

        private bool RaycastNavmesh(out Vector3 nearestPosition)
        {
            nearestPosition = new Vector3();
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (!hasHit) return false;

            NavMeshHit navMeshHit;
            bool hasCastNavMesh = 
                NavMesh.SamplePosition(hit.point, out navMeshHit, maxNavMeshProjectionDistance, NavMesh.AllAreas);
            if (!hasCastNavMesh) return false;

            nearestPosition = navMeshHit.position;
            return true;
        }
        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.Texture, mapping.Hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            for (int i = 0; i < cursorMappings.Length; i++)
            {
                if (cursorMappings[i].Type == type)
                {
                    return cursorMappings[i];
                }
            }

            return cursorMappings[0];
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
