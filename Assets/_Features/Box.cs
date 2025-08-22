using System;
using UnityEngine;

namespace _Features
{
    public class Box : MonoBehaviour
    {
        private ObjectMover _mover;

        private void Awake()
        {
            _mover = GetComponent<ObjectMover>();
        }

        public void Move()
        {
            _mover.Move(Vector3.zero);
        }
    }
}