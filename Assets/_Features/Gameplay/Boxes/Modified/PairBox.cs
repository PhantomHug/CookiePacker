using UnityEngine;

namespace _Features.Gameplay.Boxes.Modified
{
    public class PairBox : MonoBehaviour
    {
        [SerializeField] private GameObject _doubleBoxPrefab;
        [SerializeField] private Vector3 _offset;
        private bool _isCreated;
        
        //BUG: Если вызывается во время игры, выполняется еще раз
        public void CreateDoubleBox()
        {
            if (!_isCreated)
            {
                _isCreated = true;
                var doubleBox = Instantiate(_doubleBoxPrefab, 
                    transform.position + _offset, transform.rotation);
                transform.parent = doubleBox.transform;
            }
        }

        public void DestroyDoubleBox()
        {
            if (_isCreated)
            {
                _isCreated = false;
                var parent = transform.parent;
                transform.parent = null;
                DestroyImmediate(parent.gameObject);
            }
        }
        
        
    }
}