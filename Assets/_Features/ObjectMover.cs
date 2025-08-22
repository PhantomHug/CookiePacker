using UnityEngine;

namespace _Features
{
    public class ObjectMover: MonoBehaviour
    {
        [SerializeField] private float _speed;
        
        public void Move(Vector3 position)
        {
            Debug.Log("move");
        }
    }
}