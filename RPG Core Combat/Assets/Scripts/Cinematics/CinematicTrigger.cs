using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinemtics
{
    public class CinematicTrigger : MonoBehaviour
    {
        private bool _isTriggered;
        private void OnTriggerEnter(Collider other)
        {
            if (!_isTriggered)
            {
                if (other.tag == "Player")
                {
                    _isTriggered = true;
                    GetComponent<PlayableDirector>().Play();
                }
            }
        }
    }
}
