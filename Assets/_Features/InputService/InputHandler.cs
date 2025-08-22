using UnityEngine;

namespace _Features.InputService
{
    public class InputHandler
    {
        private readonly Camera _mainCamera = Camera.main;

        public void SetMouthPositionOnClick()
        {
            Ray _ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(_ray, out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent<Box>(out Box box))
                {
                    box.Move();
                }
                
            }
        }
    }
}