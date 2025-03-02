using Unity.VectorGraphics;
using UnityEngine;

[RequireComponent(typeof(SVGImage))]
public class ColoredSVG : MonoBehaviour
{
    [SerializeField] private SVGImage _image;
    [SerializeField] private ThemeColor _color;

    private void OnEnable()
    {
        UpdateColor();
        Theme.OnThemeChange += UpdateColor;
    }

    private void OnDisable()
    {
        Theme.OnThemeChange -= UpdateColor;
    }

    private void UpdateColor()
    {
        _image.color = Theme.Current[_color];
    }
}