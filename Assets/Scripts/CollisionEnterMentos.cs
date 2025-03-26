using UnityEngine;
using Zenject;

namespace Scripts
{
    public class CollisionEnterMentos : MonoBehaviour
    {
        [Inject] private GameManager _gameManager;
        public MoveMentos mentos;

        private void OnCollisionEnter(Collision other)
        {
            if (other.transform.CompareTag("Obstacle"))
            {
                mentos.EnableRigidbody();
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("asdasdasd");
            if (other.transform.CompareTag("Finish"))
            {
                _gameManager.SwitchGameStep(GameStep.CutsceneMentosMove_MentosFall);
            }
        }
    }
}