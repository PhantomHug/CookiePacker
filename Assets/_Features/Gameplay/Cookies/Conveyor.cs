using System.Collections.Generic;
using UnityEngine;

namespace _Features.Gameplay.Cookies
{
    public class Conveyor : MonoBehaviour
    {
        [SerializeField] private List<CookieStack> _cookieStacks;
        [SerializeField] private Transform[] _stackPointsOnConveyor;
        [SerializeField] private GameObject _stackPrefab;
        [SerializeField] private Vector3 _stackOffset;
        private int _stacksCount;
        public List<CookieStack> CookieStack => _cookieStacks;
        
        public void InitStackCount()
        {
            var conveyors = gameObject.transform.GetComponentsInChildren<CookieStack>();
            _stacksCount = conveyors.Length;
        }
        
        public CookieStack GetFirstNotEmptyStack()
        {
            for(int i = 0; i < _cookieStacks.Count; i++)
            {
                if (_cookieStacks[i].Stack.Count != 0)
                {
                    MoveStacksDown(i);
                    /*if (_cookieStacks[i].IsSecret)
                        _cookieStacks[i].ChangeSecretState();*/
                    return _cookieStacks[i];
                }
            }

            return _cookieStacks[0];
        }

        private void MoveStacksDown(int index)
        {
            for (int i = index; i < _stackPointsOnConveyor.Length; i++)
            {
                _cookieStacks[i].transform.position = _stackPointsOnConveyor[i-1].position;
            }
        }

        public int CreateStack()
        {
            var gm = Instantiate(_stackPrefab, transform);
            _cookieStacks[_stacksCount] = gm.GetComponent<CookieStack>();

            if (_stacksCount >= 5)
            {
                gm.transform.position = _cookieStacks[_stacksCount-1].transform.position + _stackOffset;
            }
            else if (_stacksCount >= 0)
            {
                gm.transform.position = _stackPointsOnConveyor[_stacksCount].position;
            }

            _stacksCount++;
            return _cookieStacks[_stacksCount-1].GetInstanceID();
        }

        public void DestroyStack(int stackID)
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                var child = gameObject.transform.GetChild(i).GetComponent<CookieStack>();
                if (child != null && child.GetInstanceID() == stackID)
                {
                    DestroyImmediate(child.gameObject);
                    break;
                }
            }

            return;
        }

        public List<int> GetStackID()
        {
            var listId = new List<int>();
            for (int i = 0; i < _cookieStacks.Count; i++)
            {
                listId.Add(_cookieStacks[i].GetInstanceID());
            }

            return listId;
        }
    }
}