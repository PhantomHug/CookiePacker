using UnityEngine;

namespace _Features.Gameplay.Boxes.Modified
{
    public class BlockedBox : MonoBehaviour
    {
        [SerializeField] private bool _isBlocked;
        [SerializeField] private Material _blockedMaterial;
        [SerializeField] private Material _unblockedMaterial;
        [SerializeField] private GameObject _locker;
        [SerializeField] private MeshRenderer _meshRenderer;
        
        public bool IsBlocked => _isBlocked;
        
        private void Start()
        {
            _meshRenderer = _locker.GetComponent<MeshRenderer>();
            _meshRenderer.material = _isBlocked ? _blockedMaterial : _unblockedMaterial;
        }
        
        public void OnPlayersTurn()
        {
            _isBlocked = !_isBlocked;
            _meshRenderer.material = _isBlocked ? _blockedMaterial : _unblockedMaterial;
        }

        public void SetActivateBlocked(bool isActive)
        {
            if (isActive)
            {
                _locker.SetActive(true);
                _meshRenderer.material = _isBlocked ? _blockedMaterial : _unblockedMaterial;
            }
            else
            {
                _locker.SetActive(false);
            }
        }
    }
}