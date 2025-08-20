using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Features.Gameplay.Boxes
{
    public class BoxField: MonoBehaviour
    {
        [SerializeField] private List<Box> _field;

        private void OnEnable()
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                _field.Add(gameObject.transform.GetChild(i).GetComponent<Box>());
            }
        }
        
        public IEnumerator OnPlayersTurn()
        {
            yield return new WaitForSeconds(0.2f);
            foreach (var box in _field)
            {
                box?.OnPlayersTurn();
            }
            yield return null;
        }

        public void MoveBox(Box box, Vector3 position)
        {
            StartCoroutine(box.Move(position));
        }

        public void RemoveBox(Box removedBox)
        {
            for (int i = 0; i < _field.Count; i++)
            {
                if (_field[i] == removedBox)
                {
                    _field.RemoveAt(i);
                    return;
                }
            }
        }
        
        /*public bool UpdateBlockedBoxesState()
        {
            if (_blockedBoxes.Length == 0)
                return false;
            foreach (var secretBox in _blockedBoxes)
            {
                secretBox.OnPlayersMove();
            }
            return true;
        }

        public void RemoveBlockedBox(BaseBox box)
        {
            foreach (var secretBox in _blockedBoxes)
            {
                if(secretBox == box)
                    secretBox.Locker.SetActive(false);
            }
        }*/
    }
}