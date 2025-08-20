using System.Collections.Generic;
using UnityEngine;
using Color = _Features.Gameplay.Colors.Color;

namespace _Features.Gameplay.Cookies
{
    [RequireComponent(typeof(SecretStack))]
    public class CookieStack : MonoBehaviour
    {
        [SerializeField] private CookieColor _cookieColor;
        
        [Header("CookieStack settings")]
        [SerializeField] private float _heightAmongCookie;
        [SerializeField] private List<Cookie> _stack;
        
        [Header("Cookie settings")]
        [Space(5)]
        [SerializeField] private Color _color;
        [SerializeField] private GameObject _cookiePrefab;
        
        [Header("Modificators")]
        [Space(5)]
        [SerializeField] private SecretStack _secretStack;
        [SerializeField] private bool _isSecret;
        
        private List<GameObject> _cookiesGM = new List<GameObject>();
        public List<Cookie> Stack => _stack;
        
        private void Start()
        {
            foreach (var cookie in _stack)
            {
                cookie.SetColor(_color);
            }
        }
        
        public void SetColor()
        {
            if (_isSecret) return;
            
            foreach (var cookie in _stack)
            {
                if(cookie == null) continue;
                cookie.SetMaterial(_cookieColor.GetMaterial(_color));
            }
        }

        public void AddLastCookie()
        {
            var gm = Instantiate(_cookiePrefab, transform);
            
            if (_cookiesGM.Count > 0)
            {
                gm.transform.rotation = _cookiesGM[^1].transform.rotation;
                gm.transform.position = _cookiesGM[^1].transform.position + Vector3.up * _heightAmongCookie;
            }

            gm.name = "Cookie";
            _cookiesGM.Add(gm);
            _stack[_cookiesGM.Count-1] = gm.GetComponent<Cookie>();
        }

        public void RemoveLastCookie()
        {
            DestroyImmediate(_cookiesGM[^1]);
            _cookiesGM.RemoveAt(_cookiesGM.Count - 1);
        }

        public void SetSecret()
        {
            if(_isSecret)
                _secretStack.SetSecret(_stack);
        }
    }
}