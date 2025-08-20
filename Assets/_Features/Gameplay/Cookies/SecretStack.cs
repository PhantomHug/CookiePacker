using System.Collections.Generic;
using UnityEngine;

namespace _Features.Gameplay.Cookies
{
    public class SecretStack: MonoBehaviour
    {
        [SerializeField] private Material _secretMaterial;
        private bool _isSecret = true;

        public void SetSecret(List<Cookie> cookies)
        {
            foreach (var cookie in cookies)
            {
                cookie.SetMaterial(_secretMaterial);
            }
        }
        /*public void ChangeSecretState()
        {
            _isSecret = !_isSecret;
            foreach (var cookie in _stack)
            {
                cookie.SetBasicMaterial();
            }
        }*/
    }
}