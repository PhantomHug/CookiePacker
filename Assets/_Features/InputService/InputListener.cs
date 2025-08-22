using System;
using UnityEngine;

namespace _Features.InputService
{
    //генерирует событие по нажатии на заданные кнопки
    //добавить "выбор конфигураций" в зависимости от платформ. Для ПК(лкм, пкм) и Мобилки (тап)
    public class InputListener : MonoBehaviour, IInputListener
    {
        [SerializeField] private KeyCode _key = KeyCode.Mouse0;
        public event Action OnInputPressed;
        
        private void Update()
        {
            if (Input.GetKeyDown(_key))
            {
                OnInputPressed?.Invoke();
            }
        }
        
    }
}