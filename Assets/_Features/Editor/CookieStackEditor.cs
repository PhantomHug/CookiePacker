using _Features.Gameplay.Cookies;
using UnityEditor;

namespace _Features.Editor
{
    [CustomEditor(typeof(CookieStack)), CanEditMultipleObjects]
    public class CookieStackEditor : UnityEditor.Editor
    {
        private CookieStack _cookieStack;
        private SerializedProperty _stack;
        private SerializedProperty _cookiePrefab;
        private int _prevCookieCount;
        
        private void OnEnable()
        {
            _cookieStack = (CookieStack)target;
            _stack = serializedObject.FindProperty("_stack");
            _prevCookieCount = _stack.arraySize;
            EditorApplication.update += OnEditorUpdate;
        }

        //Добавить отслеживание количества удаленных/добавленных элементов
        private void OnEditorUpdate()
        {
            while (_stack.arraySize > _prevCookieCount)
            {
                _prevCookieCount++;
                _cookieStack.AddLastCookie();
            }

            while (_stack.arraySize < _prevCookieCount)
            {
                _prevCookieCount--;
                _cookieStack.RemoveLastCookie();
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            
            EditorGUILayout.PropertyField(_stack, false);
            _cookieStack.SetSecret();
            _cookieStack.SetColor();
            serializedObject.ApplyModifiedProperties();
        }

        private void OnDisable()
        {
            EditorApplication.update -= OnEditorUpdate;
        }
    }
}