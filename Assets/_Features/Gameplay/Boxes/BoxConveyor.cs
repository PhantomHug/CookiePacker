using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Features.Gameplay.Boxes
{
    public class BoxConveyor : MonoBehaviour
    {
        [SerializeField] private Box[] _boxes;
        [SerializeField] private Transform _startMovement;
        [SerializeField] private Transform _endMovement;
        [SerializeField] private Vector3 _checkerSize;
        [SerializeField] private float _speed;
        [SerializeField] private LayerMask _boxMask;
        [SerializeField] private float _boxSpawnTimer;
        private Queue<GameObject> _queuedBoxes;
        
        private void Awake()
        {
            _queuedBoxes = new Queue<GameObject>();
            foreach (var box in _boxes)
            {
                _queuedBoxes.Enqueue(box.gameObject);
                box.gameObject.SetActive(false);
                box.transform.position = _startMovement.position;
                box.transform.rotation = Quaternion.identity;
                //box.Init(OnBoxRemoved);
                //box.Init(OnBoxRemovedFromArray);
            }
        }

        private void Start()
        {
            StartCoroutine(CheckStartMovement());
            StartCoroutine(Move());
        }

        private void OnBoxRemoved()
        {
            
        }

        private bool OnBoxRemovedFromArray(Box removedBox)
        {
            for (int i = 0; i < _boxes.Length; i++)
            {
                if (_boxes[i] == removedBox)
                {
                    _boxes[i] = null;
                    return true;
                }
            }

            return false;
        }
        
        private void FixedUpdate()
        {
            CheckEndMovement();
            //CheckStartMovement();
        }

        private IEnumerator Move()
        {
            while (true)
            {
                foreach (var box in _boxes)
                {
                    if (box == null || !box.gameObject.activeSelf)
                        continue;
                    box.transform.position += Vector3.right * _speed * Time.deltaTime;
                }
                yield return new WaitForEndOfFrame();
            }
        }
        
        private void CheckEndMovement()
        {
            if (Physics.BoxCast(_endMovement.position, _checkerSize / 2, -_endMovement.right, out RaycastHit hit,
                    _endMovement.rotation,
                .1f, _boxMask
                ))
            {
                _queuedBoxes.Enqueue(hit.transform.gameObject);
                hit.transform.position = _startMovement.position;
                hit.transform.gameObject.SetActive(false);
            }
        }

        private IEnumerator CheckStartMovement()
        {
            while (true)
            {
                if (_queuedBoxes.Count != 0)
                {
                    _queuedBoxes.Dequeue().SetActive(true);
                    yield return new WaitForSecondsRealtime(_boxSpawnTimer);
                }
                else
                {
                    yield return new WaitForEndOfFrame();
                }
            }
        }
        
        private void OnDrawGizmosSelected()
        {
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(_endMovement.position, _endMovement.rotation, _endMovement.lossyScale);
            Gizmos.matrix = rotationMatrix;
            Gizmos.DrawWireCube(-_endMovement.right, _checkerSize);
        }
    }
}