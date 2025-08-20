using System.Collections;
using UnityEngine;
using Color = _Features.Gameplay.Colors.Color;

namespace _Features.Gameplay.Cookies
{
    public class Cookie : MonoBehaviour
    {
        [SerializeField] private CookieColor _cookieData;
        [SerializeField] private MeshRenderer _meshRenderer;

        public CookieState State {get; private set;}

        public enum CookieState
        {
            Active = 0,
            Moving = 1
        }
        
        private Material _material;
        public Color Color { get; private set; }

        private void Awake()
        {
            State = CookieState.Active;
            _material = _meshRenderer.material;
        }
        
        public void SetMaterial(Material material)
        {
            _meshRenderer.material = material;
        }
        
        public IEnumerator Move(Vector3 pos)
        {
            float _time = 0;
            State = CookieState.Moving;
            while (Mathf.Abs((transform.position - pos).magnitude) > 0.1f)
            {
                _time += Time.deltaTime;
                transform.position = Vector3.Lerp(transform.position, pos, _time);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(90,0,0), _time);
                yield return new WaitForEndOfFrame();
            }

            State = CookieState.Active;
        }

        public void SetColor(Color color)
        {
            Color = color;
            _meshRenderer.material = _cookieData.GetMaterial(color);
        }
    }
}