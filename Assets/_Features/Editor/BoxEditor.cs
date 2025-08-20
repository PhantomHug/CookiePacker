using _Features.Gameplay.Boxes;
using UnityEditor;
using UnityEngine;

namespace _Features.Editor
{
    [CustomEditor(typeof(Box)), CanEditMultipleObjects]
    public class BoxEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (Application.isPlaying) return;
            ((Box)target).SetColor();
            ((Box)target).SetSize();
            ((Box)target).SetModificator();
        }
    }
}