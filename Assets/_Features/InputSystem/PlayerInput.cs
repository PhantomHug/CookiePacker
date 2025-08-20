using _Features.Gameplay.Boxes;
using _Features.Gameplay.Cookies;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Features.InputSystem
{
    public class PlayerInput : MonoBehaviour
    {
        private InputActions _inputActions;
        [SerializeField] private CookieCollector _cookieCollector;
        [SerializeField] private Camera _camera;
        
        public Vector2 PointerPosition { get; private set; }

        private void Awake()
        {
            _camera = Camera.main;
            _inputActions = new InputActions();
            Binding();
        }

        private void OnEnable()
        {
            _inputActions.Enable();
        }

        private void OnDisable()
        {
            _inputActions.Disable();
        }
        
        private void Binding()
        {
            _inputActions.Player.Tap.performed += OnTap;
        }
        
        private void OnTap(InputAction.CallbackContext context)
        {
            PointerPosition = context.ReadValue<Vector2>();
            Ray ray = _camera.ScreenPointToRay(PointerPosition);

            if (Physics.Raycast(ray, out var hit, Mathf.Infinity))
            {
                Debug.DrawRay(ray.origin, ray.direction * 1000, Color.green, 3);

                if (hit.transform.TryGetComponent<Box>(out var box) && box.IsInteractable())
                {
                    _cookieCollector.TryAddBox(box);
                }

            }
            else
            {
                Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red, 3);
            }
        }
    }
}
