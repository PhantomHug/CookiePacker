using UnityEngine;

namespace _Features.InputService
{
    //когда происходит некоторое событие(нажатие кнопки мыши), делать что-то
    public class InputManager : MonoBehaviour
    {
        private IInputListener _inputListener;
        private InputHandler _inputHandler;
        private void Awake()
        {
            _inputListener = GetComponent<IInputListener>();
            _inputHandler = new InputHandler();
            InitListener();
        }
        
        private void InitListener()
        {
            ((InputListener)_inputListener).OnInputPressed += _inputHandler.SetMouthPositionOnClick;
        }

        
    }
}
