using System;
using UnityEngine;

public enum ThemeColor
{
    PanelPrimary,
    PanelSecondary,

    Background,

    TextPrimary,
    TextSecondary,
    TextSpecial,

    InBubble,
    OutBubble,
}

[CreateAssetMenu(fileName = "Theme", menuName = "Scriptable Objects/Theme", order = -1000)]
public class Theme : ScriptableObject
{
    private static Theme _current;
    public static Theme Current => _current;

    [SerializeField] private Color[] _colors;

    public Color this[ThemeColor color] => _colors[(int)color];

    public static event Action OnThemeChange;

    public void SetCurrent()
    {
        _current = this;
        OnThemeChange?.Invoke();
    }
}
