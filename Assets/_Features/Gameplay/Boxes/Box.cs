using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Features.Gameplay.Boxes.Modified;
using _Features.Gameplay.Cookies;
using UnityEngine;
using Color = _Features.Gameplay.Colors.Color;

namespace _Features.Gameplay.Boxes
{
    [SelectionBase]
    [RequireComponent(typeof(SecretBox), typeof(BlockedBox), typeof(PairBox))]
    public class Box : MonoBehaviour
    {
        [Header("Box data")]
        [Space(5)]
        [SerializeField] private BoxData _boxData;
        [SerializeField] private Color _color;
        [SerializeField] private Size _size;
        [SerializeField] private float _speed;
        
        [Header("Modificators")]
        [Space(5)]
        [SerializeField] private bool _isSecret;
        [SerializeField] private bool _isBlocked;
        [SerializeField] private bool _isPair;
        
        [Header("Box graphics")]
        [Space(5)]
        [SerializeField] private GameObject _model;
        [SerializeField] private MeshRenderer _meshRenderer;
        
        [Header("Modificators scripts")]
        [Space(5)]
        [SerializeField] private SecretBox _secretBox;
        [SerializeField] private BlockedBox _blockedBox;
        [SerializeField] private PairBox _pairBox;
        
        private List<Transform> _storePoints;
        private sbyte _prevSize;
        private Cookie[] _cookies;
        private sbyte _cookieCount;
        private BoxState _state;
        private Action<Box> _onBoxFull;
        private Func<bool> _onPartialActiveCheck;
        private Collider _collider;
        public bool IsActive { get; private set; }

        public BoxState State => _state;
        public Color BoxColor => _color;
        public sbyte EmptyCount => (sbyte)(_size - _cookieCount);
        
        private void Start()
        {
            //InitMeshRenderer();
            InitStorePoints();
            _cookies = new Cookie[_storePoints.Count];
            _collider = GetComponent<Collider>();
            var size = _boxData.GetColliderSize(_size);
            
            if (size == Vector3.zero)
                Debug.LogError("Box collider size is zero");
            else
                ((BoxCollider)_collider).size = size;
            
            IsActive = CheckIsActive();
        }

        private void InitStorePoints()
        {
            if(_size == 0) _size = _boxData.GetLeastSize();
            
            _storePoints = new List<Transform>();
            foreach (Transform child in _model.transform)
            {
                if (_storePoints.Count >= (sbyte)_size)
                    break;
                if (child.CompareTag("CookieStore"))
                    _storePoints.Add(child);
            }

            if (_storePoints.Count < (sbyte)_size)
            {
                Debug.LogError($"U need to add more store points or change size." +
                               $" Current size is {_size} but count of points is {_storePoints.Count}");
            }
        }
        
        public void InitPairActions(Func<bool> isActive)
        {
            _onPartialActiveCheck = isActive;
        }

        public void Init(Action<Box> onBoxFull)
        {
            _onBoxFull = onBoxFull;
        }
        
        public void OnPlayersTurn()
        {
            IsActive = CheckIsActive();
            if(_isSecret)
                _secretBox.OnPlayersTurn(IsActive);
            else if(_isBlocked)
                _blockedBox.OnPlayersTurn();
        }

        private bool CheckIsActive()
        {
            return !Physics.BoxCast(transform.position, gameObject.transform.lossyScale/2, transform.up, out RaycastHit hit, transform.rotation,
                10f,
                _boxData.Mask);
        }

        public bool IsInteractable()
        {
            if (_onPartialActiveCheck == null)
                return IsActive && !_blockedBox.IsBlocked;
            return _onPartialActiveCheck.Invoke();
        }

        public IEnumerator Move(Vector3 pos)
        {
            _state = BoxState.Moving;
            float _time = 0;
            while (Mathf.Abs((transform.position - pos).magnitude) > 0.1f)
            {
                _time += Time.deltaTime * _speed;
                transform.position = Vector3.Lerp(transform.position, pos, _time);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(-40,0,0), _time);
                yield return new WaitForEndOfFrame();
            }

            _state = BoxState.Packing;
        }

        public bool CanAddCookie()
        {
            return EmptyCount != 0 && _state == BoxState.Packing;
        }

        public sbyte TryAddCookie(Cookie cookie)
        {
            if (EmptyCount == 0 || _state == BoxState.Moving)
                return -1;

            _cookies[_cookieCount] = cookie;
            _cookieCount++;

            return EmptyCount == 0 ? (sbyte)0 : (sbyte)1;
        }

        public void SetColor()
        {
            _meshRenderer.material = _boxData.GetMaterial(_color);
            if (_isSecret)
            {
                _secretBox.ChangeMeshRenderer(_meshRenderer);
                _secretBox.InitRealMaterial();
            }
        }

        public void SetModificator()
        {
            if (_isSecret)
                _secretBox.SetSecret();
            else if (_isBlocked)
                _blockedBox.SetActivateBlocked(_isBlocked);
            else if(_isPair)
                _pairBox.CreateDoubleBox();
            else
            {
                _pairBox.DestroyDoubleBox();
                _blockedBox.SetActivateBlocked(_isBlocked);
            }
        }

        public void SetSize()
        {
            if(_prevSize == 0)
                _prevSize = (sbyte)_size;
            
            //if (Application.isPlaying) return;
            if (_prevSize != (sbyte)_size)
            {
                _prevSize = (sbyte)_size;
                var gm = _boxData.TryGetGameObject(_size);
                if (gm != null)
                {
                    DestroyImmediate(_model.gameObject, true);
                    _model = Instantiate(gm, gameObject.transform);
                    InitMeshRenderer();
                }
                else
                {
                    Debug.LogError($"There is no model of this size = {_size}");
                }
            }
        }

        private void InitMeshRenderer()
        {
            _meshRenderer = _model.GetComponent<MeshRenderer>();
        }

        public void MoveCookieToBox(Cookie cookie)
        {
            StartCoroutine(cookie.Move(_storePoints[_cookieCount-1].position));
            cookie.transform.SetParent(_storePoints[_cookieCount-1]);
        }

        private bool IsCookieMoving()
        {
            return _cookies.All(cookie => cookie.State != Cookie.CookieState.Moving);
        }

        public IEnumerator MoveAfterCookieStop()
        {
            while (!IsCookieMoving())
            {
                yield return new WaitForEndOfFrame();
            }
            StartCoroutine(Move(new Vector3(150, 150, 150)));
        }
    }
}