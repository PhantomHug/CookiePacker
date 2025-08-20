using System.Collections.Generic;
using _Features.Gameplay.Cookies;
using UnityEditor;

namespace _Features.Editor
{
    [CustomEditor(typeof(CookieField))]
    public class CookieFieldEditor: UnityEditor.Editor
    {
        private CookieField _cookieField;
        private SerializedProperty _conveyors;
        private List<int> _conveyorsId;
        private List<int> _currConveyorsId;
        
        private void OnEnable()
        {
            _cookieField = (CookieField)target;
            _conveyors = serializedObject.FindProperty("_conveyors");
            _cookieField.InitConveyorsCount();
            _conveyorsId = _cookieField.GetStackID();
            EditorApplication.update += OnEditorUpdate;
        }

        private void OnEditorUpdate()
        {
            serializedObject.Update();

            while (_conveyorsId.Count < _conveyors.arraySize)
            {
                _conveyorsId.Add(_cookieField.CreateStack());
            }

            while (_conveyorsId.Count > _conveyors.arraySize)
            {
                _currConveyorsId = _cookieField.GetStackID();
                foreach (var conveyorId in _conveyorsId)
                {
                    if (!_currConveyorsId.Contains(conveyorId))
                    {
                        _cookieField.DestroyConveyor(conveyorId);
                        _conveyorsId = _cookieField.GetStackID();
                    }
                }
            }
            serializedObject.ApplyModifiedProperties();
        }
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            EditorGUILayout.PropertyField(_conveyors, false);
            serializedObject.ApplyModifiedProperties();
        }
        
        private void OnDisable()
        {
            EditorApplication.update -= OnEditorUpdate;
        }
    }
}