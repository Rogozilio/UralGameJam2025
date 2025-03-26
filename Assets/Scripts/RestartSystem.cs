using System.Linq;
using UnityEngine;

namespace Scripts
{
    public class RestartSystem : MonoBehaviour
    {
        public static void Restart()
        {
            var restartObjects = FindObjectsOfType<MonoBehaviour>().OfType<IRestart>().ToArray();

            Debug.Log("Restart - " + restartObjects.Length);
            
            foreach (IRestart obj in restartObjects)
            {
                obj.Restart();
            }
        }
    }
}