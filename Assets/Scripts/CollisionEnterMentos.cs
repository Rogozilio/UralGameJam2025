using System;
using UnityEngine;

namespace Scripts
{
    public class CollisionEnterMentos : MonoBehaviour
    {
        public MoveMentos mentos;

        private void OnCollisionEnter(Collision other)
        {
            if (other.transform.CompareTag("Obstacle"))
            {
                mentos.EnableRigidbody();
            }
        }
    }
}