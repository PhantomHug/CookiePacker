using System.Collections;
using _Features.Gameplay.Boxes;
using _Features.Gameplay.Boxes.Modified;
using Unity.VisualScripting;
using UnityEngine;

namespace _Features.Gameplay.Cookies
{
    public class CookieCollector : MonoBehaviour
    {
        [SerializeField] private Transform[] _collectorsPositions;
        [SerializeField] private CookieField _cookieField;
        [SerializeField] private BoxField _boxField;
        
        private CookieStack[] _firstStackRow;
        
        private Box[] _boxesInCollector;
        private sbyte _boxesInCollectorCount;
        
        //Сделать update в котором будет проверяться возможность заполнения коробки печеньем
        
        
        /*
         * Сделать класс, отвечающий за ячейку упаковщика.
         * В котором будет коробка и bool значение, в котором указывается пустая ячейка или нет
         */
        
        private void Awake()
        {
            _firstStackRow = new CookieStack[_collectorsPositions.Length];
            _boxesInCollector = new Box[_collectorsPositions.Length];
            
            for (int i = 0; i < _cookieField.Conveyors.Count; i++)
            {
                //Проверить работоспособность при не заполненных полностью стеках
                Debug.Log(_cookieField.Conveyors[i].CookieStack[0]);
                _firstStackRow[i] = _cookieField.Conveyors[i].CookieStack[0];
            }
        }
        
        private void Update()
        {
            RefreshFirstRow();
            //TryAddCookieStack();
        }
        
        //Проверить работоспособность при не заполненных полностью стеках
        private void RefreshFirstRow()
        {
            for (int i = 0; i < _cookieField.Conveyors.Count; i++)
            {
                if (_firstStackRow[i].Stack.Count == 0)
                {
                    if (_firstStackRow[i].Stack == null) break;
                    
                    foreach (var t in _cookieField.Conveyors[i].CookieStack)
                    {
                        _firstStackRow[i] = _cookieField.Conveyors[i].GetFirstNotEmptyStack();
                    }
                }
            }
        }

        //change bool on void
        public bool TryAddBox(Box box)
        {
            StartCoroutine(AddBox(box));
            return true;
        }

        private IEnumerator AddBox(Box newBox)
        {
            if (_boxesInCollectorCount >= _boxesInCollector.Length)
            {
                yield break;
            }
            
            for (int i = 0; i < _boxesInCollector.Length; i++)
            {
                if (_boxesInCollector[i] == null)
                {
                    if (newBox.State is BoxState.Moving or BoxState.Packing)
                        yield break;
                    
                    _boxesInCollectorCount++;
                    _boxesInCollector[i] = newBox;
                    _boxesInCollector[i].Init(OnBoxFull);
                    _boxField.MoveBox(newBox, _collectorsPositions[i].position);
                    _boxField.RemoveBox(newBox);
                    StartCoroutine(_boxField.OnPlayersTurn());
                    while (newBox.State != BoxState.Packing)
                    {
                        yield return new WaitUntil(() => newBox.State == BoxState.Packing);
                        TryAddCookie(i);
                        TryAddCookieStack();
                    }
                    
                    yield break;
                }
            }
        }

        /*private bool AddBox(Box newBox)
        {
            if (_boxesInCollectorCount + 2 >= _boxesInCollector.Length)
                return false;

            return AddBox((Box)newBox) && AddBox((Box)newBox.PairBox);
        }*/
        
        public bool TryRemoveBox(Box box)
        {
            for (int i = 0; i < _boxesInCollector.Length; i++)
            {
                if (_boxesInCollector[i] == box)
                {
                    _boxesInCollector[i] = null;
                    _boxesInCollectorCount--;
                    return true;
                }
            }

            return false;
        }


        private void TryAddCookie(int index)
        {
            if (_boxesInCollector[index] == null) return;
            
            sbyte emptyCount = _boxesInCollector[index].EmptyCount;
            
            foreach (var cookieStack in _firstStackRow)
            {
                if(cookieStack == null) continue;
                
                for (int i = cookieStack.Stack.Count - 1; i >= 0; i--)
                {
                    if (emptyCount == 0 || cookieStack.Stack[i].Color != _boxesInCollector[index].BoxColor) break;

                    switch (_boxesInCollector[index].TryAddCookie(cookieStack.Stack[i]))
                    {
                        case -1:
                            break;
                        case 0:
                            _boxesInCollector[index].MoveCookieToBox(cookieStack.Stack[i]);
                            cookieStack.Stack.RemoveAt(i);
                            emptyCount--;
                            OnBoxFull(_boxesInCollector[index]);
                            break;
                        case 1:
                            _boxesInCollector[index].MoveCookieToBox(cookieStack.Stack[i]);
                            cookieStack.Stack.RemoveAt(i);
                            emptyCount--;
                            break;
                    }
                    
                    
                }
            }
        }
        
        /*private IEnumerator TryAddCookie(int index)
        {
            if (_boxesInCollector[index] == null) yield break;
            
            sbyte emptyCount = _boxesInCollector[index].EmptyCount;
            foreach (var cookieStack in _firstStackRow)
            {
                for (int j = cookieStack.Stack.Count - 1; j >= 0; j--)
                {
                    if (cookieStack.Stack[j] == null || emptyCount == 0) break;

                    if (cookieStack.Stack[j].Color == _boxesInCollector[index].BoxColor)
                    {
                        if (_boxesInCollector[index].CanAddCookie())
                        {
                            _boxesInCollector[index].TryAddCookie(cookieStack.Stack[j]);
                            //yield return new WaitUntil(() => _boxesInCollector[index].TryAddCookie(cookieStack.Stack[j]));
                            emptyCount--;
                            cookieStack.Stack.RemoveAt(j);
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                yield return new WaitForEndOfFrame();
            }
            
        }*/
        
        private void TryAddCookieStack()
        {
            for (int i = 0; i < _boxesInCollector.Length; i++)
            {
                if (_boxesInCollector[i] == null || _boxesInCollector[i].EmptyCount == 0) continue;
                TryAddCookie(i);
            }
        }

        //Возможный баг - добавления новой коробки в позицию старой пока она ждет прилета печенек
        private void OnBoxFull(Box box)
        {
            for (int i = 0; i < _boxesInCollector.Length; i++)
            {
                if (_boxesInCollector[i] == box)
                {
                    StartCoroutine(_boxesInCollector[i].MoveAfterCookieStop());
                    _boxesInCollector[i] = null;
                    _boxesInCollectorCount--;
                    break;
                }
            }
        }

    }
}