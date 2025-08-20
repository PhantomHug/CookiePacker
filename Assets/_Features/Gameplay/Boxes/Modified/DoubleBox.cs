using System.Collections.Generic;
using UnityEngine;

namespace _Features.Gameplay.Boxes.Modified
{
    [SelectionBase]
    public class DoubleBox : MonoBehaviour
    {
        private List<Box> _subBoxes;

        private void Start()
        {
            _subBoxes = new List<Box>();
            GetComponentsInChildren(_subBoxes);
            foreach (var box in _subBoxes)
            {
                box?.InitPairActions(CheckIsActive);
            }
        }
        
        private bool CheckIsActive()
        {
            foreach (var box in _subBoxes)
            {
                if (box?.IsActive == false)
                    return false;
            }
            return true;
        }
    }
}
