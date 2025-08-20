using System.Collections.Generic;
using UnityEngine;

namespace _Features.Gameplay.Cookies
{
    public class CookieField : MonoBehaviour
    {
        [SerializeField] private List<Conveyor> _conveyors;
        [SerializeField] private GameObject _conveyorPrefab;
        [SerializeField] private Vector3 _stackOffset;
        private int _conveyorsCount;
        public List<Conveyor> Conveyors => _conveyors;

        public void InitConveyorsCount()
        {
            var conveyors = gameObject.transform.GetComponentsInChildren<Conveyor>();
            _conveyorsCount = conveyors.Length;
        }
        
        public int CreateStack()
        {
            var gm = Instantiate(_conveyorPrefab, transform);
            _conveyors[_conveyorsCount] = gm.GetComponent<Conveyor>();
            
            if (_conveyorsCount > 0)
            {
                gm.transform.position = _conveyors[_conveyorsCount-1].transform.position + _stackOffset;
            }

            _conveyorsCount++;
            return _conveyors[_conveyorsCount-1].GetInstanceID();
        }

        public void DestroyConveyor(int stackID)
        {
            for (int i = 0; i < _conveyorsCount; i++)
            {
                var child = gameObject.transform.GetChild(i).GetComponent<Conveyor>();
                if (child.GetInstanceID() == stackID)
                {
                    DestroyImmediate(child.gameObject);
                    _conveyorsCount--;
                    break;
                }
            }
        }
        
        public List<int> GetStackID()
        {
            var listId = new List<int>();
            
            if (_conveyors == null) return listId;
            
            for (int i = 0; i < _conveyors.Count; i++)
            {
                listId.Add(_conveyors[i].GetInstanceID());
            }

            return listId;
        }
    }
}