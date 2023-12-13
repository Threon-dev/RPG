using System.Collections;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A,B,C,D,E
        }
        
        [SerializeField] private int sceneIndex = -1;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private DestinationIdentifier destination;
        [SerializeField] private float fadeOut = 1f;
        [SerializeField] private float fadeIn = 1f;
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            if (sceneIndex < 0)
            {
                Debug.LogError("Scene to load not set.");
                yield break;
            }

            Fader fader = FindObjectOfType<Fader>();
            DontDestroyOnLoad(gameObject);
            yield return fader.FadeOut(fadeOut);
            yield return SceneManager.LoadSceneAsync(sceneIndex);
            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);
            yield return fader.FadeIn(fadeIn);
            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
            player.transform.rotation = otherPortal.spawnPoint.rotation;
        }

        private Portal GetOtherPortal()
        {
            foreach (var VARIABLE in  FindObjectsOfType<Portal>())
            {
                if (VARIABLE == this) continue;
                if (VARIABLE.destination != destination) continue;
                return VARIABLE;
            }

            return null;
        }
    }
}
