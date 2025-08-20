using UnityEngine;

namespace _Features.Gameplay.Boxes.Modified
{
    public class SecretBox : MonoBehaviour
    {
        [SerializeField] private Material _secretMaterial;
        [SerializeField] private Renderer _meshRenderer;
        private Material _realMaterial;

        public void OnPlayersTurn(bool isActive)
        {
            _meshRenderer.material = isActive ? _realMaterial : _secretMaterial;
        }

        public void InitRealMaterial()
        {
            if(_realMaterial != _secretMaterial)
                _realMaterial = _meshRenderer.sharedMaterial;
        }
        
        public void SetSecret()
        {
            _meshRenderer.sharedMaterial = _secretMaterial;
        }

        public void ChangeMeshRenderer(MeshRenderer meshRenderer)
        {
            _meshRenderer = meshRenderer;
        }
    }
}