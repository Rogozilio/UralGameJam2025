using UnityEngine;
using Zenject;

namespace Scripts
{
    public class CollisionEnterMentos : MonoBehaviour
    {
        [Inject] private GameManager _gameManager;
        [Inject] private UIControll _uiControll;
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
            if (other.transform.CompareTag("Finish"))
            {
                _gameManager.SwitchGameStep(GameStep.CutsceneMentosMove_MentosFall);
                _uiControll.DisableAll();
            }
        }
    }
}