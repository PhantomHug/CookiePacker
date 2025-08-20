using System.Collections.Generic;
using _Features.Gameplay.Cookies;
using UnityEditor;

namespace _Features.Editor
{
    [CustomEditor(typeof(Conveyor))]
    public class ConveyorEditor : UnityEditor.Editor
    {
        private Conveyor _conveyor;
        private SerializedProperty _cookieStacks;
        
        private List<int> _cookieStacksId;
        private List<int> _currCookieStacksId;
        
        private void OnEnable()
        {
            _conveyor = (Conveyor)target;
            _cookieStacks = serializedObject.FindProperty("_cookieStacks");
            _conveyor.InitStackCount();
            _cookieStacksId = _conveyor.GetStackID();
            
            EditorApplication.update += OnEditorUpdate;
        }
        
        private void OnEditorUpdate()
        {
            serializedObject.Update();
            
            while (_cookieStacksId.Count < _cookieStacks.arraySize)
            {
                _cookieStacksId.Add(_conveyor.CreateStack());
            }

            while (_cookieStacksId.Count > _cookieStacks.arraySize)
            {
                _currCookieStacksId = _conveyor.GetStackID();
                foreach (var _stackId in _cookieStacksId)
                {
                    if (!_currCookieStacksId.Contains(_stackId))
                    {
                        _conveyor.DestroyStack(_stackId);
                        _cookieStacksId = _conveyor.GetStackID();
                    }
                }
            }
            serializedObject.ApplyModifiedProperties();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            EditorGUILayout.PropertyField(_cookieStacks, false);
            serializedObject.ApplyModifiedProperties();
        }

        private void OnDisable()
        {
            EditorApplication.update -= OnEditorUpdate;
        }
        
    }
}