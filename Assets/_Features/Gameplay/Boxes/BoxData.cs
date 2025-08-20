using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Color = _Features.Gameplay.Colors.Color;

namespace _Features.Gameplay.Boxes
{
    [CreateAssetMenu(fileName = "Box data", menuName = "Boxes/Box Data")]
    public class BoxData : ScriptableObject
    {
        [SerializeField] private LayerMask _boxMask;
        [SerializeField] private List<BoxColor> _colors;
        [Tooltip("Max cookie count storage in box:\nX1 = 4, X2 = 6, X3 = 8")]
        [SerializeField] private List<BoxSize> _sizes;
        public LayerMask Mask => _boxMask;
        public List<BoxSize> BoxSize => _sizes;
        
        public Material GetMaterial(Color color)
        {
            return _colors.First(col => col.color == color).material;
        }

        public GameObject TryGetGameObject(Size size)
        {
            var gm = _sizes.FirstOrDefault(s => s.size == size).boxModel;
            return gm == null? null : gm;
        }

        public Size GetLeastSize()
        {
            return _sizes.Min(size => size.size);
        }

        public Vector3 GetColliderSize(Size size)
        {
            var gm = _sizes.FirstOrDefault(s => s.size == size).colliderSize;
            return gm;
        }
    }

    [Serializable]
    public struct BoxColor
    {
        public Color color;
        public Material material;
    }

    [Serializable]
    public struct BoxSize
    {
        public Size size;
        public GameObject boxModel;
        public Vector3 colliderSize;
    }

    public enum Size:sbyte
    {
        X1 = 4,
        X2 = 6,
        X3 = 8,
    }
}