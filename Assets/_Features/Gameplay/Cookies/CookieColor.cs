using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Color = _Features.Gameplay.Colors.Color;

namespace _Features.Gameplay.Cookies
{
    [CreateAssetMenu(fileName = "CookieColors", menuName = "CookieColors")]
    public class CookieColor : ScriptableObject
    {
        [SerializeField] private List<Colors> _colors;

        public Material GetMaterial(Color color)
        {
            return _colors.First((col) => col.color == color).material;
        }
    }

    [Serializable]
    public struct Colors
    {
        public Color color;
        public Material material;
    }
}